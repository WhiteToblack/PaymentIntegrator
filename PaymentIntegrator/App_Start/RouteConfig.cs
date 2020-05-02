using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PaymentIntegrator.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "{controller}", action = "{action}", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "Entry",
                 url: "Entry/{userId}",
                 defaults: new { controller = "Entry", action = "Payment", userId = UrlParameter.Optional }
             );

            routes.MapRoute(
                 name: "AltinbasEntry",
                 url: "AltinbasEntry/{action}/{association}/{source}/{transactionMessageId}",
                 defaults: new { controller = "AltinbasEntry", action = "{action}", userId = UrlParameter.Optional }
             );
        }
    }
}
