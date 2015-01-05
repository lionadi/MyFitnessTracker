using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyFitnessTrackerVS;
using MyFitnessTrackerLibrary.Globals;

namespace MyFitnessTrackerVS.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        String userID = SessionHelper.LoggedInUser<AspNetUser>().Id;
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: Exercises
        public async Task<ActionResult> Index()
        {
            
            var exercises = db.Exercises.Include(e => e.Set).Where(o => o.Set.UserId.ToLower().CompareTo(userID.ToLower()) == 0);
            return View(await exercises.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercise exercise = await db.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            return View(exercise);
        }

        // GET: Exercises/Create
        public ActionResult Create()
        {
            ViewBag.SetId = new SelectList(db.Sets, "Id", "Name");
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Target,SetId")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                db.Exercises.Add(exercise);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SetId = new SelectList(db.Sets, "Id", "Name", exercise.SetId);
            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercise exercise = await db.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            ViewBag.SetId = new SelectList(db.Sets, "Id", "Name", exercise.SetId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Target,SetId")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exercise).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SetId = new SelectList(db.Sets, "Id", "Name", exercise.SetId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercise exercise = await db.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Exercise exercise = await db.Exercises.FindAsync(id);
            db.Exercises.Remove(exercise);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
