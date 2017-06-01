using System.Web.Mvc;

namespace Framework.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "Admin", id = UrlParameter.Optional },
                new[] { "Framework.Admin.Controllers" }
            );
            //context.Routes.Add("DomainRouteForMutiWebSite",
            //    new DomainRoute(
            //    "{area}.{domain}",                             // {area}作为二级域名
            //    "{controller}/{action}/{id}",                  // URL with parameters
            //    new
            //    {
            //        area = "Admin",
            //        controller = "Home",
            //        action = "Index",
            //        id = UrlParameter.Optional,
            //        Namespaces = new string[] { "Framework.Admin.Controllers" }
            //    }  // Parameter defaults
            //));
        }
    }
}
