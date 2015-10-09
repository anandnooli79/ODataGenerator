using AzureTestWebDataLayer;
using Microsoft.Data.OData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AzureTestWebApp2.Controllers
{

    public class MsrRecurringQueryApisController : ApiController
    {
        AzureTestDBEntities db = new AzureTestDBEntities();

        //public IQueryable<MsrRecurringQuery> GetQueries()
        //{
        //    return db.MsrRecurringQueries;
        //}
        public Record GetQueries()
        {
            try
            {
                var queries = db.MsrRecurringQueries.ToList();
                Record recordAll = new Record();
                recordAll.Records = new List<Records>();
                foreach (MsrRecurringQuery recurringQuery in queries)
                {
                    Records records = new Records();
                    RecordAttributes recordAttribute = new RecordAttributes()
                    {

                        type = "Report",
                        url = "/api/MsrRecurringQueryApi/" + recurringQuery.RecurringQueryID
                    };
                    records.Values = recordAttribute;
                    records.Name = recurringQuery.RecurringQueryName;
                    records.Id = recurringQuery.RecurringQueryID.ToString();
                    recordAll.Records.Add(records);

                }
                recordAll.Done = true;
                recordAll.TotalSize = recordAll.Records.Count;
                return recordAll;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public IQueryable<MsrRecurringQuery> GetQueries(int id)
        {
            return db.MsrRecurringQueries.Where(c => c.RecurringQueryID == id);
        }

    }


    public class Record
    {

        [JsonProperty("totalSize")]
        public int TotalSize
        {
            get;
            set;
        }

        [JsonProperty("done")]
        public bool Done
        {
            get;
            set;
        }

        [JsonProperty("records")]
        public List<Records> Records
        {
            get;
            set;
        }
    }

    public class RecordAttributes
    {
        public string type
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;

        }
    }

    public class Records
    {
        [JsonProperty("attributes")]
        public RecordAttributes Values
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }


        public string Id
        {
            get;
            set;
        }

        
    }
}
