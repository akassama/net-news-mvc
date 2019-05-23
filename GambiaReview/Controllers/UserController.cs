using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GambiaReview.Models;

namespace GambiaReview.Controllers
{
    public class UserController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: User
        [SessionAuthorize]
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------LOGOUT------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        // GET: /User/Logout
        public ActionResult Logout()
        {
            Session.Remove("sessionID");
            Session.Remove("sessionEmail");
            Session.Remove("sessionFullName");

            return RedirectToAction("Index", "Home");
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
