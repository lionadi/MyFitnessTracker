using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyFitnessTrackerWebAPI;
using MyFitnessTrackerLibrary.Globals;

namespace MyFitnessTrackerWebAPI.Controllers
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    public class AspNetUsersController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/AspNetUsers
        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            if (!User.IsInRole("Admin"))
                return db.AspNetUsers.Where(o => o.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);

            return db.AspNetUsers;
        }

        // Get the user either gui GUID ID or by email
        [Authorize(Roles = "Admin")]
        public AspNetUser GetUser(string id)
        {
            if (id == null)
            {
                return null;
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                aspNetUser = db.AspNetUsers.SingleOrDefault(o => o.Email.ToLower().CompareTo(id.ToLower()) == 0);
                if (aspNetUser == null)
                {
                    return null;
                }
            }
            return aspNetUser;
        }

        // GET: api/AspNetUsers/5
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult GetAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);

            if (!User.IsInRole("Admin") && aspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                aspNetUser = null;

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return Ok(aspNetUser);
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUser(string id, AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetUser.Id)
            {
                return BadRequest();
            }

            if (db.Entry(aspNetUser).Entity.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                return NotFound();

            db.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        // POST: api/AspNetUsers
        [ResponseType(typeof(AspNetUser))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostAspNetUser(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetUser.Id }, aspNetUser);
        }

        // DELETE: api/AspNetUsers/5
        [ResponseType(typeof(AspNetUser))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return Ok(aspNetUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}