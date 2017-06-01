using System;
using System.Linq;
using System.Collections.Generic;
using Framework.Core.Configuration;
using Framework.Core.Infrastructure.DependencyManagement;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Framework.Core.Infrastructure.TypeFinder;

namespace Framework.Core.Infrastructure
{
    /// <summary>
    /// Engine
    /// </summary>
    public class FrameEngine : IEngine
    {

        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(WebConfig config)
        {
            //register dependencies
            RegisterDependencies(config);

            //模型寄存器映射器配置 => 不使用Mapper进行codeFirst可以删除
            /*RegisterMapperConfiguration(config);*/

            //startup tasks
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Run startup tasks
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            //从容器中获得 类型查找器
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            //查询IStartupTask的实现类
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            //将查出来的实现类创建实例对象存储至集合中
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //排序集合中的实例
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            //按顺序执行实例的启动任务
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(WebConfig config)
        {
            //创建Autofac容器构造器
            var builder = new ContainerBuilder();
            //获得一个Autofac容器
            var container = builder.Build();
            //初始化容器管理类
            this._containerManager = new ContainerManager(container);

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.
            //dependencies

            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder();

            //注入 WebConfig、IEngine、ITyoeFinder
            builder.RegisterInstance(config).As<WebConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            //register dependencies provided by other assemblies
            //查找IDependencyRegistrar的实现类，并创建 每个查出来的实现类的实例 添加到drInstances集合中
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));

            //sort 对查出来的实现类 Order=>进行排序 =>按顺序进行注册. 差不多就是先主后副
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();

            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder, config);
            builder.Update(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        /*/// <summary>
        /// 模型寄存器映射器注册
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterMapperConfiguration(WebConfig config)
        {
            //dependencies
            var typeFinder = new WebAppTypeFinder();

            //register mapper configurations provided by other assemblies
            var mcTypes = typeFinder.FindClassesOfType<IMapperConfiguration>();
            var mcInstances = new List<IMapperConfiguration>();
            foreach (var mcType in mcTypes)
                mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
            //sort
            mcInstances = mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //get configurations
            var configurationActions = new List<Action<IMapperConfigurationExpression>>();
            foreach (var mc in mcInstances)
                configurationActions.Add(mc.GetConfiguration());
            //register
            AutoMapperConfiguration.Init(configurationActions);
        }*/


        #endregion

    }
}
