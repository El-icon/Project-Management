using Project_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    public class ActivityLogsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: ActivityLogs
        public async Task<ActionResult> Index()
        {
            return View(await db.ActivityLogs.ToListAsync());
        }

        // GET: ActivityLogs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityLog activityLog = db.ActivityLogs.Find(id);
            if (activityLog == null)
            {
                return HttpNotFound();
            }
            return View(activityLog);
        }

        // GET: ActivityLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, string title, string message, string code, string error, string status, string notes, string loglevel, string insertdate, string insertuser)
        {
            try
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    title = title,
                    message = message,
                    code = code,
                    error = error,
                    status = status,
                    notes = notes,
                    loglevel = loglevel,
                    insertdate = insertdate,
                    insertuser = insertuser
                });
                //create activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created ActivityLogs successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(),
                    message = title,
                    source = "ActivityLogs/Create",
                    url = "ActivityLogs/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Saved sucessfully.";
                return RedirectToAction("Index", "ActivityLogs");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating ActivityLogs",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "ActivityLogs/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "ActivityLogs");
            }
        }


        // GET: ActivityLogs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityLog activityLog = db.ActivityLogs.Find(id);
            if (activityLog == null)
            {
                return HttpNotFound();
            }
            return View(activityLog);
        }

        // POST: ActivityLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string title, string message, string code, string error, string status, string notes, string loglevel, string insertdate, string insertuser)
        {
            try
            {
                var ActivityLog = db.ActivityLogs.Find(id);
                ActivityLog.title = title;
                ActivityLog.message = message;
                ActivityLog.code = code;
                ActivityLog.error = error;
                ActivityLog.status = status;
                ActivityLog.notes = notes;
                ActivityLog.loglevel = loglevel;
                ActivityLog.insertdate = insertdate;
                ActivityLog.insertuser = insertuser;

                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Edited ActivityLog Successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(),
                    message = ActivityLog.title,
                    source = "ActivityLogs/Edit/" + id,
                    url = "ActivityLogs/Edit/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Edited sucessfully.";
                return RedirectToAction("Index", "ActivityLogs");

            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while editing ActivityLogs",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "ActivityLogs/edit/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return View(db.ActivityLogs.Find(id));
            }
        }

        // GET: ActivityLogs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityLog activityLog = db.ActivityLogs.Find(id);
            if (activityLog == null)
            {
                return HttpNotFound();
            }
            return View(activityLog);
        }

        // POST: ActivityLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var pub = db.ActivityLogs.Find(id);
                db.ActivityLogs.Remove(db.ActivityLogs.Find(id));
                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Deleted ActivityLogs successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(),
                    message = "Deleted ActivityLogs successsfully: " + pub.title,
                    source = "ActivityLogs/delete/" + id,
                    url = "ActivityLogs/delete/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deleted sucessfully.";
                return RedirectToAction("Index", "ActivityLogs");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting ActivityLogs",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "ActivityLogs/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "ActivityLogs");
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