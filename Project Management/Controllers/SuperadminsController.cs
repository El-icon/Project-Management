using Project_Management.Models;
using Project_Management.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class SuperadminsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Superadmins
        public ActionResult Index()
        {
            return View(db.Superadmins.ToList());
        }

        // GET: Superadmins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Superadmin superadmin = db.Superadmins.Find(id);
            if (superadmin == null)
            {
                return HttpNotFound();
            }
            return View(superadmin);
        }

        // GET: Superadmins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Superadmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, string name, string email, string userrole, string status)
        {  //create
            try
            { 
                db.Superadmins.Add(new Superadmin
                {
                    id = Guid.NewGuid().ToString(),
                    name = name,
                    email = email,
                    userrole = userrole,
                    status = status, 
                    insertdate = DateTime.Now.ToString(),
                });

                //create activity log
                //
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created Superadmins Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"].ToString(),
                    message = "FULL NAME: " + name + " EMAIL : " + email + " USERROLE : " + userrole + " STATUS : " + status,
                    source = "Superadmins/Create",
                    url = "Superadmins/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Saved Successfully.";
                return RedirectToAction("Index", "Superadmins");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating Superadmins",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Superadmins/Create",
                    logtype = "Package",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "Superadmins");
            }
        }

        // GET: Superadmins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Superadmin superadmin = db.Superadmins.Find(id);
            if (superadmin == null)
            {
                return HttpNotFound();
            }
            return View(superadmin);
        }

        // POST: Superadmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string name, string email, string userrole, string status)
        {
            try
            {
                //edit
                var superadmins = db.Superadmins.Find(id);
                superadmins.name = name;
                superadmins.email = email;
                superadmins.userrole = userrole;
                superadmins.status = status; 
                superadmins.insertdate = DateTime.Now.ToString();

                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Edited Superadmins Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = "FULL NAME: " + name + " EMAIL : " + email + " USERROLE : " + userrole + " STATUS : " + status,
                    source = "Superadmins/Edit/" + id,
                    url = "Superadmins/Edit/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Edited Sucessfully.";
                return RedirectToAction("Index", "Superadmins");

            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while editing Superadmins",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Superadmins/edit/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return View(db.Superadmins.Find(id));
            }
        }

        // GET: Superadmins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Superadmin superadmin = db.Superadmins.Find(id);
            if (superadmin == null)
            {
                return HttpNotFound();
            }
            return View(superadmin);
        }

        // POST: Superadmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var superadmins = db.Superadmins.Find(id);
                db.Superadmins.Remove(db.Superadmins.Find(id));
                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Deleted Superadmins Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = "Deleted a Superadmins Successsfully: " + superadmins.name,
                    source = "Superadmins/delete/" + id,
                    url = "Superadmins/delete/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deleted Sucessfully.";
                return RedirectToAction("Index", "Superadmins");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting Superadmins",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Superadmins/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "Superadmins");
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
