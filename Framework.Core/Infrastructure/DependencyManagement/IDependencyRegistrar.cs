using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Framework.Core.Configuration;
using Framework.Core.Infrastructure.TypeFinder;

namespace Framework.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注册接口 > 一般来说添加插件只要继承该接口写一个依赖注册实现就好了
    /// Dependency registrar interface
    /// </summary>
    public interface IDependencyRegistrar
    {

        /// <summary>
        /// 注册服务和接口
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config);

        /// <summary>
        /// 注册顺序
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }

    }
}
