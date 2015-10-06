using AzureTestWebApp2.RouteHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AzureTestWebApp2.RouteHandler
{
    public class ScheduleQueryRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new QueryTableHttpHandler(requestContext.HttpContext, null);
        }
    }
}