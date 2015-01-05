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
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    public class ExerciseRecordsController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/ExerciseRecords
        public IQueryable<ExerciseRecord> GetExerciseRecords()
        {
            return db.ExerciseRecords.Where(o => o.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);
        }

        // GET: api/ExerciseRecords/5
        [ResponseType(typeof(ExerciseRecord))]
        public async Task<IHttpActionResult> GetExerciseRecord(long id)
        {
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);
            if (exerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                exerciseRecord = null;

            if (exerciseRecord == null)
            {
                return NotFound();
            }

            return Ok(exerciseRecord);
        }

        // PUT: api/ExerciseRecords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExerciseRecord(long id, ExerciseRecord exerciseRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exerciseRecord.Id)
            {
                return BadRequest();
            }

            if (db.Entry(exerciseRecord).Entity.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                return NotFound();

            db.Entry(exerciseRecord).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseRecordExists(id))
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

        // POST: api/ExerciseRecords
        [ResponseType(typeof(ExerciseRecord))]
        public async Task<IHttpActionResult> PostExerciseRecord(ExerciseRecord exerciseRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExerciseRecords.Add(exerciseRecord);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = exerciseRecord.Id }, exerciseRecord);
        }

        // DELETE: api/ExerciseRecords/5
        [ResponseType(typeof(ExerciseRecord))]
        public async Task<IHttpActionResult> DeleteExerciseRecord(long id)
        {
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);

            if (exerciseRecord.Exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0)
                exerciseRecord = null;

            if (exerciseRecord == null)
            {
                return NotFound();
            }

            db.ExerciseRecords.Remove(exerciseRecord);
            await db.SaveChangesAsync();

            return Ok(exerciseRecord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseRecordExists(long id)
        {
            return db.ExerciseRecords.Count(e => e.Id == id) > 0;
        }
    }
}