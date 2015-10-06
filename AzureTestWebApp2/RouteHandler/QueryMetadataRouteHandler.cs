using AzureTestWebApp2.HttpHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AzureTestWebApp2.RouteHandler
{
    public class QueryMetadataRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new QueryMetadataHttpHandler(requestContext.HttpContext);
        }
    }
}