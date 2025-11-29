using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_Management.Models;

namespace Project_Management.Controllers
{
    public class AdminfaqcategoriesController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Adminfaqcategories
        public ActionResult Index()
        {
            return View(db.Adminfaqcategories.ToList());
        }

        // GET: Adminfaqcategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminfaqcategory adminfaqcategory = db.Adminfaqcategories.Find(id);
            if (adminfaqcategory == null)
            {
                return HttpNotFound();
            }
            return View(adminfaqcategory);
        }

        // GET: Adminfaqcategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adminfaqcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,categoryname")] Adminfaqcategory adminfaqcategory)
        {
            if (ModelState.IsValid)
            {
                db.Adminfaqcategories.Add(adminfaqcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminfaqcategory);
        }

        // GET: Adminfaqcategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminfaqcategory adminfaqcategory = db.Adminfaqcategories.Find(id);
            if (adminfaqcategory == null)
            {
                return HttpNotFound();
            }
            return View(adminfaqcategory);
        }

        // POST: Adminfaqcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,categoryname")] Adminfaqcategory adminfaqcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminfaqcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminfaqcategory);
        }

        // GET: Adminfaqcategories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminfaqcategory adminfaqcategory = db.Adminfaqcategories.Find(id);
            if (adminfaqcategory == null)
            {
                return HttpNotFound();
            }
            return View(adminfaqcategory);
        }

        // POST: Adminfaqcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Adminfaqcategory adminfaqcategory = db.Adminfaqcategories.Find(id);
            db.Adminfaqcategories.Remove(adminfaqcategory);
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
