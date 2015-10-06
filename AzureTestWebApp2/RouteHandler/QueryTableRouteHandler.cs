using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace AzureTestWebApp2.RouteHandler
{
    public class QueryTableRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string queryId = requestContext.RouteData.Values["queryid"].ToString();
            return new QueryTableHttpHandler(requestContext.HttpContext, queryId);
        }
    }
}