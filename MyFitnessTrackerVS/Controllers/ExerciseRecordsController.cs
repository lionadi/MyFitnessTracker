using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyFitnessTrackerVS;

namespace MyFitnessTrackerVS.Controllers
{
    public class ExerciseRecordsController : Controller
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: ExerciseRecords
        public ActionResult Index()
        {
            var exerciseRecords = db.ExerciseRecords.Include(e => e.Exercise);
            return View(exerciseRecords.ToList());
        }

        // GET: ExerciseRecords/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = db.ExerciseRecords.Find(id);
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
            return View();
        }

        // POST: ExerciseRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Record,Date,StartDate,EndDate,ExerciseId")] ExerciseRecord exerciseRecord)
        {
            if (ModelState.IsValid)
            {
                db.ExerciseRecords.Add(exerciseRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseRecord.ExerciseId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = db.ExerciseRecords.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Record,Date,StartDate,EndDate,ExerciseId")] ExerciseRecord exerciseRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exerciseRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseRecord.ExerciseId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseRecord exerciseRecord = db.ExerciseRecords.Find(id);
            if (exerciseRecord == null)
            {
                return HttpNotFound();
            }
            return View(exerciseRecord);
        }

        // POST: ExerciseRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ExerciseRecord exerciseRecord = db.ExerciseRecords.Find(id);
            db.ExerciseRecords.Remove(exerciseRecord);
            db.SaveChanges();
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
