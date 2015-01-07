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
    public class ExercisesController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: api/Exercises
        public IQueryable<Exercise> GetExercises()
        {
            return db.Exercises.Where(o => o.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);
        }

        // GET: api/Exercises/5
        [ResponseType(typeof(Exercise))]
        public async Task<IHttpActionResult> GetExercise(long id)
        {
            Exercise exercise = await db.Exercises.FindAsync(id);
            if (exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                exercise = null;

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        // PUT: api/Exercises/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExercise(long id, Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exercise.Id)
            {
                return BadRequest();
            }

            if (db.Entry(exercise).Entity.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                return NotFound();

            db.Entry(exercise).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
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

        // POST: api/Exercises
        [ResponseType(typeof(Exercise))]
        public async Task<IHttpActionResult> PostExercise(Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exercises.Add(exercise);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = exercise.Id }, exercise);
        }

        // DELETE: api/Exercises/5
        [ResponseType(typeof(Exercise))]
        public async Task<IHttpActionResult> DeleteExercise(long id)
        {
            Exercise exercise = await db.Exercises.FindAsync(id);
            
            if (exercise.Set.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) != 0)
                exercise = null;

            if (exercise == null)
            {
                return NotFound();
            }

            db.Exercises.Remove(exercise);
            await db.SaveChangesAsync();

            return Ok(exercise);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseExists(long id)
        {
            return db.Exercises.Count(e => e.Id == id) > 0;
        }
    }
}