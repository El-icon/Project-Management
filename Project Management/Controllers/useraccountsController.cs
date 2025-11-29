using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_Management.Models;
using Project_Management.Setup;

namespace Project_Management.Controllers
{
    public class useraccountsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        //[CheckAuthentication]
        // GET: useraccounts

        //public ActionResult Index()
        //{
        //    return View(db.useraccounts.ToList());
        //}



        [CheckAuthentication]
        public ActionResult Index()
        {
            var users = db.useraccounts.ToList();

            // Decrypt each user's password before sending to view
            foreach (var u in users)
            {
                try
                {
                    u.password = Project_Management.Setup.CryptoEngine.Decrypt(u.password);
                }
                catch
                {
                    u.password = "ERROR"; // fallback if decryption fails
                }
            }

            return View(users);
        }


        //Login Page 
        public ActionResult Login()
        {   //
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            //string email = form["email"];
            //string password = form["password"];
            List<string> Meassage = new List<string>();
            string clearpassword;
            Meassage.Add("Login failed. Please review your credentials and login again!! ");

            var User = db.useraccounts.FirstOrDefault(u => u.email == email);
            if (User != null)
            {
                clearpassword = Project_Management.Setup.CryptoEngine.Decrypt(User.password);
                if (password == clearpassword)
                {
                    Session["userid"] = User.id.ToString();
                    Session["email"] = User.email.ToString();
                    Session["usertype"] = User.usertype;
                    // Session["branchid"] = User.branchid;

                    db.ActivityLogs.Add(new ActivityLog
                    {
                        id = Guid.NewGuid().ToString(),
                        code = "200",
                        title = "Login Successfully",
                        status = "Access Granted successsfully",
                        notes = Session["userid"].ToString(),
                        message = "Login Successfully",
                        source = "useraccounts/login",
                        url = "useraccounts/Login",
                        logtype = "user",
                        insertdate = DateTime.Now.ToString(),
                        insertuser = Session["email"].ToString()
                    });
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = Meassage[0];
                    return View();
                }
            }
            else
            {
                ViewBag.error = Meassage[0];
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string id, string name, string address, string notes,
            string email, string phone, string insertdate, string status, string usertype, string password)
        {
            try
            {
                db.useraccounts.Add(new useraccount
                {
                    id = Setup.GenerateID.GetID(),
                    name = name,
                    address = address,
                    email = email,
                    phone = phone,
                    password = Setup.CryptoEngine.Encrypt(password),
                    status = "ACTIVE",
                    usertype = "ADMIN",
                    notes = "INFO"
                    //insertdate = DateTime.Now,
                });

                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "User Registered Successfully",
                    status = "Access Granted successsfully",
                    //notes = Session["userid"].ToString(),
                    message = name,
                    source = "useraccounts/Create",
                    url = "useraccounts/Create",
                    logtype = "user",
                    insertdate = DateTime.Now.ToString(),
                    //insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "You have successfully registered. Please Login to continue.";
                return RedirectToAction("Login", "useraccounts");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating User",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "useraccounts/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "useraccounts");
            }
        }


        //Log out User from the system
        public ActionResult Logout()
        {
            //db.ActivityLogs.Add(new ActivityLog
            //{
            //    id = Guid.NewGuid().ToString(),
            //    code = "200",
            //    title = "Logout Successfully",
            //    status = "Access Granted successsfully",
            //    notes = Session["userid"].ToString(),
            //    message = "Logout Successfully",
            //    source = "UserAccounts/login",
            //    url = "UserAccounts/Login",
            //    logtype = "user",
            //    insertdate = DateTime.Now.ToString(),
            //    insertuser = Session["email"].ToString()
            //});
            //db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Login", "UserAccounts");
        }


