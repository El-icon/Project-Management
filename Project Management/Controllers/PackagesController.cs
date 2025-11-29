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

    public class PackagesController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Packages
        public ActionResult Index()
        {
            return View(db.Packages.ToList());
        }

        // GET: Project_Management/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: Package/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Package/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Create(
    string id, string packagename, string monthlyprice, string annualprice,
    string currency, string filestoragemb, string maxemployees,
    string moduleinpackage1, string moduleinpackage2, string moduleinpackage3,
    string moduleinpackage4, string moduleinpackage5, string moduleinpackage6,
    string moduleinpackage7, string moduleinpackage8, string moduleinpackage9,
    string moduleinpackage10, string moduleinpackage11, string moduleinpackage12,
    string moduleinpackage13, string moduleinpackage14, string moduleinpackage15,
    string moduleinpackage16, string moduleinpackage17, string moduleinpackage18,
    string moduleinpackage19, string moduleinpackage20, string moduleinpackage21,
    string moduleinpackage22, string moduleinpackage23, string moduleinpackage24,
    string moduleinpackage25, string moduleinpackage26, string moduleinpackage27,
    string moduleinpackage28, string moduleinpackage29, string moduleinpackage30,
    string moduleinpackage31, string moduleinpackage32, string moduleinpackage33, 
    string moduleinpackage34, string status, string insertdate, string packagetype,
    string packagecategory, string description, string paymentdate, string nextpaymentdate,
    string licenceexpireson)
        {
            /* ----------  DIAGNOSTIC:  log what we received  ---------- */
            for (int i = 1; i <= 34; i++)
            {
                string key = "moduleinpackage" + i;
                string val = Request.Form[key];          // may be "true", "false", null, "on"
                System.Diagnostics.Debug.WriteLine($"{key} = {(val == null ? "NULL" : val)}");
            }
            /* --------------------------------------------------------- */

            try
            {
                /*  safe helper : only "true" (case-insensitive) → true  */
                bool ToBool(string v) => string.Equals(v, "true", StringComparison.OrdinalIgnoreCase);

                var package = new Package
                {
                    id = Guid.NewGuid().ToString(),
                    packagename = packagename,
                    monthlyprice = monthlyprice,
                    annualprice = annualprice,
                    currency = currency,
                    filestoragemb = filestoragemb,
                    maxemployees = maxemployees,
                    status = status,
                    packagetype = packagetype,
                    packagecategory = packagecategory,
                    description = description,
                    paymentdate = paymentdate,
                    nextpaymentdate = nextpaymentdate,
                    licenceexpireson = licenceexpireson,
                    insertdate = DateTime.Now.ToString(),

                    /*  modules – null / missing / "on" all become false  */
                    moduleinpackage1 = ToBool(moduleinpackage1),
                    moduleinpackage2 = ToBool(moduleinpackage2),
                    moduleinpackage3 = ToBool(moduleinpackage3),
                    moduleinpackage4 = ToBool(moduleinpackage4),
                    moduleinpackage5 = ToBool(moduleinpackage5),
                    moduleinpackage6 = ToBool(moduleinpackage6),
                    moduleinpackage7 = ToBool(moduleinpackage7),
                    moduleinpackage8 = ToBool(moduleinpackage8),
                    moduleinpackage9 = ToBool(moduleinpackage9),
                    moduleinpackage10 = ToBool(moduleinpackage10),
                    moduleinpackage11 = ToBool(moduleinpackage11),
                    moduleinpackage12 = ToBool(moduleinpackage12),
                    moduleinpackage13 = ToBool(moduleinpackage13),
                    moduleinpackage14 = ToBool(moduleinpackage14),
                    moduleinpackage15 = ToBool(moduleinpackage15),
                    moduleinpackage16 = ToBool(moduleinpackage16),
                    moduleinpackage17 = ToBool(moduleinpackage17),
                    moduleinpackage18 = ToBool(moduleinpackage18),
                    moduleinpackage19 = ToBool(moduleinpackage19),
                    moduleinpackage20 = ToBool(moduleinpackage20),
                    moduleinpackage21 = ToBool(moduleinpackage21),
                    moduleinpackage22 = ToBool(moduleinpackage22),
                    moduleinpackage23 = ToBool(moduleinpackage23),
                    moduleinpackage24 = ToBool(moduleinpackage24),
                    moduleinpackage25 = ToBool(moduleinpackage25),
                    moduleinpackage26 = ToBool(moduleinpackage26),
                    moduleinpackage27 = ToBool(moduleinpackage27),
                    moduleinpackage28 = ToBool(moduleinpackage28),
                    moduleinpackage29 = ToBool(moduleinpackage29),
                    moduleinpackage30 = ToBool(moduleinpackage30),
                    moduleinpackage31 = ToBool(moduleinpackage31),
                    moduleinpackage32 = ToBool(moduleinpackage32),
                    moduleinpackage33 = ToBool(moduleinpackage33),
                    moduleinpackage34 = ToBool(moduleinpackage34)
                };

                db.Packages.Add(package);
                db.SaveChanges();

                /*  activity log  */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created Package Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"]?.ToString(),
                    message = "PACKAGE NAME: " + packagename + " MONTHLYPRICE : " + monthlyprice + " ANNUALPRICE : " + annualprice + " CURRENCY : " + currency + " FILESTORAGEMB : " + filestoragemb 
                    + " MAXEMPLOYEES : " + maxemployees + " MODULEINPACKAGE1 : " + moduleinpackage1 + " MODULEINPACKAGE2 : " + moduleinpackage2 + " MODULEINPACKAGE3 : " + moduleinpackage3 
                    + " MODULEINPACKAGE4 : " + moduleinpackage4 + " MODULEINPACKAGE5 : " + moduleinpackage5 + " MODULEINPACKAGE6 : " + moduleinpackage6 + " MODULEINPACKAGE7 : " + moduleinpackage7 
                    + " MODULEINPACKAGE8 : " + moduleinpackage8 + " MODULEINPACKAGE9 : " + moduleinpackage9 + " MODULEINPACKAGE10 : " + moduleinpackage10 + " MODULEINPACKAGE11 : " + moduleinpackage11 
                    + " MODULEINPACKAGE12 : " + moduleinpackage12 + " MODULEINPACKAGE13 : " + moduleinpackage13 + " MODULEINPACKAGE14 : " + moduleinpackage14 + " MODULEINPACKAGE15 : " + moduleinpackage15 
                    + " MODULEINPACKAGE16 : " + moduleinpackage16 + " MODULEINPACKAGE17 : " + moduleinpackage17 + " MODULEINPACKAGE18 : " + moduleinpackage18 + " MODULEINPACKAGE19 : " + moduleinpackage19 
                    + " MODULEINPACKAGE20 : " + moduleinpackage20 + " MODULEINPACKAGE21 : " + moduleinpackage21 + " MODULEINPACKAGE22 : " + moduleinpackage22 
                    + " MODULEINPACKAGE23 : " + moduleinpackage23 + " MODULEINPACKAGE24 : " + moduleinpackage24 + " MODULEINPACKAGE25 : " + moduleinpackage25 + " MODULEINPACKAGE26 : " + moduleinpackage26 
                    + " MODULEINPACKAGE27 : " + moduleinpackage27 + " MODULEINPACKAGE28 : " + moduleinpackage28 + " MODULEINPACKAGE29 : " + moduleinpackage29 + " MODULEINPACKAGE30 : " + moduleinpackage30 
                    + " MODULEINPACKAGE31 : " + moduleinpackage31 + " MODULEINPACKAGE32 : " + moduleinpackage32 + " MODULEINPACKAGE33 : " + moduleinpackage33 + " MODULEINPACKAGE34 : " + moduleinpackage34 
                    + " STATUS : " + status + " PACKAGE TYPE : " + packagetype + " PACKAGE CATEGORY : " + packagecategory + " DESCRIPTION : " + description,
                    source = "Package/Create",
                    url = "Package/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Saved Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while creating Package",
                    status = "Unable to save",
                    notes = Session["userid"]?.ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Packages/Create",
                    logtype = "Package",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again. " + err.Message;
                return RedirectToAction("Index");
            }
        }





        // GET: Package/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package Package = db.Packages.Find(id);
            if (Package == null)
            {
                return HttpNotFound();
            }
            return View(Package);
        }

        // GET: Package/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    var pkg = db.Packages.Find(id);
        //    return pkg == null ? HttpNotFound() : View(pkg);   // or return PartialView for modal
        //}

        // POST: client_fuel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            string id, string packagename, string monthlyprice, string annualprice,
            string currency, string filestoragemb, string maxemployees,
            string moduleinpackage1, string moduleinpackage2, string moduleinpackage3,
            string moduleinpackage4, string moduleinpackage5, string moduleinpackage6,
            string moduleinpackage7, string moduleinpackage8, string moduleinpackage9,
            string moduleinpackage10, string moduleinpackage11, string moduleinpackage12,
            string moduleinpackage13, string moduleinpackage14, string moduleinpackage15,
            string moduleinpackage16, string moduleinpackage17, string moduleinpackage18,
            string moduleinpackage19, string moduleinpackage20, string moduleinpackage21,
            string moduleinpackage22, string moduleinpackage23, string moduleinpackage24,
            string moduleinpackage25, string moduleinpackage26, string moduleinpackage27,
            string moduleinpackage28, string moduleinpackage29, string moduleinpackage30,
            string moduleinpackage31, string moduleinpackage32, string moduleinpackage33,
            string moduleinpackage34, string status, string packagetype,
            string packagecategory, string description, string paymentdate,
            string nextpaymentdate, string licenceexpireson)
        {
            /*  same safe helper  */
            bool ToBool(string v) => string.Equals(v, "true", StringComparison.OrdinalIgnoreCase);

            try
            {
                var package = db.Packages.Find(id);
                if (package == null) return HttpNotFound();

                /*  update scalar fields  */
                package.packagename = packagename;
                package.monthlyprice = monthlyprice;
                package.annualprice = annualprice;
                package.currency = currency;
                package.filestoragemb = filestoragemb;
                package.maxemployees = maxemployees;
                package.status = status;
                package.packagetype = packagetype;
                package.packagecategory = packagecategory;
                package.description = description;
                package.paymentdate = paymentdate;
                package.nextpaymentdate = nextpaymentdate;
                package.licenceexpireson = licenceexpireson;

                /*  update 34 modules  */
                package.moduleinpackage1 = ToBool(moduleinpackage1);
                package.moduleinpackage2 = ToBool(moduleinpackage2);
                package.moduleinpackage3 = ToBool(moduleinpackage3);
                package.moduleinpackage4 = ToBool(moduleinpackage4);
                package.moduleinpackage5 = ToBool(moduleinpackage5);
                package.moduleinpackage6 = ToBool(moduleinpackage6);
                package.moduleinpackage7 = ToBool(moduleinpackage7);
                package.moduleinpackage8 = ToBool(moduleinpackage8);
                package.moduleinpackage9 = ToBool(moduleinpackage9);
                package.moduleinpackage10 = ToBool(moduleinpackage10);
                package.moduleinpackage11 = ToBool(moduleinpackage11);
                package.moduleinpackage12 = ToBool(moduleinpackage12);
                package.moduleinpackage13 = ToBool(moduleinpackage13);
                package.moduleinpackage14 = ToBool(moduleinpackage14);
                package.moduleinpackage15 = ToBool(moduleinpackage15);
                package.moduleinpackage16 = ToBool(moduleinpackage16);
                package.moduleinpackage17 = ToBool(moduleinpackage17);
                package.moduleinpackage18 = ToBool(moduleinpackage18);
                package.moduleinpackage19 = ToBool(moduleinpackage19);
                package.moduleinpackage20 = ToBool(moduleinpackage20);
                package.moduleinpackage21 = ToBool(moduleinpackage21);
                package.moduleinpackage22 = ToBool(moduleinpackage22);
                package.moduleinpackage23 = ToBool(moduleinpackage23);
                package.moduleinpackage24 = ToBool(moduleinpackage24);
                package.moduleinpackage25 = ToBool(moduleinpackage25);
                package.moduleinpackage26 = ToBool(moduleinpackage26);
                package.moduleinpackage27 = ToBool(moduleinpackage27);
                package.moduleinpackage28 = ToBool(moduleinpackage28);
                package.moduleinpackage29 = ToBool(moduleinpackage29);
                package.moduleinpackage30 = ToBool(moduleinpackage30);
                package.moduleinpackage31 = ToBool(moduleinpackage31);
                package.moduleinpackage32 = ToBool(moduleinpackage32);
                package.moduleinpackage33 = ToBool(moduleinpackage33);
                package.moduleinpackage34 = ToBool(moduleinpackage34);

                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();

                /*  activity log  */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Updated Package Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"]?.ToString(),
                    message = "PACKAGE NAME: " + packagename + " MONTHLYPRICE : " + monthlyprice + " ANNUALPRICE : " + annualprice + " CURRENCY : " + currency + " FILESTORAGEMB : " + filestoragemb
                    + " MAXEMPLOYEES : " + maxemployees + " MODULEINPACKAGE1 : " + moduleinpackage1 + " MODULEINPACKAGE2 : " + moduleinpackage2 + " MODULEINPACKAGE3 : " + moduleinpackage3
                    + " MODULEINPACKAGE4 : " + moduleinpackage4 + " MODULEINPACKAGE5 : " + moduleinpackage5 + " MODULEINPACKAGE6 : " + moduleinpackage6 + " MODULEINPACKAGE7 : " + moduleinpackage7
                    + " MODULEINPACKAGE8 : " + moduleinpackage8 + " MODULEINPACKAGE9 : " + moduleinpackage9 + " MODULEINPACKAGE10 : " + moduleinpackage10 + " MODULEINPACKAGE11 : " + moduleinpackage11
                    + " MODULEINPACKAGE12 : " + moduleinpackage12 + " MODULEINPACKAGE13 : " + moduleinpackage13 + " MODULEINPACKAGE14 : " + moduleinpackage14 + " MODULEINPACKAGE15 : " + moduleinpackage15
                    + " MODULEINPACKAGE16 : " + moduleinpackage16 + " MODULEINPACKAGE17 : " + moduleinpackage17 + " MODULEINPACKAGE18 : " + moduleinpackage18 + " MODULEINPACKAGE19 : " + moduleinpackage19
                    + " MODULEINPACKAGE20 : " + moduleinpackage20 + " MODULEINPACKAGE21 : " + moduleinpackage21 + " MODULEINPACKAGE22 : " + moduleinpackage22
                    + " MODULEINPACKAGE23 : " + moduleinpackage23 + " MODULEINPACKAGE24 : " + moduleinpackage24 + " MODULEINPACKAGE25 : " + moduleinpackage25 + " MODULEINPACKAGE26 : " + moduleinpackage26
                    + " MODULEINPACKAGE27 : " + moduleinpackage27 + " MODULEINPACKAGE28 : " + moduleinpackage28 + " MODULEINPACKAGE29 : " + moduleinpackage29 + " MODULEINPACKAGE30 : " + moduleinpackage30
                    + " MODULEINPACKAGE31 : " + moduleinpackage31 + " MODULEINPACKAGE32 : " + moduleinpackage32 + " MODULEINPACKAGE33 : " + moduleinpackage33 + " MODULEINPACKAGE34 : " + moduleinpackage34
                    + " STATUS : " + status + " PACKAGE TYPE : " + packagetype + " PACKAGE CATEGORY : " + packagecategory + " DESCRIPTION : " + description,
                    source = "Package/Edit",
                    url = "Package/Edit",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while updating Package",
                    status = "Unable to save",
                    notes = Session["userid"]?.ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Packages/Edit",
                    logtype = "Package",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again. " + err.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Package/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package Package = db.Packages.Find(id);
            if (Package == null)
            {
                return HttpNotFound();
            }
            return View(Package);
        }

        // POST: Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var pac = db.Packages.Find(id);
                db.Packages.Remove(db.Packages.Find(id));
                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Deleted Package Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = "Deleted a Package Successsfully: " + pac.packagename,
                    source = "Package/delete/" + id,
                    url = "Package/delete/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deleted Sucessfully.";
                return RedirectToAction("Index", "Packages");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting Package",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Packages/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "Packages");
            }
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
