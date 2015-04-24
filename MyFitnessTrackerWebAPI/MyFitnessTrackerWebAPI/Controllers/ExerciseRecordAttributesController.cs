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
    [Helpers.AuthenticationActionFilterHelper]
    public class ExerciseRecordAttributesController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/ExerciseRecordAttributes
        public IQueryable<ExerciseRecordAttribute> GetExerciseRecordAttributes()
        {
            return db.ExerciseRecordAttributes.Where(o => o.ExerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);
        }

        // GET: api/ExerciseRecordAttributes/5
        [ResponseType(typeof(ExerciseRecordAttribute))]
        public async Task<IHttpActionResult> GetExerciseRecordAttribute(long id)
        {
            ExerciseRecordAttribute exerciseRecordAttribute = await db.ExerciseRecordAttributes.FindAsync(id);

            if (exerciseRecordAttribute.ExerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                exerciseRecordAttribute = null;
            
            if (exerciseRecordAttribute == null)
            {
                return NotFound();
            }

            return Ok(exerciseRecordAttribute);
        }

        // PUT: api/ExerciseRecordAttributes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExerciseRecordAttribute(long id, ExerciseRecordAttribute exerciseRecordAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exerciseRecordAttribute.Id)
            {
                return BadRequest();
            }

            if (db.Entry(exerciseRecordAttribute).Entity.ExerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                return NotFound();

            db.Entry(exerciseRecordAttribute).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseRecordAttributeExists(id))
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

        // POST: api/ExerciseRecordAttributes
        [ResponseType(typeof(ExerciseRecordAttribute))]
        public async Task<IHttpActionResult> PostExerciseRecordAttribute(ExerciseRecordAttribute exerciseRecordAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExerciseRecordAttributes.Add(exerciseRecordAttribute);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = exerciseRecordAttribute.Id }, exerciseRecordAttribute);
        }

        // DELETE: api/ExerciseRecordAttributes/5
        [ResponseType(typeof(ExerciseRecordAttribute))]
        public async Task<IHttpActionResult> DeleteExerciseRecordAttribute(long id)
        {
            ExerciseRecordAttribute exerciseRecordAttribute = await db.ExerciseRecordAttributes.FindAsync(id);

            if (exerciseRecordAttribute.ExerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                exerciseRecordAttribute = null;

            if (exerciseRecordAttribute == null)
            {
                return NotFound();
            }

            db.ExerciseRecordAttributes.Remove(exerciseRecordAttribute);
            await db.SaveChangesAsync();

            return Ok(exerciseRecordAttribute);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseRecordAttributeExists(long id)
        {
            return db.ExerciseRecordAttributes.Count(e => e.Id == id) > 0;
        }
    }
}