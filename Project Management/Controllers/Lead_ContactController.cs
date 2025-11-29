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
    public class Lead_ContactController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Lead_Contact
        public ActionResult Index()
        {
            //var lead_Contact = db.Lead_Contact.Include(l => l.company001);
            //var lead_Contact = db.Lead_Contact.Include(l => l.employeeid);
            //return View(lead_Contact.ToList());
            var lead_Contact = db.Lead_Contact
                        .Include(l => l.company001)
                        .Include(l => l.Employee)
                        .Include(l => l.Deals)
                        .ToList();
            return View(lead_Contact);
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

        // GET: Lead_Contact/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead_Contact lead_Contact = db.Lead_Contact.Find(id);
            if (lead_Contact == null)
            {
                return HttpNotFound();
            }
            return View(lead_Contact);
        }

        // GET: Lead_Contact/Create
        public ActionResult Create()
        {
            ViewBag.companyid = new SelectList(db.company001, "id", "name");
            ViewBag.companyid = new SelectList(db.Employees, "id", "name");
            return View();
        }

        // POST: Lead_Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,name,email,leadsource,addedby,leadowner,insertdate,companyid")] Lead_Contact lead_Contact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Lead_Contact.Add(lead_Contact);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.companyid = new SelectList(db.company001, "id", "name", lead_Contact.companyid);
        //    return View(lead_Contact);
        //}
        [HttpPost]
        [ValidateInput(false)]          // allow HTML in address
        [ValidateAntiForgeryToken]
        public ActionResult Create(
         /* ----------  Lead_Contact scalars  ---------- */
         string name,
         string email,
         string leadsource,
         string addedby,
         string leadowner,

         /* ----------  FK columns that exist in Lead_Contact  ---------- */
         string companyid,        // not used when we create company on-the-fly
         string employeeid,       // not used here (we use addedby / leadowner)
         string companyid1,
         string leadid1,

         /* ----------  company001 fields  ---------- */
         string companyName,
         string website,
         string mobile,
         string officemobile,
         string country,
         string state,
         string city,
         string postalcode,
         string address,
         string note,
         string insertdate,
         string status,

         /* ----------  Deal fields  ---------- */
         string dealname,
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
                /* 1.  CREATE / RE-USE COMPANY */
                var company = db.company001
                                .FirstOrDefault(c => c.name == companyName);

                if (company == null)   // brand-new company
                {
                    company = new company001
                    {
                        id = Guid.NewGuid().ToString(),
                        name = companyName,
                        website = website,
                        mobile = mobile,
                        officemobile = officemobile,
                        country = country,
                        state = state,
                        city = city,
                        postalcode = postalcode,
                        address = setEditor.getJsonString(address),
                        note = "Company",
                        insertdate = DateTime.Now.ToString(),
                        status = "Active"
                    };
                    db.company001.Add(company);
                    db.SaveChanges();          // now we have company.id
                }

                /* 2.  CREATE LEAD */
                var lead = new Lead_Contact
                {
                    id = Guid.NewGuid().ToString(),
                    name = name,
                    email = email,
                    leadsource = leadsource,
                    addedby = addedby,
                    leadowner = leadowner,
                    insertdate = DateTime.Now.ToString(),
                    companyid = company.id   // FK to company
                };
                db.Lead_Contact.Add(lead);
                db.SaveChanges();

                /* 3.  CREATE DEAL */
                var deal = new Deal
                {
                    id = Guid.NewGuid().ToString(),
                    name = dealname,
                    pipeline = pipeline,
                    dealstages = dealstages,
                    dealvalue = dealvalue,
                    closedate = closedate,
                    dealcategory = dealcategory,
                    dealagent = dealagent,
                    product = product,
                    dealwatcher = dealwatcher,
                    insertdate = DateTime.Now.ToString(),
                    companyid1 = company.id,   // FK to company
                    leadid1 = lead.id       // FK to lead
                };
                db.Deals.Add(deal);
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Company, Lead and Deal saved successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["success"] = "false";
                TempData["message"] = $"Failed: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        // GET: Lead_Contact/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead_Contact lead_Contact = db.Lead_Contact.Find(id);
            if (lead_Contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyid = new SelectList(db.company001, "id", "name", lead_Contact.companyid);
            return View(lead_Contact);
        }

        // POST: Lead_Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,email,leadsource,addedby,leadowner,insertdate,companyid")] Lead_Contact lead_Contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lead_Contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyid = new SelectList(db.company001, "id", "name", lead_Contact.companyid);
            return View(lead_Contact);
        }

        // GET: Lead_Contact/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead_Contact lead_Contact = db.Lead_Contact.Find(id);
            if (lead_Contact == null)
            {
                return HttpNotFound();
            }
            return View(lead_Contact);
        }

        // POST: Lead_Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Lead_Contact lead_Contact = db.Lead_Contact.Find(id);
            db.Lead_Contact.Remove(lead_Contact);
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
