using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DEA.Models;
namespace DEA.Controllers
{
    public class AccountController : Controller
    {
        DEA_DBEntities db = new DEA_DBEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login_User()
        {

            var un = Request.Form["UserName"];
            var pass = Request.Form["Password"];
            var p = db.Users.Where(x => x.UserName == un).Select(x => x).FirstOrDefault();
            if(p!=null)
            {
                if (p.Password == pass && p.RoleID == 1)
                {
                    return RedirectToAction("Index","Admin");
                }
            }
           
            return RedirectToAction("Login");
        }
    }
}