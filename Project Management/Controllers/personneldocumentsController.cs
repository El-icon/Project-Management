using Project_Management.Models;
using Project_Management.setup;
using Project_Management.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class personneldocumentsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: personneldocuments
         public ActionResult Index()
        {
            var personneldocuments = db.personneldocuments
                .Include(p => p.document)
                .Include(p => p.personel_files_cat)
                .ToList();

            // group categories by document id (document.id is string in your DB)
            var categoriesByDoc = personneldocuments
                .GroupBy(d => d.document.id)
                .ToDictionary(
                    g => g.Key, // string key (document id)
                    g => string.Join(", ",
                        g.Select(x => x.personel_files_cat != null
                            ? x.personel_files_cat.cat_name
                            : "(no category)").Distinct())
                );

            // pass categories dictionary to the view
            ViewBag.CategoriesByDoc = categoriesByDoc;

            // still return the original model so you don’t break @model
            return View(personneldocuments);
        }



        /******************** JQUERY EDIT ****************/
        /*************************************************/
        [HttpPost]
        public JsonResult Updatecat_name(string id, string newTitle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newTitle))
                    return Json("Title cannot be empty.", JsonRequestBehavior.AllowGet);

                var faq = db.personel_files_cat.Find(id);
                if (faq == null) return Json("Record not found.", JsonRequestBehavior.AllowGet);

                faq.cat_name = newTitle.Trim();
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
        /*****************************************************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult personel_files_cat([Bind(Include = "id,cat_name")] personel_files_cat personel_files_cat)
        {
            try
            {
                if (!ModelState.IsValid) return View(personel_files_cat);

                personel_files_cat.id = Guid.NewGuid().ToString();
                db.personel_files_cat.Add(personel_files_cat);
                db.SaveChanges();

                /*  activity log  */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created FAQ Category Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"]?.ToString(),
                    message = "CATEGORY NAME: " + personel_files_cat.cat_name,
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

        // GET: Adminfaqs/Delete/5
        // POST: Adminfaqcategories/Delete/5
        [HttpPost, ActionName("deletepersonel_files_cat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var category = db.personel_files_cat.Find(id);
                if (category != null)
                {
                    // delete related FAQs
                   // db.documents.RemoveRange(db.documents.Where(f => f.cat_id == id));

                    // delete category
                    db.personel_files_cat.Remove(category);
                    db.SaveChanges();

                    // log success
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Deleted FAQ Category Successfully",
                        status = "Access Granted Successfully",
                        notes = Session["userid"]?.ToString(),
                        message = "CATEGORY NAME: " + category.cat_name,
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

        public ActionResult CreateAdminfaq(string id)
        {
            var personneldocuments = db.personneldocuments.Include(p => p.document).Include(p => p.personel_files_cat);
            return View(personneldocuments.ToList());
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdminfaq(HttpPostedFileBase file,
                                   string name,
                                   string cat,
                                   string doc_order,
                                   string expdate,
                                   string doctype,
                                   string description,
                                   string notes)
        {
            try
            {
                /* 1. basic file check ------------------------------------*/
                if (file == null || file.ContentLength == 0)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.";
                    return RedirectToAction("Index");
                }

                string ext = Path.GetExtension(file.FileName).ToLower();
                string[] allowed = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Only PDF or image files are allowed.";
                    return RedirectToAction("Index");
                }

                /* 2. create DOCUMENT row first ---------------------------*/
                var docRow = new document
                {
                    id = Guid.NewGuid().ToString(),
                    name = name,
                    description = setEditor.getJsonString(description),
                    status = "True",
                    notes = "True",
                    insertdate = DateTime.Now,
                    updatedate = DateTime.Now,
                    createdby = Session["email"]?.ToString(),
                    updateby = Session["email"]?.ToString()
                };
                db.documents.Add(docRow);
                db.SaveChanges();          // now we have a valid id

                /* 3. file handling ---------------------------------------*/
                string fileName = $"{cat}_{Guid.NewGuid()}{ext}";
                string folder = Server.MapPath("~/UploadedFiles/Files/" + cat);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string fullPath = Path.Combine(folder, fileName);
                file.SaveAs(fullPath);

                /* 3b. auto-detect document type (C# 7.3) -----------------*/
                string detectedType;
                switch (ext)
                {
                    case ".pdf": detectedType = "PDF"; break;
                    case ".jpg":
                    case ".jpeg": detectedType = "JPEG"; break;
                    case ".png": detectedType = "PNG"; break;
                    case ".gif": detectedType = "GIF"; break;
                    case ".bmp": detectedType = "BMP"; break;
                    case ".webp": detectedType = "WebP"; break;
                    default: detectedType = "Unknown"; break;
                }
                 

                /* 4. create PERSONNELDOCUMENT row (uses real document id) */
                var perDoc = new personneldocument
                {
                    id = Guid.NewGuid().ToString(),
                    documentid = docRow.id,     // FK to documents table
                    cat_id = cat,
                    doc_order = doc_order,
                    doctype = string.IsNullOrWhiteSpace(doctype) ? detectedType : doctype,
                    notes = "True",
                    description = setEditor.getJsonString(description),
                    expdate = DateTime.Now,
                    status = "True",
                    insertdate = DateTime.Now,
                    //updatedate = DateTime.Now,
                    //createdby = Session["email"]?.ToString(),
                    //updateby = Session["email"]?.ToString(),
                    url = "/UploadedFiles/Files/" + cat + "/" + fileName
                };
                db.personneldocuments.Add(perDoc);
                db.SaveChanges();

                /* 5. activity log ----------------------------------------*/
                string catName = db.personel_files_cat.Find(cat)?.cat_name ?? "(none)";
                string message = $"DOC: {name} | CATEGORY: {catName} | FILE: {fileName}";
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Created",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = message,
                    source = "personneldocuments/CreateAdminfaq",
                    url = "personneldocuments/CreateAdminfaq",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Document created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Create Admin FAQ Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "personneldocuments/CreateAdminfaq",
                    url = "personneldocuments/CreateAdminfaq",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Create failed: " + ex.Message;
                return RedirectToAction("Index");
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
                /* 1. load child + parent ------------------------------------------*/
                var perDoc = db.personneldocuments
                               .Include(p => p.document)
                               .FirstOrDefault(p => p.id == id);

                if (perDoc == null || perDoc.document == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Record not found.";
                    return RedirectToAction("Index");
                }

                /* 2. save values for log **before** we delete anything -------------*/
                string docName = perDoc.document.name;
                string relativePath = perDoc.url;
                string catName = db.personel_files_cat
                                        .Find(perDoc.cat_id)?.cat_name ?? "(none)";
                string fileName = Path.GetFileName(relativePath);

                /* 3. delete physical file -----------------------------------------*/
                string physicalPath = Server.MapPath("~" + relativePath);
                if (System.IO.File.Exists(physicalPath))
                    System.IO.File.Delete(physicalPath);

                /* 4. cascade delete (DB now has ON DELETE CASCADE) ---------------*/
                db.documents.Remove(perDoc.document);   // child perDoc disappears automatically
                db.SaveChanges();

                /* 5. activity log --------------------------------------------------*/
                string msg = $"DOC: {docName} | CATEGORY: {catName} | FILE: {fileName}";
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Admin FAQ Deleted",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = msg,
                    source = "personneldocuments/DeleteAdminfaqs",
                    url = "personneldocuments/DeleteAdminfaqs",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Document deleted successfully!";
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Delete Admin FAQ Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "personneldocuments/DeleteAdminfaqs",
                    url = "personneldocuments/DeleteAdminfaqs",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Delete failed: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        /*****************************************************************/
        /*****************************************************************/

        public ActionResult Uploaddocument(string id)
        {
            var personneldocuments = db.personneldocuments.Include(p => p.document).Include(p => p.personel_files_cat);
            return View(personneldocuments.ToList());
        }

        /*****************************************************************/
        /*****************************************************************/

        // GET: personneldocuments/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var doc = db.personneldocuments
                         .Include(p => p.document)
                         .Include(p => p.personel_files_cat)
                         .FirstOrDefault(p => p.id == id);

            if (doc == null) return HttpNotFound();
            return View(doc);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Uploaddocument(HttpPostedFileBase file,
                            string id,          // personneldocument id (for return to Details)
                            string cat,         // category id (dropdown)
                            string documentid,  // existing document id
                            string doc_order)
        {
            try
            {
                /* 1. Guard checks */
                if (string.IsNullOrWhiteSpace(documentid))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Document id missing.";
                    return RedirectToAction("Index", "personneldocuments");
                }
                if (string.IsNullOrWhiteSpace(cat))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Category not selected.";
                    return RedirectToAction("Details", new { id = id });
                }
                if (file == null || file.ContentLength == 0)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.";
                    return RedirectToAction("Details", new { id = id });
                }

                /* 2. Validate extension */
                string ext = Path.GetExtension(file.FileName).ToLower();
                string[] allowed = { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Only PDF or image files are allowed.";
                    return RedirectToAction("Details", new { id = id });
                }

                string detectedType;
                switch (ext)
                {
                    case ".pdf": detectedType = "PDF"; break;
                    case ".jpg":
                    case ".jpeg": detectedType = "JPEG"; break;
                    case ".png": detectedType = "PNG"; break;
                    case ".gif": detectedType = "GIF"; break;
                    case ".bmp": detectedType = "BMP"; break;
                    case ".webp": detectedType = "WebP"; break;
                    default: detectedType = "Unknown"; break;
                }

                /* 3. Save file */
                string fileName = $"{cat}_{Guid.NewGuid()}{ext}";
                string folder = Server.MapPath($"~/UploadedFiles/Files/{cat}");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string fullPath = Path.Combine(folder, fileName);
                file.SaveAs(fullPath);

                /* 4. Check document exists */
                var doc = db.documents.FirstOrDefault(d => d.id == documentid);
                if (doc == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Document not found.";
                    return RedirectToAction("Details", new { id = id });
                }

                /* 5. Create personneldocument row */
                var perDoc = new personneldocument
                {
                    id = Guid.NewGuid().ToString(),
                    documentid = documentid,
                    cat_id = cat,
                    doc_order = doc_order,
                    doctype = detectedType,
                    notes = "True",
                    description = doc.description,   // <-- copy description here
                    expdate = DateTime.Now.AddYears(1),
                    status = "True",
                    insertdate = DateTime.Now,
                    url = $"/UploadedFiles/Files/{cat}/{fileName}"
                };
                db.personneldocuments.Add(perDoc);
                db.SaveChanges();

                /* 6. Log */
                string catName = db.personel_files_cat.Find(cat)?.cat_name ?? "(none)";
                string msg = $"ADDED TO CATEGORY: {catName} | FILE: {fileName}";
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Document Added to Category",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = msg,
                    source = "personneldocuments/UploadDocument",
                    url = "personneldocuments/UploadDocument",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "File added to category successfully!";
                return RedirectToAction("Details", new { id = id });   // <-- back to same Details page
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Upload Document Failed",
                    status = "Error",
                    notes = Session["userid"]?.ToString(),
                    message = ex.ToString(),
                    source = "personneldocuments/UploadDocument",
                    url = "personneldocuments/UploadDocument",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Upload failed: " + ex.Message;
                return RedirectToAction("Details", new { id = id });   // <-- still back to same Details
            }
        }


        /*****************************************************************/
        /*****************************************************************/

        [HttpPost]
        public ActionResult updateDocument(string id, HttpPostedFileBase file)
        {
            var doc = db.personneldocuments.Find(id);
            var dtype = db.documents.FirstOrDefault(p => p.id == doc.documentid);
            //var crewname = db.crews.FirstOrDefault(p => p.id == doc.crewid);
            var pcat = db.personel_files_cat.FirstOrDefault(p => p.id == doc.cat_id);

            try
            {

                if (file.ContentLength > 0)
                {
                    //string _FileName = Path.GetFileName(file.FileName);
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);
                    //creating filename to avoid file name conflicts.
                    string fileName = id;
                    //saving file in savedImage folder.
                    //string savePath = savelocation + fileName + fileExtention;

                    var folder = Server.MapPath("~/UploadedFiles/Files/" + doc.documentid);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles/Files/" + doc.documentid), file.FileName);//fileName + fileExtention
                    file.SaveAs(_path);

                    string url = "/UploadedFiles/Files/" + doc.documentid + "/" + file.FileName;

                    doc.url = url;
                    doc.insertdate = DateTime.Now;

                    //create activity log
                    //
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "AdminFaq Document updated Successfully",
                        status = "Access Granted Successsfully",
                        notes = Session["userid"].ToString(),
                        message = "DOC NAME: " + dtype.name + " FILE CATE: " + pcat.cat_name + " CREW NAME: " + " FILE NAME: " + file.FileName,
                        source = "AdminFaq/updateDocument",
                        url = "AdminFaq/updateDocument",
                        logtype = "success",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"].ToString()
                    });
                    db.SaveChanges();

                    TempData["success"] = "true";
                    TempData["message"] = "Uploaded Successfully!!";
                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.!!";
                    return RedirectToAction("Details", new { id = id });
                }
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while uploading Crew document",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "AdminFaq/updateDocuments/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild to submit, please review the entry and try again." + err.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

        /*****************************************************************/
        /*****************************************************************/
        public ActionResult EditDocument(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]   // allows HTML only in this action
        public ActionResult EditDocument(string id, string documentid, string cat) 
        {
            var doc = db.personneldocuments.Find(id);
            var dtype = db.documents.FirstOrDefault(p => p.id == documentid);
            //var crewname = db.crews.FirstOrDefault(p => p.id == doc.crewid);
            var pcat = db.personel_files_cat.FirstOrDefault(p => p.id == cat);

            try
            {
                doc.documentid = documentid;
                doc.cat_id = cat;

                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Edited Adminfaq Title Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = "DOC NAME: " + dtype.name + " FILE CAT: " + pcat.cat_name,
                    source = "Adminfaq/Edit/" + id,
                    url = "Adminfaq/Edit/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Updated sucessfully.!!";
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while editing Adminfaq",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Adminfaq/edit/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild to submit, please review the entry and try again." + err.Message;
                return RedirectToAction("Details", new { id = id });

            }
        }
        /*****************************************************************/
        /*****************************************************************/

        // GET: DeleteDocument/Delete/5
        public ActionResult DeleteDocument(int id)
        {
            return View();
        }

        // POST: DeleteDocument/Delete/5
        [HttpPost]
        public ActionResult DeleteDocument(string id)
        {
            var doc = db.personneldocuments.Find(id);
            var dtype = db.documents.FirstOrDefault(p => p.id == doc.documentid);
            //var crewname = db.crews.FirstOrDefault(p => p.id == doc.crewid);
            var pcat = db.personel_files_cat.FirstOrDefault(p => p.id == doc.cat_id);
            string parentDocId = doc.documentid;  // This is key: redirect to parent document
            try
            {

                // TODO: Add delete logic here
                db.personneldocuments.Remove(db.personneldocuments.Find(id));

                //db.personneldocuments.Remove(doc);


                //delete folder
                //var folder = Server.MapPath("~/UploadedFiles/Files/" + doc.id);
                //Directory.Delete(folder, true); 

                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Crew document Delete Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = "DOC NAME: " + dtype.name + " FILE CAT: " + pcat.cat_name + " FILE NAME: " + doc.url,
                    source = "Documentation/delete/" + id,
                    url = "Documentation/delete/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Deleted sucessfully.!!";
                return RedirectToAction("Index", "personneldocuments");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting Crew Documentation",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Documentation/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild to submit, please review the entry and try again." + err.Message;
                return RedirectToAction("Index", "personneldocuments");
            }
        }


        /*****************************************************************/
        /*****************************************************************/


        // GET: personneldocuments/Create
        public ActionResult Create()
        {
            ViewBag.documentid = new SelectList(db.documents, "id", "name");
            ViewBag.cat_id = new SelectList(db.personel_files_cat, "id", "cat_name");
            return View();
        }

        /*****************************************************************/
        /*****************************************************************/

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditAdminfaq(string id,        // personneldocument id
                                       string name,
                                       string cat_id,
                                       string description)
        {
            try
            {
                var perDoc = db.personneldocuments
                               .Include(p => p.document)
                               .Include(p => p.personel_files_cat)
                               .FirstOrDefault(p => p.id == id);

                if (perDoc == null) return HttpNotFound();

                /* 1. document table */
                perDoc.document.name = name;
                perDoc.document.description = setEditor.getJsonString(description);
                perDoc.document.updatedate = DateTime.Now;
                perDoc.document.updateby = Session["email"]?.ToString();

                /* 2. personneldocument table */
                perDoc.cat_id = cat_id;
                perDoc.description = setEditor.getJsonString(description);

                /* 3. save with validation trap --------------------------------*/
                try
                {
                    db.SaveChanges();   // business save
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    var errorLines = dbEx.EntityValidationErrors
                                         .SelectMany(v => v.ValidationErrors)
                                         .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                    string fullMsg = string.Join(" | ", errorLines);

                    TempData["success"] = "false";
                    TempData["message"] = fullMsg;
                    return RedirectToAction("Edit", new { id = id });
                }

                /* 4. activity log --------------------------------------------*/
                string catName = db.personel_files_cat.Find(cat_id)?.cat_name ?? "(none)";
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Document Edited",
                    status = "Access Granted",
                    notes = Session["userid"]?.ToString(),
                    message = $"DOC: {name} | CATEGORY: {catName}",
                    source = "personneldocuments/EditAdminfaq",
                    url = "personneldocuments/EditAdminfaq",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"]?.ToString()
                });

                /* 5. log save (wrapped too) ----------------------------------*/
                try
                {
                    db.SaveChanges();   // log save
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    var errorLines = dbEx.EntityValidationErrors
                                         .SelectMany(v => v.ValidationErrors)
                                         .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                    string fullMsg = string.Join(" | ", errorLines);

                    TempData["success"] = "false";
                    TempData["message"] = "Log save failed: " + fullMsg;
                    return RedirectToAction("Edit", new { id = id });
                }

                TempData["success"] = "true";
                TempData["message"] = "Saved successfully.";
                return RedirectToAction("Index", "personneldocuments");
            }
            catch (Exception ex)
            {
                /* 6. any other exception -------------------------------------*/
                TempData["success"] = "false";
                TempData["message"] = "Save failed: " + ex.Message;
                return RedirectToAction("Edit", new { id = id });
            }
        }
        /*****************************************************************/
        /*****************************************************************/

        // POST: personneldocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,doc_order,description,expdate,insertdate,notes,status,url,doctype,documentid,cat_id")] personneldocument personneldocument)
        {
            if (ModelState.IsValid)
            {
                db.personneldocuments.Add(personneldocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.documentid = new SelectList(db.documents, "id", "name", personneldocument.documentid);
            ViewBag.cat_id = new SelectList(db.personel_files_cat, "id", "cat_name", personneldocument.cat_id);
            return View(personneldocument);
        }

        // GET: personneldocuments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personneldocument personneldocument = db.personneldocuments.Find(id);
            if (personneldocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.documentid = new SelectList(db.documents, "id", "name", personneldocument.documentid);
            ViewBag.cat_id = new SelectList(db.personel_files_cat, "id", "cat_name", personneldocument.cat_id);
            return View(personneldocument);
        }

        // POST: personneldocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,doc_order,description,expdate,insertdate,notes,status,url,doctype,documentid,cat_id")] personneldocument personneldocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personneldocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.documentid = new SelectList(db.documents, "id", "name", personneldocument.documentid);
            ViewBag.cat_id = new SelectList(db.personel_files_cat, "id", "cat_name", personneldocument.cat_id);
            return View(personneldocument);
        }

        // GET: personneldocuments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personneldocument personneldocument = db.personneldocuments.Find(id);
            if (personneldocument == null)
            {
                return HttpNotFound();
            }
            return View(personneldocument);
        }

        // POST: personneldocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    personneldocument personneldocument = db.personneldocuments.Find(id);
        //    db.personneldocuments.Remove(personneldocument);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
