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
using MyFitnessTrackerLibrary.SignalRLogic;

namespace MyFitnessTrackerVS.Controllers
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    public class ExercisesController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: Exercises
        public async Task<ActionResult> Index()
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Exercise operation.");
            var exercises = db.Exercises.Include(e => e.Set).Where(o => o.Set.UserId.ToLower().CompareTo(user.Id.ToLower()) == 0);
            return View(await exercises.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Exercise operation.");
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

            ViewBag.SetId = new SelectList(db.Sets.Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Name");
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Target,SetId")] Exercise exercise)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Create Exercise operation.");
            if (ModelState.IsValid)
            {
                db.Exercises.Add(exercise);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SetId = new SelectList(db.Sets.Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Name", exercise.SetId);
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
            ViewBag.SetId = new SelectList(db.Sets.Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Name", exercise.SetId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Target,SetId")] Exercise exercise)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Update Exercise operation.");
            if (ModelState.IsValid)
            {
                db.Entry(exercise).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SetId = new SelectList(db.Sets.Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Name", exercise.SetId);
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
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Delete Exercise operation.");
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
