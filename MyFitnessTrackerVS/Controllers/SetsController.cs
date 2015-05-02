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
using MyFitnessTrackerLibrary.SignalRLogic;


namespace MyFitnessTrackerVS.Controllers
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    public class SetsController : ControllerBase
    {
        private MyFitnessTrackerDBEntities db = new MyFitnessTrackerDBEntities();

        // GET: Sets
        public async Task<ActionResult> Index()
        {

            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Set operation.");
            var sets = db.Sets.Include(s => s.AspNetUser).Where(o => o.AspNetUser.Id.ToLower().CompareTo(user.Id.ToLower()) == 0);
            return View(await sets.ToListAsync());
        }

        // GET: Sets/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Set operation.");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = await db.Sets.FindAsync(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            return View(set);
        }

        // GET: Sets/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                ViewBag.UserId = new SelectList(db.AspNetUsers.Where(o => o.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Email");
            else
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");

            var model = new Set();
            return View(model);
        }

        // POST: Sets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,UserId")] Set set)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Create Set operation.");
            await HubGateway.GetInstance().IsDataUpdateRequiredForMobileClient(user.Email, true, "Client Mobile Application needs to update UI with new data from server.");
            if (ModelState.IsValid)
            {
                db.Sets.Add(set);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (!User.IsInRole("Admin"))
                ViewBag.UserId = new SelectList(db.AspNetUsers.Where(o => o.Id.ToLower().CompareTo(user.Id.ToLower()) == 0), "Id", "Email", set.UserId);
            else
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", set.UserId);
            return View(set);
        }

        // GET: Sets/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Set operation.");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = await db.Sets.FindAsync(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", set.UserId);
            return View(set);
        }

        // POST: Sets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,UserId")] Set set)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Update Set operation.");
            await HubGateway.GetInstance().IsDataUpdateRequiredForMobileClient(user.Email, true, "Client Mobile Application needs to update UI with new data from server.");
            if (ModelState.IsValid)
            {
                db.Entry(set).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", set.UserId);
            return View(set);
        }

        // GET: Sets/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Get Set operation.");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = await db.Sets.FindAsync(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            return View(set);
        }

        // POST: Sets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await HubGateway.GetInstance().SendNormalMessage(user.Email, "Delete Set operation.");
            await HubGateway.GetInstance().IsDataUpdateRequiredForMobileClient(user.Email, true, "Client Mobile Application needs to update UI with new data from server.");
            Set set = await db.Sets.FindAsync(id);
            db.Sets.Remove(set);
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
