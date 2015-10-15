using AzureTestWebApp2.Controllers;
using AzureTestWebApp2.HttpHandler;
using AzureTestWebDataLayer;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Atom;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AzureTestWebApp2.RouteHandler
{
    public class QueryTableHttpHandler : IHttpHandler
    {
        public string QueryId { get; private set; }
        public HttpContextBase ContextBase { get; private set; }

        public QueryTableHttpHandler(HttpContextBase context, string queryId)
        {
            this.ContextBase = context;
            this.QueryId = queryId;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = HttpContext.Current.Request.FilePath }, "OAuth2Bearer");
                return;
            }
            MSRAHttpResponseMessage message = new MSRAHttpResponseMessage(this.ContextBase.Response);
            message.StatusCode = 200;
            message.SetHeader(ODataConstants.ContentTypeHeader, "application/json");
            // create the writer, indent for readability
            ODataMessageWriterSettings messageWriterSettings = new ODataMessageWriterSettings()
            {
                Indent = true,
                CheckCharacters = false,
                BaseUri = context.Request.Url

            };
            messageWriterSettings.SetContentType(ODataFormat.Json);
           messageWriterSettings.SetMetadataDocumentUri(new Uri("http://localhost:31435/odata/MSRAQuery/$metadata"));

            if (string.IsNullOrEmpty(QueryId))
            {
                AzureTestDBEntities db = new AzureTestDBEntities();
                var queries = db.MsrRecurringQueries.ToList().Take(1);
                
                ODataWorkspace workSpace = new ODataWorkspace();
                var collections = new List<ODataResourceCollectionInfo>();

                foreach (MsrRecurringQuery recurringQuery in queries)
                {

                    ODataResourceCollectionInfo collectionInfo = new ODataResourceCollectionInfo()
                    {
                        Name = "MsrRecurringQueries",
                        Url = new Uri(context.Request.Url+"data/" + recurringQuery.RecurringQueryID.ToString(), UriKind.Absolute)
                    };
                    collectionInfo.SetAnnotation<AtomResourceCollectionMetadata>(new AtomResourceCollectionMetadata()
                    {
                        Title = new AtomTextConstruct()
                        {
                            Text = "MsrRecurringQueries"//recurringQuery.RecurringQueryName
                        },
                    });
                    collections.Add(collectionInfo);


                }
                workSpace.Collections = collections.AsEnumerable<ODataResourceCollectionInfo>();

                using (ODataMessageWriter messageWriter = new ODataMessageWriter(message, messageWriterSettings))
                {
                    messageWriter.WriteServiceDocumentAsync(workSpace);
                }
            }

            else
            {
                EdmModel mainModel =(EdmModel) QueryMetadataHttpHandler.BuildODataModel();
                using (ODataMessageWriter messageWriter = new ODataMessageWriter(message, messageWriterSettings, mainModel))
                {
                    var msrRecurringQueryResultType = new EdmEntityType("mainNS", "MsrRecurringQuery", null);
                    IEdmPrimitiveType edmPrimitiveType1 = new MSRAEdmPrimitiveType("Int32", "Edm", EdmPrimitiveTypeKind.Int32, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    IEdmPrimitiveType edmPrimitiveType2 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    IEdmPrimitiveType edmPrimitiveType3 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    IEdmPrimitiveType edmPrimitiveType4 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    IEdmPrimitiveType edmPrimitiveType5 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    IEdmPrimitiveType edmPrimitiveType6 = new MSRAEdmPrimitiveType("Decimal", "Edm", EdmPrimitiveTypeKind.Decimal, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
                    msrRecurringQueryResultType.AddKeys(new EdmStructuralProperty(msrRecurringQueryResultType, "RowId", new EdmPrimitiveTypeReference(edmPrimitiveType1, false)));
                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "RowId", new EdmPrimitiveTypeReference(edmPrimitiveType1, false)));

                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Pricing_Level", new EdmPrimitiveTypeReference(edmPrimitiveType2, false)));
                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Business_Summary", new EdmPrimitiveTypeReference(edmPrimitiveType3, false)));
                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Future_Flag", new EdmPrimitiveTypeReference(edmPrimitiveType4, false)));
                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Fiscal_Month", new EdmPrimitiveTypeReference(edmPrimitiveType5, false)));
                    msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "MS_Sales_Amount_Const", new EdmPrimitiveTypeReference(edmPrimitiveType6, false)));
                    ODataWriter feedWriter = messageWriter.CreateODataFeedWriter(
                        mainModel.EntityContainers().Select(c => c.EntitySets().First()).First(), msrRecurringQueryResultType);
                    ODataFeed feed = new ODataFeed()
                    {
                        Id = "MsrRecurringQueries",
                        
                    };

                    feedWriter.WriteStart(feed);

                    AzureTestDBEntities db = new AzureTestDBEntities();
                    var queries = db.T_annooli_231161891 ;
                    foreach (var recurringQuery in queries)
                    {
                        ODataEntry entry = this.GetODataEntry(recurringQuery);
                        feedWriter.WriteStart(entry);
                        feedWriter.WriteEnd();
                    }
                    feedWriter.WriteEnd();
                }
            }
        }


        private ODataEntry GetODataEntry(T_annooli_231161891  recurringQuery)
        {
            ODataEntry entry = new ODataEntry();
            entry.TypeName = "mainNS.MsrRecurringQuery";
           
            List<ODataProperty> properties = new List<ODataProperty>();
            ODataProperty property = new ODataProperty()
            {
                Name = "RowId",
                Value = recurringQuery.RowId,
            };
            
            properties.Add(property);
            property = new ODataProperty()
            {
                Name = "Pricing_Level",
                Value = recurringQuery.Pricing_Level,
            };

            properties.Add(property);
            property = new ODataProperty()
            {
                Name = "Business_Summary",
                Value = recurringQuery.Business_Summary,
            };

            properties.Add(property);

            property = new ODataProperty()
            {
                Name = "Future_Flag",
                Value = recurringQuery.Future_Flag,
            };

            properties.Add(property);

            property = new ODataProperty()
            {
                Name = "Fiscal_Month",
                Value = recurringQuery.Fiscal_Month,
            };

            properties.Add(property);

            property = new ODataProperty()
            {
                Name = "MS_Sales_Amount_Const",
                Value = recurringQuery.MS_Sales_Amount_Const,
            };

            properties.Add(property);
            //ODataProperty property1 = new ODataProperty()
            //{
            //    Name = "AttributeFilters",
            //    Value = recurringQuery.AttributeFilters ,
            //};
            //properties.Add(property1);

            entry.Properties = properties;
            return entry;
        }
    }


    public class MSRAHttpResponseMessage : IODataResponseMessageAsync
    {
        private const string JsonContentType = @"application/json";
        private const string AtomContentType = @"application/atom+xml";
        private const string XmlContentType = @"application/xml";

        private HttpResponseBase httpResponse;
        private bool lockedHeaders = false;

        public MSRAHttpResponseMessage(HttpResponseBase httpResponse)
        {
            this.httpResponse = httpResponse;
        }

        public Task<Stream> GetStreamAsync()
        {
            lockedHeaders = true;
            TaskCompletionSource<Stream> completionSource = new TaskCompletionSource<Stream>();
            completionSource.SetResult(this.httpResponse.OutputStream);
            return completionSource.Task;
        }

        public string GetHeader(string headerName)
        {
            return this.httpResponse.Headers[headerName];
        }

        public Stream GetStream()
        {
            return this.httpResponse.OutputStream;
        }

        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get
            {
                foreach (string headerName in this.httpResponse.Headers.Keys)
                {
                    yield return new KeyValuePair<string, string>(headerName, this.httpResponse.Headers[headerName]);
                }
            }
        }

        public void SetHeader(string headerName, string headerValue)
        {
            if (lockedHeaders)
            {
                throw new ODataException("Cannot set headers they have already been written to the stream");
            }

            if (headerName.ToUpperInvariant() == ODataConstants.ContentTypeHeader.ToUpperInvariant())
            {
                this.httpResponse.ContentType = headerValue;
            }
            else
            {
                this.httpResponse.Headers[headerName] = headerValue;
            }
        }

        public int StatusCode
        {
            get
            {
                return (int)this.httpResponse.StatusCode;
            }
            set
            {
                this.httpResponse.StatusCode = value;
            }
        }
    }
}