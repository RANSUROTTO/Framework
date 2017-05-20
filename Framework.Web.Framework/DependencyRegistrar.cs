using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System.Web;
using Framework.Core.Caching;
using Framework.Core.Configuration;
using Framework.Core.Data;
using Framework.Core.Data.Providers;
using Framework.Core.Fakes;
using Framework.Core.Infrastructure.DependencyManagement;
using Framework.Core.Infrastructure.TypeFinder;
using Framework.Data;
using Framework.Data.Repository;

namespace Framework.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config)
        {
            //注册Http上下文及其相关内容
            builder.Register(c =>
                //当HttpContext不可用时，注册FakeHttpContext => 单元测试模拟Web允许环境
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //注册控制器
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //注册数据层
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings(); //从配置文件中读取数据配置
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            //默认提供 EF数据库管理 填充 BaseDataProviderManager  //更换orm可修改
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();
            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                //注册数据库上下文
                //builder.Register<IDbContext>(c => new FrameDbContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                //直接注册数据库上下文
                //builder.Register<IDbContext>(c => new FrameDbContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
            }

            //表操作封装实现
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //注册缓存管理

            if (false) //全局缓存 如果有使用Redis则不使用内置的MemoryCache.
            {
                //注册Reids缓存服务

            }
            else
            {
                //注册MemoryCache缓存管理
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_static").SingleInstance();
            }
            //注册页面级缓存PerRequestCache
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_per_request").InstancePerLifetimeScope();





        }

        /// <summary>
        /// 依赖注册实现顺序
        /// </summary>
        public int Order
        {
            get { return 0; }
        }

    }
}
