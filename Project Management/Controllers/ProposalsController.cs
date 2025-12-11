
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
   
        public class ProposalsController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: Proposals
        // GET: Deals
        public ActionResult Index()
        {
            var deals = db.Deals.Include(d => d.Lead_Contact);
            var deals1 = db.Deals.Include(d => d.Employee);
            return View(deals.ToList());
        }

        // GET: Deals/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

       
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }  
     
        public ActionResult Template()
        {
            return View();
        }
    }
}


