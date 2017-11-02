using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using VotersEDM;

namespace VotersEDM.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using VotersEDM;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ncvoter40>("ncvoter40");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ncvoter40Controller : ODataController
    {
        private VotersDBEntities db = new VotersDBEntities();

        // GET: odata/ncvoter40
        [EnableQuery]
        public IQueryable<ncvoter40> Getncvoter40()
        {
            return db.ncvoter40;
        }

        // GET: odata/ncvoter40(5)
        [EnableQuery]
        public SingleResult<ncvoter40> Getncvoter40([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ncvoter40.Where(ncvoter40 => ncvoter40.ID == key));
        }

        // PUT: odata/ncvoter40(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ncvoter40> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ncvoter40 ncvoter40 = await db.ncvoter40.FindAsync(key);
            if (ncvoter40 == null)
            {
                return NotFound();
            }

            patch.Put(ncvoter40);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ncvoter40Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ncvoter40);
        }

        // POST: odata/ncvoter40
        public async Task<IHttpActionResult> Post(ncvoter40 ncvoter40)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            ncvoter40.ID = Guid.NewGuid();  //added by developer
            db.ncvoter40.Add(ncvoter40);
            await db.SaveChangesAsync();

            return Created(ncvoter40);
        }

        // PATCH: odata/ncvoter40(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<ncvoter40> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ncvoter40 ncvoter40 = await db.ncvoter40.FindAsync(key);
            if (ncvoter40 == null)
            {
                return NotFound();
            }

            patch.Patch(ncvoter40);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ncvoter40Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ncvoter40);
        }

        // DELETE: odata/ncvoter40(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            ncvoter40 ncvoter40 = await db.ncvoter40.FindAsync(key);
            if (ncvoter40 == null)
            {
                return NotFound();
            }

            db.ncvoter40.Remove(ncvoter40);
            await db.SaveChangesAsync();

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

        private bool ncvoter40Exists(Guid key)
        {
            return db.ncvoter40.Count(e => e.ID == key) > 0;
        }
    }
}
