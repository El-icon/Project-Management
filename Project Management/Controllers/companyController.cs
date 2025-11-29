using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using Project_Management.Models;
using Project_Management.Setup;


namespace Project_Management.Controllers
{
    public class companyController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.Package);
            return View(companies.ToList());
        }
        public ActionResult company()
        {
            var companies = db.Companies.Include(c => c.Package);
            return View(companies.ToList());
        }
        public ActionResult uploadProfilePicture(HttpPostedFileBase file, string id)
        {
            try
            {
                //string id = Guid.NewGuid().ToString();
                if (file.ContentLength > 0)
                {
                    //string _FileName = Path.GetFileName(file.FileName);
                    string fileExtention = System.IO.Path.GetExtension(file.FileName);
                    //creating filename to avoid file name conflicts.
                    string fileName = id;
                    //saving file in savedImage folder.
                    //string savePath = savelocation + fileName + fileExtention;

                    var folder = Server.MapPath("~/UploadedFiles/profile/" + id);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles/profile/" + id), file.FileName);//fileName + fileExtention
                    file.SaveAs(_path);


                    string url = "/UploadedFiles/profile/" + id + "/" + file.FileName;

                    var company = db.Companies.Find(id);
                    company.photo = url;

                    //create activity log
                    //
                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Created uploadProfilePicture Successfully",
                        status = "Access Granted successsfully",
                        notes = Session["userid"].ToString(),
                        message = fileName,
                        source = "uploadProfilePicture/Uploaded",
                        url = "uploadProfilePicture/Uploaded",
                        logtype = "success",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"].ToString()
                    });
                    db.SaveChanges();
                    TempData["success"] = "true";
                    TempData["message"] = "Uploaded Successfully!!";
                    return RedirectToAction("Companies", new { id = id });
                }
                else
                {
                    TempData["success"] = "false";
                    TempData["message"] = "No file selected.!!";

                    return RedirectToAction("Companies", new { id = id });
                }
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating uploadProfilePicture",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "uploadProfilePicture/Uploaded",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild to submit, please review the entry and try again." + err.Message;
                //return View(db.studenterollments.FirstOrDefault(p => p.id == admissionid));

                return RedirectToAction("Companies", new { id = id });
            }
        }


        // GET: Companies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,companyname,packageid,details,insertdate,status,photo,url,expdate,employees,clients")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", company.packageid);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", company.packageid);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,companyname,packageid,details,insertdate,status,photo,url,expdate,employees,clients")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", company.packageid);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
