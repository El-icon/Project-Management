using Project_Management.Models;
using Project_Management.setup;
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
    public class DealsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Deals
        public ActionResult Index()
        {
            var deals = db.Deals.Include(d => d.Lead_Contact);
            var deals1 = db.Deals.Include(d => d.Employee);
            return View(deals.ToList());
        }

        // GET: Deals/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // GET: Deals/Create
        public ActionResult Create()
        {
            ViewBag.leadcontactid = new SelectList(db.Lead_Contact, "id", "name");
            ViewBag.employeeid = new SelectList(db.Employees, "id", "name");
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
           string leadcontactid,
           string name,
           string pipeline,
           string dealstages,
           string dealvalue,
           string closedate,
           string dealcategory,
           string dealagent,
           string product,
           string dealwatcher)
        {
            try
            {
                db.Deals.Add(new Deal
                {
                    id = Guid.NewGuid().ToString(),
                    leadcontactid = leadcontactid,
                    name = name,
                    pipeline = pipeline,
                    dealstages = dealstages,
                    dealvalue = dealvalue,
                    closedate = closedate,
                    dealcategory = dealcategory,
                    dealagent = dealagent,
                    product = product,
                    dealwatcher = dealwatcher,
                    insertdate = DateTime.Now.ToString()
                });

                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deal created.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["success"] = "false";
                TempData["message"] = "Save failed: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult UpdateStage(string id, string stage)
        {
            var deal = db.Deals.Find(id);
            if (deal == null) return Json(new { success = false });

            deal.dealstages = stage;
            db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Deals/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.leadcontactid = new SelectList(db.Lead_Contact, "id", "name", deal.leadcontactid);
            return View(deal);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,leadcontactid,name,pipeline,dealstages,dealvalue,closedate,dealcategory,dealagent,product,dealwatcher,insertdate")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.leadcontactid = new SelectList(db.Lead_Contact, "id", "name", deal.leadcontactid);
            return View(deal);
        }

        // GET: Deals/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
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