        [CheckAuthentication]
        // GET: useraccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }
        [CheckAuthentication]
        // GET: useraccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [CheckAuthentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, string name, string email, string password,
            string phone, string address, string notes, string status, string usertype, string insertdate)
        {
            try
            {
                db.useraccounts.Add(new useraccount
                {
                    id = Setup.GenerateID.GetID(),
                    name = name,
                    email = email,
                    password = Setup.CryptoEngine.Encrypt(password),
                    phone = phone,
                    address = address,
                    notes = notes,
                    status = status,
                    usertype = usertype
                    //insertdate = DateTime.Now.ToString(),
                });

                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "User Registered Successfully",
                    status = "Access Granted Successsfully",
                    notes = Session["userid"].ToString(),
                    message = " FULL NAME: " + name + " EMAIL: " + email + " PHONE: " + phone + " ADDRESS: " + address + " PASSWORD ENCRIPTED " + " STATUS: " + status + " USERTYPE: " + usertype + " NOTE: " + notes,
                    source = "useraccounts/Create",
                    url = "useraccounts/Create",
                    logtype = "user",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "You have successfully registered. Please Login to continue.";
                return RedirectToAction("Index", "useraccounts");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while creating User",
                    status = "Unable to save",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "useraccounts/Create",
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again." + err;
                return RedirectToAction("Index", "useraccounts");
            }
        }

        [CheckAuthentication]
        // GET: useraccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.        
        [CheckAuthentication]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string name, string email, string password,
    string phone, string address, string notes, string status, string usertype, string insertdate)
        {
            try
            {
                var useraccount = db.useraccounts.Find(id);

                if (useraccount == null)
                {
                    TempData["success"] = "false";
                    TempData["message"] = "User not found.";
                    return RedirectToAction("Index", "useraccounts");
                }

                // Always encrypt before saving (because password from view is already decrypted plain text)
                useraccount.password = Project_Management.Setup.CryptoEngine.Encrypt(password);

                // Update other fields
                useraccount.name = name;
                useraccount.email = email;
                useraccount.address = address;
                useraccount.phone = phone;
                useraccount.notes = notes;
                useraccount.status = status;
                useraccount.usertype = usertype;

                // Log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Edited User Successfully",
                    status = "Access Granted successfully",
                    notes = Session["userid"].ToString(),
                    message = $" FULL NAME: {name} EMAIL: {email} PHONE: {phone} ADDRESS: {address} STATUS: {status} USERTYPE: {usertype} NOTE: {notes}",
                    source = "useraccounts/Edit/" + id,
                    url = "useraccounts/Edit/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });

                db.SaveChanges();

                TempData["success"] = "true";
                TempData["message"] = "Edited successfully.";
                return RedirectToAction("Index", "useraccounts");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while editing useraccount",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "useraccounts/edit/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Failed, please review the fields and try again." + err;
                return View(db.useraccounts.Find(id));
            }
        }




        public ActionResult ResetPassword(string id)
        {
            return View(db.useraccounts.Find(id));
        }
        [HttpPost]
        public ActionResult ResetPassword(string id, string password)
        {
            try
            {
                var user = db.useraccounts.FirstOrDefault(p => p.id == id);
                user.password = Setup.CryptoEngine.Encrypt(password);
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Password reset Success!!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["success"] = "false";
                TempData["message"] = "Failed to reset Password!!";
                return RedirectToAction("ResetPassword/" + id, "useraccounts");
            }
        }

        [CheckAuthentication]
        // GET: useraccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            useraccount useraccount = db.useraccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }
        [CheckAuthentication]
        // POST: useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var pub = db.useraccounts.Find(id);
                db.useraccounts.Remove(db.useraccounts.Find(id));
                //Create Activity log
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "200",
                    title = "Deleted useraccount successfully",
                    status = "Access Granted successsfully",
                    notes = Session["userid"].ToString(),
                    message = "Deleted a useraccount successsfully: " + pub.email,
                    source = "useraccounts/delete/" + id,
                    url = "useraccounts/delete/" + id,
                    logtype = "success",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Deleted sucessfully.";
                return RedirectToAction("Index", "useraccounts");
            }
            catch (Exception err)
            {
                db.ActivityLogs.Add(new ActivityLog
                {
                    id = Guid.NewGuid().ToString(),
                    code = "500",
                    title = "An error occured while deleting User",
                    notes = Session["userid"].ToString(),
                    message = err.ToString(),
                    source = err.Source,
                    url = "useraccounts/delete/" + id,
                    logtype = "error",
                    insertdate = DateTime.Now.ToString(),
                    insertuser = Session["email"].ToString()
                });
                db.SaveChanges();

                TempData["success"] = "false";
                TempData["message"] = "Faild, please review the fields and try again." + err;
                return RedirectToAction("Index", "useraccounts");
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
