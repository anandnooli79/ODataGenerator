using AzureTestWebApp2.RouteHandler;
using AzureTestWebApp2.RouteHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AzureTestWebApp2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.Add("ScheduleQuery", new Route("odata/MSRAQuery/", new ScheduleQueryRouteHandler()));
            routes.Add("QueryTable", new Route("odata/MSRAQuery/data/{queryid}", new QueryTableRouteHandler()));
            routes.Add("QueryMetadata", new Route("odata/MSRAQuery/$metadata", new QueryMetadataRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
