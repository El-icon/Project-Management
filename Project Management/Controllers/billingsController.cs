using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Project_Management.Models;
using Project_Management.Setup;
using System.Data.Entity.Infrastructure;
using System.Web.Routing;
using System.Text;
using System.IO;
using System.Web.Mvc.Filters;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class billingsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: billings
        public ActionResult Index()
        {
            var billings = db.billings.Include(b => b.Company).Include(b => b.Package);
            return View(billings.ToList());
        }

        // GET: billings/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            billing billing = db.billings.Find(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // GET: billings/Create
        public ActionResult Create()
        {
            ViewBag.companyid = new SelectList(db.Companies, "id", "companyname");
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename");
            return View();
        }

        // POST: billings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,companyid,packageid,paymentdate,nextpaymentdate,transactionid,amount,paymentgateway")] billing billing)
        {
            if (ModelState.IsValid)
            {
                db.billings.Add(billing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.companyid = new SelectList(db.Companies, "id", "companyname", billing.companyid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", billing.packageid);
            return View(billing);
        }

        // GET: billings/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            billing billing = db.billings.Find(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyid = new SelectList(db.Companies, "id", "companyname", billing.companyid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", billing.packageid);
            return View(billing);
        }

        // POST: billings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,companyid,packageid,paymentdate,nextpaymentdate,transactionid,amount,paymentgateway")] billing billing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyid = new SelectList(db.Companies, "id", "companyname", billing.companyid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", billing.packageid);
            return View(billing);
        }

        // GET: billings/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            billing billing = db.billings.Find(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // POST: billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            billing billing = db.billings.Find(id);
            db.billings.Remove(billing);
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
