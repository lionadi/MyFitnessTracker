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
    public class ExerciseRecordsController : Controller
    {
        String userID = SessionHelper.LoggedInUser<AspNetUser>().Id;
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: ExerciseRecords
        public async Task<ActionResult> Index()
        {
            
            var exerciseRecords = db.ExerciseRecords.Include(e => e.Exercise).Where(o => o.Exercise.Set.UserId.ToLower().CompareTo(userID.ToLower()) == 0);
            return View(await exerciseRecords.ToListAsync());
        }

        // GET: ExerciseRecords/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);
            if (exerciseRecord == null)
            {
                return HttpNotFound();
            }
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name");
            var model = new ExerciseRecord();
            model.Date = DateTime.Now;
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;
            model.Record = 0;
            return View(model);
        }

        // POST: ExerciseRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Record,Date,StartDate,EndDate,ExerciseId")] ExerciseRecord exerciseRecord)
        {
            if (ModelState.IsValid)
            {
                db.ExerciseRecords.Add(exerciseRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseRecord.ExerciseId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);
            if (exerciseRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseRecord.ExerciseId);
            return View(exerciseRecord);
        }

        // POST: ExerciseRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Record,Date,StartDate,EndDate,ExerciseId")] ExerciseRecord exerciseRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exerciseRecord).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseRecord.ExerciseId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);
            if (exerciseRecord == null)
            {
                return HttpNotFound();
            }
            return View(exerciseRecord);
        }

        // POST: ExerciseRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            ExerciseRecord exerciseRecord = await db.ExerciseRecords.FindAsync(id);
            db.ExerciseRecords.Remove(exerciseRecord);
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
