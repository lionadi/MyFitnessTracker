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

namespace MyFitnessTrackerWebAPI.Controllers
{
    [Authorize]
    public class ExerciseAttributesController : ApiController
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/ExerciseAttributes
        public IQueryable<ExerciseAttribute> GetExerciseAttributes()
        {
            return db.ExerciseAttributes;
        }

        // GET: api/ExerciseAttributes/5
        [ResponseType(typeof(ExerciseAttribute))]
        public async Task<IHttpActionResult> GetExerciseAttribute(long id)
        {
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            if (exerciseAttribute == null)
            {
                return NotFound();
            }

            return Ok(exerciseAttribute);
        }

        // PUT: api/ExerciseAttributes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExerciseAttribute(long id, ExerciseAttribute exerciseAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exerciseAttribute.Id)
            {
                return BadRequest();
            }

            db.Entry(exerciseAttribute).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseAttributeExists(id))
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

        // POST: api/ExerciseAttributes
        [ResponseType(typeof(ExerciseAttribute))]
        public async Task<IHttpActionResult> PostExerciseAttribute(ExerciseAttribute exerciseAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExerciseAttributes.Add(exerciseAttribute);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = exerciseAttribute.Id }, exerciseAttribute);
        }

        // DELETE: api/ExerciseAttributes/5
        [ResponseType(typeof(ExerciseAttribute))]
        public async Task<IHttpActionResult> DeleteExerciseAttribute(long id)
        {
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            if (exerciseAttribute == null)
            {
                return NotFound();
            }

            db.ExerciseAttributes.Remove(exerciseAttribute);
            await db.SaveChangesAsync();

            return Ok(exerciseAttribute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseAttributeExists(long id)
        {
            return db.ExerciseAttributes.Count(e => e.Id == id) > 0;
        }
    }
}