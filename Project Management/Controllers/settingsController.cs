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

    public class settingsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: settings
        public ActionResult Index()
        {
            var settings = db.settings.Include(s => s.settings_categories);
            return View(settings.ToList());
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

                var faq = db.settings_categories.Find(id);
                if (faq == null) return Json("Record not found.", JsonRequestBehavior.AllowGet);

                faq.name = newTitle.Trim();
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
        public ActionResult settings_categories([Bind(Include = "id,name")] settings_categories settings_categories)
        {
            try
            {
                if (!ModelState.IsValid) return View(settings_categories);

                settings_categories.id = Guid.NewGuid().ToString();
                db.settings_categories.Add(settings_categories);
                db.SaveChanges();

                /*  activity log  */
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created Setting Category Successfully",
                    status = "Access Granted Successfully",
                    notes = Session["userid"]?.ToString(),
                    message = "CATEGORY NAME: " + settings_categories.name,
                    source = "settings_categories/Create",
                    url = "settings_categories/Create",
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
                    title = "An error occurred while creating settings_categories",
                    status = "Unable to save",
                    notes = Session["userid"]?.ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "settings_categories/Create",
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
        [HttpPost, ActionName("deletesettings_cat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var category = db.settings_categories.Find(id);
                if (category != null)
                {
                    // delete related FAQs
                    // db.documents.RemoveRange(db.documents.Where(f => f.cat_id == id));

                    // delete category
                    db.settings_categories.Remove(category);
                    db.SaveChanges();

                    // log success
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Deleted settings_categories Successfully",
                        status = "Access Granted Successfully",
                        notes = Session["userid"]?.ToString(),
                        message = "CATEGORY NAME: " + category.name,
                        source = "settings_categories/Delete",
                        url = "settings_categories/Delete",
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
                    url = "settings_categories/Delete",
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











        // GET: settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting setting = db.settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: settings/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.settings_categories, "id", "name");
            return View();
        }

        // POST: settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,category_id,name,value,created_at,updated_at")] setting setting)
        {
            if (ModelState.IsValid)
            {
                db.settings.Add(setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.settings_categories, "id", "name", setting.categoryid);
            return View(setting);
        }

        // GET: settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting setting = db.settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.settings_categories, "id", "name", setting.categoryid);
            return View(setting);
        }

        // POST: settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,category_id,name,value,created_at,updated_at")] setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.settings_categories, "id", "name", setting.categoryid);
            return View(setting);
        }

        // GET: settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting setting = db.settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            setting setting = db.settings.Find(id);
            db.settings.Remove(setting);
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
