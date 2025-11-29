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
    public class StickyNotesController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: StickyNotes
        //public ActionResult Index()
        //{
        //    return View(db.StickyNotes.ToList());
        //}
        // GET: StickyNotes
        public ActionResult Index(string searchTerm)
        {
            var notes = db.StickyNotes.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // case-insensitive search
                notes = notes.Where(n => n.title.ToLower().Contains(searchTerm));
            }

            return View(notes.OrderByDescending(n => n.insertdate).ToList());
        }


        // GET: StickyNotes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StickyNote stickyNote = db.StickyNotes.Find(id);
            if (stickyNote == null)
            {
                return HttpNotFound();
            }
            return View(stickyNote);
        }

        // GET: StickyNotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StickyNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // Disable request validation for this action
        public ActionResult Create(string id, string title, string status, string insertdate,
            string updatedate, string createdby, string updateby,
            string description, string color)
        {
            try
            {
                db.StickyNotes.Add(new StickyNote
                {
                    id = Guid.NewGuid().ToString(),
                    title = title,
                    status = status,
                    color = color,
                    description = setEditor.getJsonString(description),
                    createdby = Session["email"].ToString(),
                    updateby = Session["email"].ToString(),
                    updatedate = DateTime.Now,
                    insertdate = DateTime.Now
                });

                // Create activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Sticky Note Created Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"].ToString(),
                    message = $"TITLE: {title} STATUS: {status} COLOR: {color} DESCRIPTION: {description}",
                    source = "StickyNotes/Create",
                    url = "StickyNotes/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString() // Assuming you have the user's email stored in the session
                });
                db.SaveChanges();

                // Redirect to the index page with a success message
                TempData["success"] = "true";
                TempData["message"] = "Sticky Note Saved Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                // Log the error
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while creating Sticky Note",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "StickyNotes/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                // Return to the view with an error message
                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again." + err.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: StickyNotes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StickyNote stickyNote = db.StickyNotes.Find(id);
            if (stickyNote == null)
            {
                return HttpNotFound();
            }
            return View(stickyNote);
        }

        // POST: StickyNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: StickyNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // Allow HTML in description
        public ActionResult Edit(string id, string title, 
            string status, string description, string color)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Invalid Sticky Note ID.";
                    return RedirectToAction("Index");
                }

                StickyNote stickyNote = db.StickyNotes.Find(id);
                if (stickyNote == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Sticky Note not found.";
                    return RedirectToAction("Index");
                }

                // Update fields
                stickyNote.title = title;
                stickyNote.status = status;
                stickyNote.color = color;
                stickyNote.description = setEditor.getJsonString(description); // same as Create
                stickyNote.updateby = Session["email"].ToString();
                stickyNote.updatedate = DateTime.Now;

                db.Entry(stickyNote).State = EntityState.Modified;

                // Create activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Sticky Note Updated Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"].ToString(),
                    message = $"TITLE: {title} STATUS: {status} COLOR: {color} DESCRIPTION: {description}",
                    source = "StickyNotes/Edit",
                    url = "StickyNotes/Edit",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });

                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Sticky Note Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while updating Sticky Note",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "StickyNotes/Edit",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again." + err.Message;
                return RedirectToAction("Index");
            }
        }


        // GET: StickyNotes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StickyNote stickyNote = db.StickyNotes.Find(id);
            if (stickyNote == null)
            {
                return HttpNotFound();
            }
            return View(stickyNote);
        }

        // POST: StickyNotes/Delete/5
        // POST: StickyNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Invalid Sticky Note ID.";
                    return RedirectToAction("Index");
                }

                StickyNote stickyNote = db.StickyNotes.Find(id);

                if (stickyNote == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Sticky Note not found.";
                    return RedirectToAction("Index");
                }

                db.StickyNotes.Remove(stickyNote);

                // Create activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Sticky Note Deleted Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"].ToString(),
                    message = $"Deleted Sticky Note TITLE: {stickyNote.title}, STATUS: {stickyNote.status}, COLOR: {stickyNote.color}",
                    source = "StickyNotes/Delete",
                    url = "StickyNotes/Delete",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });

                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Sticky Note Deleted Successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while deleting Sticky Note",
                    status = "Unable to delete",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "StickyNotes/Delete",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed to delete Sticky Note. " + err.Message;
                return RedirectToAction("Index");
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
