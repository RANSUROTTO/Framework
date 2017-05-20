using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace Framework.Core.Infrastructure.DependencyManagement
{
    public class ContainerManager
    {

        /// <summary>
        /// 容器对象
        /// </summary>
        public virtual IContainer Container { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">Conainer</param>
        public ContainerManager(IContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// 分解
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 分解
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 分解多个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// 分解已注册服务
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// 分解已注册服务
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new Exception("未知的依赖");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            throw new Exception("没有发现依赖项的构造函数");
        }

        /// <summary>
        /// 尝试分解
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <param name="instance">分解服务</param>
        /// <returns>分解是否成功</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// 检查指定服务是否已注册 (允许分离)
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// 可选择分解
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围；通过null来自动解析当前的范围</param>
        /// <returns>分解服务</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// 获取当前的生命周期范围
        /// </summary>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }


    }
}
