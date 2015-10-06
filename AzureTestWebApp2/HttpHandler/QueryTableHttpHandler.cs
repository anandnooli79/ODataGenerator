using AzureTestWebApp2.Controllers;
using AzureTestWebDataLayer;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Atom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            MSRAHttpResponseMessage message = new MSRAHttpResponseMessage(this.ContextBase.Response);
            message.StatusCode = 200;
            message.SetHeader(ODataConstants.ContentTypeHeader, "application/atom+xml");
            // create the writer, indent for readability
            ODataMessageWriterSettings messageWriterSettings = new ODataMessageWriterSettings()
            {
                Indent = true,
                CheckCharacters = false,
                BaseUri = context.Request.Url

            };

           
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
                       // Url = new Uri("http://localhost:31435/api/MsrRecurringQueryApis/" + recurringQuery.RecurringQueryID.ToString(), UriKind.Absolute)
                        Url = new Uri(context.Request.Url.ToString()+"/data/" + recurringQuery.RecurringQueryID.ToString(), UriKind.Absolute)
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
                    messageWriter.WriteServiceDocument(workSpace);
                }
            }

            else
            {
                using (ODataMessageWriter messageWriter = new ODataMessageWriter(message, messageWriterSettings))
                {
                    ODataWriter feedWriter = messageWriter.CreateODataFeedWriter();
                    ODataFeed feed = new ODataFeed()
                    {
                        Id = "MsrRecurringQueries",
                        
                    };

                    feedWriter.WriteStart(feed);

                    AzureTestDBEntities db = new AzureTestDBEntities();
                    var queries = db.MsrRecurringQueries.ToList().Where(c=>c.RecurringQueryID == Convert.ToInt32(this.QueryId)) ;
                    foreach (MsrRecurringQuery recurringQuery in queries)
                    {
                        ODataEntry entry = this.GetODataEntry(recurringQuery);
                        feedWriter.WriteStart(entry);
                        feedWriter.WriteEnd();
                    }
                    feedWriter.WriteEnd();
                }
            }
        }

        private ODataEntry GetODataEntry(MsrRecurringQuery recurringQuery)
        {
            ODataEntry entry = new ODataEntry();
            entry.TypeName = "System.Type";
           
            List<ODataProperty> properties = new List<ODataProperty>();
            ODataProperty property = new ODataProperty()
            {
                Name = "RecurringQueryID",
                Value = recurringQuery.RecurringQueryID,
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