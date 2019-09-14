using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyFirstMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("HakkimdaRoute", "Hakkimda", new { controller = "Home", action = "About" },
             namespaces: new string[] { "MyFirstMVC.Controllers" });
           
            routes.MapRoute("ProjelerimRoute", "Projelerim", new { controller = "Home", action = "Projects" },
             namespaces: new string[] { "MyFirstMVC.Controllers" });
          
            routes.MapRoute("İletisimRoute", "İletisim", new { controller = "Home", action = "Contact" },
             namespaces: new string[] { "MyFirstMVC.Controllers" });
            
            routes.MapRoute("KvkkRoute", "Kvkk", new { controller = "Home", action = "Kvkk" },
             namespaces: new string[] { "MyFirstMVC.Controllers" });
                
            routes.MapRoute("PrivacyPolicyRoute", "PrivacyPolicy", new { controller = "Home", action = "PrivacyPolicy" },
             namespaces: new string[] { "MyFirstMVC.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MyFirstMVC.Controllers"}
            );
        }
    }
}
