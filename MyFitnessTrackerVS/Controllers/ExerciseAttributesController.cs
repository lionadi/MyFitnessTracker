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
    [Helpers.AuthenticationActionFilterHelper]
    public class ExerciseAttributesController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: ExerciseAttributes
        public async Task<ActionResult> Index()
        {

            
            var exerciseAttributes = db.ExerciseAttributes.Include(e => e.Exercise).Where(o => o.Exercise.Set.UserId.ToLower().CompareTo(user.Id.ToLower()) == 0);
            return View(await exerciseAttributes.ToListAsync());
        }

        // GET: ExerciseAttributes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            if (exerciseAttribute == null)
            {
                return HttpNotFound();
            }
            return View(exerciseAttribute);
        }

        // GET: ExerciseAttributes/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name");
            return View();
        }

        // POST: ExerciseAttributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AttributeID,Name,Data,ExerciseId")] ExerciseAttribute exerciseAttribute)
        {
            if (ModelState.IsValid)
            {
                db.ExerciseAttributes.Add(exerciseAttribute);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseAttribute.ExerciseId);
            return View(exerciseAttribute);
        }

        // GET: ExerciseAttributes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            if (exerciseAttribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseAttribute.ExerciseId);
            return View(exerciseAttribute);
        }

        // POST: ExerciseAttributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AttributeID,Name,Data,ExerciseId")] ExerciseAttribute exerciseAttribute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exerciseAttribute).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "Id", "Name", exerciseAttribute.ExerciseId);
            return View(exerciseAttribute);
        }

        // GET: ExerciseAttributes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            if (exerciseAttribute == null)
            {
                return HttpNotFound();
            }
            return View(exerciseAttribute);
        }

        // POST: ExerciseAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            ExerciseAttribute exerciseAttribute = await db.ExerciseAttributes.FindAsync(id);
            db.ExerciseAttributes.Remove(exerciseAttribute);
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
