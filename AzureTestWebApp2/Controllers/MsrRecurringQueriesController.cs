using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using AzureTestWebDataLayer;

namespace AzureTestWebApp2.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using AzureTestWebDataLayer;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<MsrRecurringQuery>("MsrRecurringQueries");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MsrRecurringQueriesController : ODataController
    {
        private AzureTestDBEntities db = new AzureTestDBEntities();

         //GET: odata/MsrRecurringQueries
        [EnableQuery]
        public IQueryable<MsrRecurringQuery> GetMsrRecurringQueries()
        {
            return db.MsrRecurringQueries;
        }

        //[EnableQuery]
        //public IQueryable<Record> GetMsrRecurringQueries()
        //{
        //    try
        //    {
        //        var queries = db.MsrRecurringQueries.ToList();
        //        List<Record> recordAlls = new List<Record>();
        //        Record recordAll = new Record();
        //        recordAll.Records = new List<Records>();
        //        foreach (MsrRecurringQuery recurringQuery in queries)
        //        {
        //            Records records = new Records();
        //            RecordAttributes recordAttribute = new RecordAttributes()
        //            {

        //                type = "Report",
        //                url = "/api/MsrRecurringQueryApi/" + recurringQuery.RecurringQueryID
        //            };
        //            records.Values = recordAttribute;
        //            records.Name = recurringQuery.RecurringQueryName;
        //            records.Id = recurringQuery.RecurringQueryID.ToString();
        //            recordAll.Records.Add(records);

        //        }
        //        recordAll.Done = true;
        //        recordAll.TotalSize = recordAll.Records.Count;
        //        recordAlls.Add(recordAll);
        //        return recordAlls.Select(c => c).AsQueryable<Record>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // GET: odata/MsrRecurringQueries(5)
        [EnableQuery]
        public SingleResult<MsrRecurringQuery> GetMsrRecurringQuery([FromODataUri] int key)
        {
            return SingleResult.Create(db.MsrRecurringQueries.Where(msrRecurringQuery => msrRecurringQuery.RecurringQueryID == key));
        }

        // PUT: odata/MsrRecurringQueries(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<MsrRecurringQuery> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MsrRecurringQuery msrRecurringQuery = db.MsrRecurringQueries.Find(key);
            if (msrRecurringQuery == null)
            {
                return NotFound();
            }

            patch.Put(msrRecurringQuery);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MsrRecurringQueryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(msrRecurringQuery);
        }

        // POST: odata/MsrRecurringQueries
        public IHttpActionResult Post(MsrRecurringQuery msrRecurringQuery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MsrRecurringQueries.Add(msrRecurringQuery);
            db.SaveChanges();

            return Created(msrRecurringQuery);
        }

        // PATCH: odata/MsrRecurringQueries(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<MsrRecurringQuery> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MsrRecurringQuery msrRecurringQuery = db.MsrRecurringQueries.Find(key);
            if (msrRecurringQuery == null)
            {
                return NotFound();
            }

            patch.Patch(msrRecurringQuery);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MsrRecurringQueryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(msrRecurringQuery);
        }

        // DELETE: odata/MsrRecurringQueries(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            MsrRecurringQuery msrRecurringQuery = db.MsrRecurringQueries.Find(key);
            if (msrRecurringQuery == null)
            {
                return NotFound();
            }

            db.MsrRecurringQueries.Remove(msrRecurringQuery);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MsrRecurringQueryExists(int key)
        {
            return db.MsrRecurringQueries.Count(e => e.RecurringQueryID == key) > 0;
        }
    }
}
