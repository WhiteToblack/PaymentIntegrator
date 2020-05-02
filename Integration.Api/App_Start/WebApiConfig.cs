using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Integrator.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            // Web API yapılandırması ve hizmetleri

            // Web API yolları
            config.MapHttpAttributeRoutes();            
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "Configurations", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
