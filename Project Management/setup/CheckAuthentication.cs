using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Project_Management.Setup
{       //Seasion create 
    public class CheckAuthentication : FilterAttribute, IAuthorizationFilter
    {
        //private DASOLAGFEEDEntities db = new DASOLAGFEEDEntities();
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["userid"] == null)
            {
                filterContext.Result = new RedirectResult("/useraccounts/login");
            }
        }
    }

}
