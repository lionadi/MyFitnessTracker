﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyFitnessTracker.Models;

namespace MyFitnessTracker.Controllers
{
    public class SetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sets
        public ActionResult Index()
        {
            return View(db.Sets.ToList());
        }

        // GET: Sets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = db.Sets.Find(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            return View(set);
        }

        // GET: Sets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SetId,Name")] Set set)
        {
            if (ModelState.IsValid)
            {
                db.Sets.Add(set);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set);
        }

        // GET: Sets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = db.Sets.Find(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            return View(set);
        }

        // POST: Sets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SetId,Name")] Set set)
        {
            if (ModelState.IsValid)
            {
                db.Entry(set).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set);
        }

        // GET: Sets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Set set = db.Sets.Find(id);
            if (set == null)
            {
                return HttpNotFound();
            }
            return View(set);
        }

        // POST: Sets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Set set = db.Sets.Find(id);
            db.Sets.Remove(set);
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