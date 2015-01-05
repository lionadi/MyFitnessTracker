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
using System.Web.Http.Description;
using MyFitnessTrackerWebAPI;
using MyFitnessTrackerLibrary.Globals;

namespace MyFitnessTrackerWebAPI.Controllers
{
    [Helpers.AuthenticationActionFilterHelper]
    [Authorize]
    public class SetsController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/Sets
        public IQueryable<Set> GetSets()
        {
            var sets = db.Sets.Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);
            return sets;
        }

        // GET: api/Sets/5
        [ResponseType(typeof(Set))]
        public async Task<IHttpActionResult> GetSet(long id)
        {
            Set set = await db.Sets.FindAsync(id);

            if (set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                set = null;

            if (set == null)
            {
                return NotFound();
            }

            return Ok(set);
        }

        // PUT: api/Sets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSet(long id, Set set)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != set.Id)
            {
                return BadRequest();
            }

            if (db.Entry(set).Entity.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                return NotFound();

            db.Entry(set).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sets
        [ResponseType(typeof(Set))]
        public async Task<IHttpActionResult> PostSet(Set set)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sets.Add(set);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = set.Id }, set);
        }

        // DELETE: api/Sets/5
        [ResponseType(typeof(Set))]
        public async Task<IHttpActionResult> DeleteSet(long id)
        {
            Set set = await db.Sets.FindAsync(id);
            if (set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                set = null;

            if (set == null)
            {
                return NotFound();
            }

            db.Sets.Remove(set);
            await db.SaveChangesAsync();

            return Ok(set);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SetExists(long id)
        {
            return db.Sets.Count(e => e.Id == id) > 0;
        }
    }
}