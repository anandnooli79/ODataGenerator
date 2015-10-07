using AzureTestWebDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Formatter;


namespace AzureTestWebApp2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<MsrRecurringQuery>("MsrRecurringQueries");
            config.Routes.MapODataRoute(
            routeName: "ODataRoute",
            routePrefix: "odata",
            model: builder.GetEdmModel());

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.UseDataContractJsonSerializer = true;

            //var odataFormatters = ODataMediaTypeFormatters.Create();
            //odataFormatters = odataFormatters.Where(
            //    f => f.SupportedMediaTypes.Any(
            //        m => m.MediaType == "application/atom+xml" ||
            //            m.MediaType == "application/atomsvc+xml")).ToList();

            //config.Formatters.Clear();
            //config.Formatters.AddRange(odataFormatters);



        }
    }
}
