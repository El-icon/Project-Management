using Project_Management.Models;
using Project_Management.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class CompaniesController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Companies
        public ActionResult Index()
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

                return RedirectToAction("Edit", new { id = id });
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
        public ActionResult Create(string id, string companyname, string companyemail, string companyphone, 
            string companywebsite, string timezone, string language, string currency, string  address, 
            string acctname, string acctemail, string packageid, string details, string insertdate, 
            string status, string photo, string url, string expdate, string employees, string clients, 
            string invoices, string storage, string estimates, string projects, string tasks, string leads, 
            string orders, string tickets, string contacts, string amount, string paymentdate, 
            string nextpaymentdate, string licenceexpireson)
        {
             var pack = db.Packages.Find(packageid); 

            try
            {
                db.Companies.Add(new Company
                {
                    id = Guid.NewGuid().ToString(),
                    companyname = companyname,
                    companyemail = companyemail,
                    companyphone = companyphone,
                    companywebsite = companywebsite,
                    timezone = timezone,
                    language = language,
                    currency = currency,
                    address = address,
                    acctname = acctname,
                    acctemail = acctemail,
                    packageid = packageid,
                    details = details,
                    photo = photo,
                    url = url,
                    expdate = expdate,
                    employees = employees,
                    clients = clients,
                    invoices = invoices,
                    storage = storage,
                    estimates = estimates,
                    projects = projects,
                    tasks = tasks,
                    leads = leads,
                    amount = amount,
                    paymentdate = paymentdate,
                    nextpaymentdate = nextpaymentdate, 
                    licenceexpireson = licenceexpireson,
                    orders = orders,
                    tickets = tickets,
                    contacts = contacts, 
                    status = status,
                    insertdate = DateTime.Now.ToString(),
                });
                //create activity log
                //
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Created Company Successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(), 
                    message = "COMPANY NAME.: " + companyname + " COMPANY EMAIL: " + companyemail + " COMPANY PHONE: " + companyphone 
                    + " COMPANY WEBSITE: " + companywebsite + " ACCT NAME: " + acctname + " ACCT EMAIL: " + acctemail + " PACKAGE NAME: " 
                    + pack.packagename + " CURRENCY: " + currency + " TIMEZONE: " + timezone + " ADDRESS: " + address + " LANGUAGE: " 
                    + language + " STATUS: " + status + " AMOUNT: " + amount + " PAYMENT DATE : " + paymentdate + " NEXT PAYMENT DATE: " 
                    + nextpaymentdate + " LICENCE EXPIRE ON: " + licenceexpireson,
                    source = "Company/Create",
                    url = "Company/Create",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Company Saved sucessfully.";
                return RedirectToAction("Index", "Companies");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating Company",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Company/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "Companies");
            }
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
            ViewBag.packageid = new SelectList(db.Packages, "id", "monthlyprice", company.packageid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "annualprice", company.packageid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagetype", company.packageid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "packagecategory", company.packageid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "currency", company.packageid);
            ViewBag.packageid = new SelectList(db.Packages, "id", "description", company.packageid);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string companyname, string companyemail, 
            string companyphone, string companywebsite, string timezone, string language, 
            string currency, string address, string acctname, string acctemail, 
            string packageid, string details, string insertdate, string status, 
            string photo, string url, string expdate, string employees, string clients, 
            string invoices, string storage, string estimates, string projects, string tasks, 
            string leads, string orders, string tickets, string contacts, string amount, 
            string paymentdate, string nextpaymentdate, string licenceexpireson)
        {
            //var pack = db.Packages.Find(packageid);
            try
            {
                //edit
                var company = db.Companies.Find(id);
                //company.packageid = packageid;
                company.companyname = companyname;
                company.companyemail = companyemail;
                company.companyphone = companyphone;
                company.companywebsite = companywebsite;
                company.timezone = timezone;
                company.language = language;
                company.currency = currency;
                company.address = address;
                company.acctname = acctname;
                company.acctemail = acctemail;
                company.details = details;
                company.photo = photo;
                company.url = url;
                company.expdate = expdate;
                company.employees = employees;
                company.clients = clients;
                company.invoices = invoices;
                company.storage = storage;
                company.estimates = estimates;
                company.projects = projects;
                company.tasks = tasks;
                company.leads = leads;
                //company.amount = amount;
                //company.paymentdate = paymentdate;
                //company.nextpaymentdate = nextpaymentdate;
                //company.licenceexpireson = licenceexpireson;
                company.orders = orders;
                company.tickets = tickets;
                company.contacts = contacts;
                company.status = status;
                company.insertdate = DateTime.Now.ToString();

                //create activity log
                //
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Edited Company Info Successfully",
                    status = "Access Granted successfully",
                    notes = Session["userid"].ToString(),
                    message = "COMPANY NAME.: " + companyname + " COMPANY EMAIL: " 
                    + companyemail + " COMPANY PHONE: " + companyphone 
                    + " COMPANY WEBSITE: " + companywebsite + " ACCT NAME: " 
                    + acctname + " ACCT EMAIL: " + acctemail + " CURRENCY: " 
                    + currency + " TIMEZONE: " + timezone + " ADDRESS: " 
                    + address + " LANGUAGE: " + language + " STATUS: " + status,
                    source = "Companies/Index",
                    url = "Companies/Index",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                // return RedirectToAction("Index");
                TempData["success"] = "true";
                TempData["message"] = "Edited successfully.";
                return RedirectToAction("Index", "Companies");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while Editing Companies",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "Companies/Index",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return View(db.Companies.Find(id));
            }
        }

        //public ActionResult Updatepackage(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Company company = db.Companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "packagename", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "monthlyprice", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "annualprice", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "packagetype", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "packagecategory", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "currency", company.packageid);
        //    ViewBag.packageid = new SelectList(db.Packages, "id", "description", company.packageid);
        //    return View(company);
        //}
        // GET: Companies/Updatepackage/{id}
        public ActionResult Updatepackage(string id)
        {
            var company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }

            ViewBag.Packages = db.Packages.ToList(); // for dropdown
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Updatepackage(string id, string packageid, string amount, string paymentdate, string nextpaymentdate, string licenceexpireson)
        {
            try
            {
                var company = db.Companies.FirstOrDefault(c => c.id == id);
                if (company == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "Company not found.";
                    return RedirectToAction("Index", "Companies");
                }

                company.packageid = packageid;
                company.amount = amount;
                company.paymentdate = paymentdate;
                company.nextpaymentdate = nextpaymentdate;
                company.licenceexpireson = licenceexpireson;
                company.insertdate = DateTime.Now.ToString();

                var package = db.Packages.Find(packageid);

                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Company package updated",
                    status = "Success",
                    notes = Session["userid"].ToString(),
                    message = "PACKAGE: " + package?.packagename
                              + " | PAYMENT DATE: " + paymentdate
                              + " | NEXT PAYMENT DATE: " + nextpaymentdate
                              + " | LICENCE EXPIRES ON: " + licenceexpireson,
                    source = "Companies/Updatepackage",
                    url = "Companies/Index",
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });

                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Package updated successfully.";
                return RedirectToAction("Index", "Companies");
            }
            catch (Exception ex)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "Error updating company package",
                    status = "Failed",
                    notes = Session["userid"].ToString(),
                    message = ex.ToString(),
                    source = ex.Source,
                    url = "Companies/Index",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });

                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed to update package. Please try again.";
                return RedirectToAction("Index", "Companies");
            }
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
            try
            {
                //delete Companies
                foreach (var item in db.Companies.Where(p => p.packageid == id).ToList())
                {
                    //delete packageid crew
                    foreach (var row in db.Companies.Where(p => p.packageid == item.id).ToList())
                    {
                        //remove flight crew
                        db.Packages.Remove(db.Packages.Find(row.id));
                    }
                    //remove log sector
                    db.Companies.Remove(db.Companies.Find(item.id));
                }


                Company Company = db.Companies.Find(id);
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Company Deleted Successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(),
                    message = "COMPANY ID: " + Company.companyname,
                    source = "Company/delete/" + Company.companyname,
                    url = "Company/delete/" + Company.companyname,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.Companies.Remove(Company);
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deleted Sucessfully.";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting tech_log",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "tech_log/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
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

