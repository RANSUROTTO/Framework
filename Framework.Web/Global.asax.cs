using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Core.Infrastructure;

namespace Framework.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //移除 X-AspNetMvc-Version 请求头
            MvcHandler.DisableMvcResponseHeader = true;

            //初始化引擎上下文
            EngineContext.Initialize(false);

            //清空视图引擎仅使用Razor视图引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }



        /// <summary>
        /// 向客户端发送Http标头之前执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //移除 Server 请求头
            HttpApplication app = sender as HttpApplication;
            app?.Context?.Response.Headers.Remove("Server");
        }

    }
}
