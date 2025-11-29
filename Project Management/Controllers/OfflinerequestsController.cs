using Project_Management.Models;
using Project_Management.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class OfflinerequestsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Offlinerequests
        public ActionResult Index()
        {
            return View(db.Offlinerequests.ToList());
        }

        // GET: Offlinerequests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offlinerequest offlinerequest = db.Offlinerequests.Find(id);
            if (offlinerequest == null)
            {
                return HttpNotFound();
            }
            return View(offlinerequest);
        }

        // GET: Offlinerequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offlinerequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,company,package,paymentby,created,status,insertdate")] Offlinerequest offlinerequest)
        {
            if (ModelState.IsValid)
            {
                db.Offlinerequests.Add(offlinerequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offlinerequest);
        }

        // GET: Offlinerequests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offlinerequest offlinerequest = db.Offlinerequests.Find(id);
            if (offlinerequest == null)
            {
                return HttpNotFound();
            }
            return View(offlinerequest);
        }

        // POST: Offlinerequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,company,package,paymentby,created,status,insertdate")] Offlinerequest offlinerequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offlinerequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offlinerequest);
        }

        // GET: Offlinerequests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offlinerequest offlinerequest = db.Offlinerequests.Find(id);
            if (offlinerequest == null)
            {
                return HttpNotFound();
            }
            return View(offlinerequest);
        }

        // POST: Offlinerequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Offlinerequest offlinerequest = db.Offlinerequests.Find(id);
            db.Offlinerequests.Remove(offlinerequest);
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
