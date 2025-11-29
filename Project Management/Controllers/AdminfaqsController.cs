using Antlr.Runtime;
using Project_Management.Models;
using Project_Management.setup;
using Project_Management.Setup;
using System;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class AdminfaqsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Adminfaqs
        public ActionResult Index()
        {
            var adminfaqs = db.Adminfaqs.Include(a => a.Adminfaqcategory);
            return View(adminfaqs.ToList());
        }
        /*****************************************************************/
        /*****************************************************************/

         
        // GET: Adminfaqs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var adminfaq = db.Adminfaqs.Find(id);
            if (adminfaq == null)
                return HttpNotFound();

            // dropdown list for inline category editor
            ViewBag.Cats = db.Adminfaqcategories
                             .OrderBy(c => c.categoryname)
                             .ToList();

            return View(adminfaq);
        }
        /*****************************************************************/
        /*****************************************************************/



        // GET: Adminfaqs/Create
        public ActionResult Create()
        {
            ViewBag.categoryid = new SelectList(db.Adminfaqcategories, "id", "categoryname");
            return View();
        } 

        [HttpPost]
        [ValidateInput(false)]   // allows HTML only in this action
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,
                       string articletitle,
                       string categoryid,
                       string description,
                       string doc_order,
                       string expdate,
                       string doctype,
                       string notes)
        {
            try
            {
                /* 1.  validate file */
                if (file == null || file.ContentLength == 0)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.";
                    return RedirectToAction("Index");
                }

                /* 2. validate extension */
                string ext = Path.GetExtension(file.FileName);
                string[] allowedExt = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (!allowedExt.Contains(ext.ToLower()))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Only PDF or image files are allowed.";
                    return RedirectToAction("Index");
                }

                /* 3. build safe file name */
                string fileName = $"{categoryid}_{Guid.NewGuid()}{ext}";

                /* 4. create folder */
                string folder = Server.MapPath("~/UploadedFiles/Files/" + categoryid);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                /* 5. save file */
                string fullPath = Path.Combine(folder, fileName);
                file.SaveAs(fullPath);

                /* 6. persist record */
                var faq = new Adminfaq
                {
                    id = Guid.NewGuid().ToString(),
                    articletitle = articletitle,
                    categoryid = categoryid,
                    description = setEditor.getJsonString(description),
                    files = fileName,
                    url = "/UploadedFiles/Files/" + categoryid + "/" + fileName,
                    status = "True",
                    insertdate = DateTime.Now.ToString(),
                    expdate = string.IsNullOrEmpty(expdate) ? (DateTime?)null : Convert.ToDateTime(expdate),
                    doc_order = doc_order,
                    doctype = doctype,
                    notes = notes
                };
                db.Adminfaqs.Add(faq);
                db.SaveChanges();

                /* 7. build exact message string */
                var cat = db.Adminfaqcategories.Find(categoryid);
                string message = "DOC NAME: " + articletitle +
                                 " ADMINFAQCAT: " + (cat?.categoryname ?? "N/A") +
                                 " DOC ORDER: " + doc_order +
                                 " TITLE: " + articletitle +
                                 " DESCRIPTION: " + description +
                                 " FILE NAME: " + file.FileName;

                /* 8. activity log */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Uploaded",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = message,
                    source = "Adminfaq/Create",
                    url = "Adminfaq/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Uploaded successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Upload Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaq/Create",
                    url = "Adminfaq/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Upload failed: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /*****************************************************************/
        /*****************************************************************/


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]   // allows HTML only in this action
        public ActionResult Uploaddocument(HttpPostedFileBase file,
                                   string id,          // <-- FAQ id (kept for consistency)
                                   string doc_order,
                                   string expdate)
        {
            try
            {
                /* 1. basic file check */
                if (file == null || file.ContentLength == 0)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.";
                    return RedirectToAction("Details", new { id = id });
                }

                /* 2. extension check */
                string ext = Path.GetExtension(file.FileName).ToLower();
                string[] allowed = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Only PDF or image files are allowed.";
                    return RedirectToAction("Details", new { id = id });
                }

                /* 3. resolve the FAQ row so we have the real category id */
                var faqRow = db.Adminfaqs.Find(id);
                if (faqRow == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "FAQ record not found.";
                    return RedirectToAction("Index");
                }
                string categoryid = faqRow.categoryid;

                /* 4. build safe file name */
                string fileName = $"{categoryid}_{Guid.NewGuid()}{ext}";

                /* 5. create folder */
                string folder = Server.MapPath("~/UploadedFiles/Files/" + categoryid);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                /* 6. save file */
                string fullPath = Path.Combine(folder, fileName);
                file.SaveAs(fullPath);

                /* 7. create new FAQ record (same table, new row) */
                var newFaq = new Adminfaq
                {
                    id = Guid.NewGuid().ToString(),
                    articletitle = faqRow.articletitle,          // inherit from parent
                    categoryid = categoryid,
                    description = faqRow.description,    // inherit from parent
                    files = fileName,
                    url = "/UploadedFiles/Files/" + categoryid + "/" + fileName,
                    status = "True",
                    insertdate = DateTime.Now.ToString(),
                    expdate = string.IsNullOrEmpty(expdate) ? (DateTime?)null : Convert.ToDateTime(expdate),
                    doc_order = doc_order,
                    doctype = ext == ".pdf" ? "PDF" : "IMAGE",
                    notes = null
                };
                db.Adminfaqs.Add(newFaq);
                db.SaveChanges();

                /* 8. build identical message string */
                var cat = db.Adminfaqcategories.Find(categoryid);
                string message = "DOC NAME: " + newFaq.articletitle +
                                 " ADMINFAQCAT: " + (cat?.categoryname ?? "N/A") +
                                 " DOC ORDER: " + doc_order +
                                 " TITLE: " + newFaq.articletitle +
                                 " DESCRIPTION: " + newFaq.description +
                                 " FILE NAME: " + file.FileName;

                /* 9. activity log */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Uploaded",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = message,
                    source = "Adminfaq/Uploaddocument",
                    url = "Adminfaq/Uploaddocument",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Uploaded successfully!";
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Upload Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaq/Uploaddocument",
                    url = "Adminfaq/Uploaddocument",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Upload failed: " + ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }


        /*****************************************************************/
        /*****************************************************************/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult updateDocument(HttpPostedFileBase file, string id)
        {
            try
            {
                /* 1. find the row */
                var faq = db.Adminfaqs.Find(id);
                if (faq == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Record not found.";
                    return RedirectToAction("Index");
                }

                /* 2. basic file check */
                if (file == null || file.ContentLength == 0)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.";
                    return RedirectToAction("Details", new { id = faq.id });
                }

                /* 3. extension check */
                string ext = Path.GetExtension(file.FileName).ToLower();
                string[] allowed = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Only PDF or image files are allowed.";
                    return RedirectToAction("Details", new { id = faq.id });
                }

                /* 4. delete old file (optional) */
                string oldFullPath = Server.MapPath("~" + faq.url);
                if (System.IO.File.Exists(oldFullPath))
                    System.IO.File.Delete(oldFullPath);

                /* 5. build new file name & path (same folder) */
                string fileName = $"{faq.categoryid}_{Guid.NewGuid()}{ext}";
                string folder = Server.MapPath("~/UploadedFiles/Files/" + faq.categoryid);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string fullPath = Path.Combine(folder, fileName);
                file.SaveAs(fullPath);

                /* 6. update record */
                faq.files = fileName;
                faq.url = "/UploadedFiles/Files/" + faq.categoryid + "/" + fileName;
                faq.doctype = ext == ".pdf" ? "PDF" : "IMAGE";
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();

                /* 7. activity log */
                var cat = db.Adminfaqcategories.Find(faq.categoryid);
                string message = "DOC REPLACED: " + faq.articletitle +
                                 " ADMINFAQCAT: " + (cat?.categoryname ?? "N/A") +
                                 " NEW FILE: " + file.FileName;
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Document Replaced",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = message,
                    source = "Adminfaq/ChangeDocument",
                    url = "Adminfaq/ChangeDocument",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Document replaced successfully!";
                return RedirectToAction("Details", new { id = faq.id });
            }
            catch (Exception ex)
            {
                /* 8. error log */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Document Replace Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaq/ChangeDocument",
                    url = "Adminfaq/ChangeDocument",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Replace failed: " + ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

        /*****************************************************************/
        /*****************************************************************/
        /*****************************************************************/
        /*****************************************************************/

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]   // allows HTML only in this action
        public ActionResult EditDocument(
            string id, string articletitle,
            string categoryid, string doc_order, 
            string description, string url, 
            string status, 
            string insertdate, string updatedate, 
            string createdby, string updateby) 
        {            
                /* 1. locate row ------------------------------------------------*/
                var faq = db.Adminfaqs.Find(id);
                var admincat = db.Adminfaqcategories.FirstOrDefault(p => p.id == categoryid);

                try
                {
                faq.articletitle = articletitle;
                faq.doc_order = doc_order;
                faq.categoryid = categoryid;
                faq.description = setEditor.getJsonString(description);
                faq.status = status;
                faq.insertdate = DateTime.Now.ToString();
                faq.updatedate = DateTime.Now;
                faq.createdby = createdby;
                faq.updateby = updateby;

                    //Create Activity log
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Edited Adminfaqs Title Successfully",
                        status = "Access Granted Successsfully",
                        notes = Session["userid"].ToString(),
                        message = "ADMINFAQS TITLE: " + faq.articletitle + " ADMINFAQS CAT: " + admincat.categoryname + " ADMINFAQS ORDER: " + faq.doc_order,
                        source = "Adminfaqs/Edit/" + id,
                        url = "Adminfaqs/Edit/" + id,
                        logtype = "success",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"].ToString()
                    });
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                    TempData["success"] = "true";
                    TempData["message"] = "Edited successfully.";
                    return RedirectToAction("Details", new { id = id });

            }
            catch (Exception err)
                {
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "500",
                        title = "An error occured while editing Adminfaqs",
                        notes = Session["userid"].ToString(),
                        message = err.ToString(),
                        source = err.Source,
                        url = "Adminfaqs/edit/" + id,
                        logtype = "error",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"].ToString()
                    });
                    db.SaveChanges();

                    TempData["success"] = "false";
                    TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Details", new { id = id });
            }
        }



        /*****************************************************************/
        /*****************************************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAdminfaqs(string id)
        {
            try
            {
                /* 1. Find the row -----------------------------------------*/
                var faq = db.Adminfaqs.Find(id);
                if (faq == null) return HttpNotFound();

                /* 2. Remember values for the log --------------------------*/
                string title = faq.articletitle;
                string oldFile = faq.files;
                string oldUrl = faq.url;

                /* 3. Delete physical file ---------------------------------*/
                if (!string.IsNullOrEmpty(oldUrl))
                {
                    string fullPath = Server.MapPath("~" + oldUrl);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                /* 4. Remove DB record -------------------------------------*/
                db.Adminfaqs.Remove(faq);
                db.SaveChanges();

                /* 5. Activity log -----------------------------------------*/
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Deleted",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = $"FAQ '{title}' (file: {oldFile ?? "none"}) permanently deleted",
                    source = "Adminfaqs/Delete",
                    url = "Adminfaqs/Delete",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                /* 6. Feedback ---------------------------------------------*/
                TempData["success"] = "true";
                TempData["message"] = "Document deleted successfully!";
            }
            catch (Exception ex)
            {
                /* 7. Error logging ----------------------------------------*/
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Delete Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaqs/Delete",
                    url = "Adminfaqs/Delete",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Delete failed: " + ex.Message;
            }

            /* 8. PRG redirect ------------------------------------------*/
            return RedirectToAction("Index");
        }

        /******************** JQUERY EDIT ****************/
        /*************************************************/
        [HttpPost]
        public JsonResult UpdateTitle(string id, string newTitle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newTitle))
                    return Json("Title cannot be empty.", JsonRequestBehavior.AllowGet);

                var faq = db.Adminfaqs.Find(id);
                if (faq == null) return Json("Record not found.", JsonRequestBehavior.AllowGet);

                faq.articletitle = newTitle.Trim();
                faq.updatedate = DateTime.Now;
                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Save failed: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        /*****************************************************************/
        [HttpPost]
        public JsonResult UpdateCategoryname(string id, string newCatId)
        {
            try
            {
                var faq = db.Adminfaqs.Find(id);
                if (faq == null) return Json("Record not found.", JsonRequestBehavior.AllowGet);

                faq.categoryid = newCatId;
                faq.updatedate = DateTime.Now;
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Save failed: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        /*****************************************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocument(string id)
        {
            try
            {
                /* 1. locate the FAQ --------------------------------------------------*/
                var faq = db.Adminfaqs.Find(id);
                if (faq == null) return HttpNotFound();

                /* 2. delete the physical file ---------------------------------------*/
                if (!string.IsNullOrEmpty(faq.url))
                {
                    string fullPath = Server.MapPath("~" + faq.url);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                /* 3. clear the DB columns -------------------------------------------*/
                faq.files = null;
                faq.url = null;
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();

                /* 4. activity log ----------------------------------------------------*/
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ File Deleted",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = $"File for FAQ '{faq.articletitle}' removed",
                    source = "Adminfaqs/DeleteDocument",
                    url = "Adminfaqs/DeleteDocument",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "File removed successfully!";
            }
            catch (Exception ex)
            {
                /* 5. error log -------------------------------------------------------*/
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "File Delete Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaqs/DeleteDocument",
                    url = "Adminfaqs/DeleteDocument",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "File delete failed: " + ex.Message;
            }

            /* 6. PRG redirect ------------------------------------------------------*/
            return RedirectToAction("Details", new { id });
        }
        /*****************************************************************/
        /*****************************************************************/
                 
        // GET: Adminfaqs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminfaq adminfaq = db.Adminfaqs.Find(id);
            if (adminfaq == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryid = new SelectList(db.Adminfaqcategories, "id", "categoryname", adminfaq.categoryid);
            return View(adminfaq);
        }


        // POST: Adminfaqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,articletitle,category,description,files,categoryid")] Adminfaq adminfaq)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminfaq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryid = new SelectList(db.Adminfaqcategories, "id", "categoryname", adminfaq.categoryid);
            return View(adminfaq);
        }

        /*****************************************************************/
        /*****************************************************************/
        
        // GET: Adminfaqs/Delete/5
        // POST: Adminfaqcategories/Delete/5
        [HttpPost, ActionName("deleteAdminfaqscat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var category = db.Adminfaqcategories.Find(id);
                if (category != null)
                {
                    // delete related FAQs
                    db.Adminfaqs.RemoveRange(db.Adminfaqs.Where(f => f.categoryid == id));

                    // delete category
                    db.Adminfaqcategories.Remove(category);
                    db.SaveChanges();

                    // log success
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Deleted FAQ Category Successfully",
                        status = "Access Granted Successfully",
                        notes = Session["userid"]?.ToString(),
                        message = "CATEGORY NAME: " + category.categoryname,
                        source = "Adminfaqscategory/Delete",
                        url = "Adminfaqscategory/Delete",
                        logtype = "success",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"]?.ToString()
                    });
                    db.SaveChanges();
                }

                TempData["success"] = "true";
                TempData["message"] = "Category deleted successfully.";
            }
            catch (Exception ex)
            {
                // log error
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Error while deleting FAQ Category",
                    status = "Delete Failed",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = ex.Source,
                    url = "Adminfaqscategory/Delete",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed to delete category. " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        /*****************************************************************/
        /*****************************************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adminfaqscategory([Bind(Include = "id,categoryname")] Adminfaqcategory adminfaqcategory)
        {
            try
            {
                if (!ModelState.IsValid) return View(adminfaqcategory);

                adminfaqcategory.id = Guid.NewGuid().ToString();
                db.Adminfaqcategories.Add(adminfaqcategory);
                db.SaveChanges();

                /*  activity log  */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created FAQ Category Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"]?.ToString(),
                    message = "CATEGORY NAME: " + adminfaqcategory.categoryname,
                    source = "Adminfaqscategory/Create",
                    url = "Adminfaqscategory/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occurred while creating FAQ Category",
                    status = "Unable to save",
                    notes = Session["userid"]?.ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Adminfaqscategory/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again. " + err.Message;
                return RedirectToAction("Index");
            }
        }

        /*****************************************************************/
        /*****************************************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategory(string id, string categoryname)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(categoryname))
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            var category = db.Adminfaqcategories.Find(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }

            try
            {
                // Update category
                category.categoryname = categoryname;
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;

                // Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "FAQ Category Updated Successfully",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = "CATEGORY UPDATED TO: " + categoryname,
                    source = "Adminfaqscategory/UpdateCategory",
                    url = "Adminfaqscategory/UpdateCategory",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });

                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "FAQ Category Update Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "Adminfaqscategory/UpdateCategory",
                    url = "Adminfaqscategory/UpdateCategory",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        /*****************************************************************/
        /*****************************************************************/
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
