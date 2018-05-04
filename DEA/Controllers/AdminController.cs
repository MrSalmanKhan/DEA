using DEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEA.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult validate(User user)
            {
            DBEntities db = new DBEntities();
            try
            {
                using (db)
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                        return View();

                    //Retrive Stored HASH Value From Database According To Username (one unique field)
                    var userInfo = db.Users.Where(s => s.UserName == user.UserName.Trim() && s.Password == user.Password).FirstOrDefault();

                    //Assign HASH Value
                    if (userInfo != null)
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Access Denied! Wrong Credential";
                        return View("AdminLogin",user);
                    }
                }
        }
            catch
            {
                throw;
                return View("AdminLogin");
            }
           
        }
    }
}