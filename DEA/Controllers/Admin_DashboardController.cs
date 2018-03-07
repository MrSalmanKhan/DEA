using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OfficeOpenXml;
using DEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DEA
{
    [Authorize(Roles = "Accountant,Admin,Principal")]
    public class Admin_DashboardController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private DEA_DBEntities db = new DEA_DBEntities();
        // GET: Admin_Dashboard
        public ActionResult Index()
        {
            return View();
            
        }


        public ActionResult test1()
        {
            return View();
        }

        public Admin_DashboardController()
        {
            
        }

        public ViewResult Class_Assessment()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            var sessiionid = db.AspNetSessions.Where(p => p.Status == "Active").FirstOrDefault().Id;
            ViewBag.TermID = new SelectList(db.AspNetTerms.Where(x => x.SessionID == sessiionid), "Id", "TermName", "TermNo");
            return View("_ClassAssessment");
        }

        public Admin_DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Dashboard()
        {
            return View("BlankPage");
        }


        /*******************************************************************************************************************/
        /*                                                                                                                 */
        /*                                    Accountant's Functions                                                       */
        /*                                                                                                                 */
        /*******************************************************************************************************************/

        public ActionResult AccountantEdit()
        {
            string id = Request.Form["id"];

            var user = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            var details = db.AspNetEmployees.Where(x => x.UserId == id).FirstOrDefault();
            var transaction = db.Database.BeginTransaction();
            try
            {
                user.Name = Request.Form["Name"];
                user.UserName = Request.Form["UserName"];
                user.Email = Request.Form["Email"];
                user.PhoneNumber = Request.Form["cellNo"];

                details.PositionAppliedFor = Request.Form["appliedFor"];
                details.DateAvailable = Request.Form["dateAvailable"];
                details.JoiningDate = Request.Form["JoiningDate"];
                details.BirthDate = Request.Form["birthDate"];
                details.Nationality = Request.Form["nationality"];
                details.Religion = Request.Form["religion"];
                details.Gender = Request.Form["gender"];
                details.MailingAddress = Request.Form["mailingAddress"];
                details.Email = user.Email;
                details.Name = user.Name;
                details.UserName = user.UserName;
                details.CellNo = user.PhoneNumber;
                details.Landline = Request.Form["landLine"];
                details.SpouseName = Request.Form["spouseName"];
                details.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                details.SpouseOccupation = Request.Form["spouseOccupation"];
                details.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];
                details.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                details.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                details.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                details.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                details.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                details.Tax = Convert.ToInt32(Request.Form["Tax"]);
                details.EOP = Convert.ToInt32(Request.Form["EOP"]);

                db.SaveChanges();
                transaction.Commit();
                return RedirectToAction("AccountantIndex", "AspNetUser");
            }
            catch
            {
                transaction.Dispose();
                return View("Sonething Went wrong");
            }

        }

        public ActionResult AccountantRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountantRegister(RegisterViewModel model)
        {
            if (1 == 1)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                var result = await UserManager.CreateAsync(user, model.Password);

                AspNetUser Accountant = new AspNetUser();
                Accountant.Name = user.Name;
                Accountant.UserName = user.UserName;
                Accountant.Email = user.Email;
                Accountant.PasswordHash = user.PasswordHash;
                Accountant.Status = "Active";
                Accountant.PhoneNumber = Request.Form["cellNo"];

                AspNetEmployee emp = new AspNetEmployee();
                emp.Email = Accountant.Email;
                emp.UserName = Accountant.UserName;
                emp.Name = Accountant.Name;
                emp.PositionAppliedFor = Request.Form["appliedFor"];
                emp.DateAvailable = Request.Form["dateAvailable"];
                emp.JoiningDate = Request.Form["JoiningDate"];
                emp.BirthDate = Request.Form["birthDate"];
                emp.Nationality = Request.Form["nationality"];
                emp.Religion = Request.Form["religion"];
                emp.Gender = Request.Form["gender"];
                emp.MailingAddress = Request.Form["mailingAddress"];
                emp.CellNo = Request.Form["cellNo"];
                emp.Landline = Request.Form["landLine"];
                emp.SpouseName = Request.Form["spouseName"];
                emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                emp.SpouseOccupation = Request.Form["spouseOccupation"];
                emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];
                emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
                emp.EOP = Convert.ToInt32(Request.Form["EOP"]);
                emp.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Management Staff").Select(x => x.Id).FirstOrDefault();
                emp.UserId = user.Id;
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(user.Id, "Accountant");

                    db.AspNetEmployees.Add(emp);
                    db.SaveChanges();
                    string Error = "Accountant Saved successfully";
                    return RedirectToAction("AccountantsIndex", "AspNetUser" , new { Error});
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult DisabledTeachers()
        {
            return View();
        }

        public JsonResult DisableTeachers()
        {
            var teachers = (from teacher in db.AspNetUsers.Where(x => x.Status == "False")
                            where teacher.AspNetRoles.Select(y => y.Name).Contains("Teacher")
                            select new
                            {
                                teacher.Id,
                                Class = teacher.AspNetClasses.Select(x => x.ClassName).FirstOrDefault(),
                                Subject = "-",
                                teacher.Email,
                                teacher.PhoneNumber,
                                teacher.UserName,
                                teacher.Name,
                            }).ToList();

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountantfromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["Accountants"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var AccountantList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    ApplicationDbContext context = new ApplicationDbContext();
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var Accountant = new RegisterViewModel();
                        Accountant.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        Accountant.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        Accountant.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        Accountant.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                        Accountant.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();
                        string number = workSheet.Cells[rowIterator, 14].Value.ToString();
                        var user = new ApplicationUser { UserName = Accountant.UserName, Email = Accountant.Email, Name = Accountant.Name, PhoneNumber = number };
                        var result = await UserManager.CreateAsync(user, Accountant.Password);
                        if (result.Succeeded)
                        {
                            AspNetEmployee AccountantDetail = new AspNetEmployee();
                            AccountantDetail.Name = Accountant.Name;
                            AccountantDetail.Email = Accountant.Email;
                            AccountantDetail.UserName = Accountant.UserName;
                            AccountantDetail.UserId = user.Id;
                            AccountantDetail.CellNo = user.PhoneNumber;
                            AccountantDetail.PositionAppliedFor = workSheet.Cells[rowIterator, 6].Value.ToString();
                            AccountantDetail.DateAvailable = workSheet.Cells[rowIterator, 7].Value.ToString();
                            AccountantDetail.JoiningDate = workSheet.Cells[rowIterator, 8].Value.ToString();
                            AccountantDetail.BirthDate = workSheet.Cells[rowIterator, 9].Value.ToString();
                            AccountantDetail.Nationality = workSheet.Cells[rowIterator, 10].Value.ToString();
                            AccountantDetail.Religion = workSheet.Cells[rowIterator, 11].Value.ToString();
                            AccountantDetail.Gender = workSheet.Cells[rowIterator, 12].Value.ToString(); ;
                            AccountantDetail.MailingAddress = workSheet.Cells[rowIterator, 13].Value.ToString();
                            AccountantDetail.CellNo = workSheet.Cells[rowIterator, 14].Value.ToString();
                            AccountantDetail.Landline = workSheet.Cells[rowIterator, 15].Value.ToString();
                            AccountantDetail.SpouseName = workSheet.Cells[rowIterator, 16].Value.ToString();
                            AccountantDetail.SpouseHighestDegree = workSheet.Cells[rowIterator, 17].Value.ToString();
                            AccountantDetail.SpouseOccupation = workSheet.Cells[rowIterator, 18].Value.ToString();
                            AccountantDetail.SpouseBusinessAddress = workSheet.Cells[rowIterator, 19].Value.ToString();
                            AccountantDetail.Illness = workSheet.Cells[rowIterator, 20].Value.ToString();
                            AccountantDetail.GrossSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 21].Value.ToString());
                            AccountantDetail.BasicSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 22].Value.ToString());
                            AccountantDetail.MedicalAllowance = Convert.ToInt32(workSheet.Cells[rowIterator, 23].Value.ToString());
                            AccountantDetail.Accomodation = Convert.ToInt32(workSheet.Cells[rowIterator, 24].Value.ToString());
                            AccountantDetail.ProvidedFund = Convert.ToInt32(workSheet.Cells[rowIterator, 25].Value.ToString());
                            AccountantDetail.Tax = Convert.ToInt32(workSheet.Cells[rowIterator, 26].Value.ToString());
                            AccountantDetail.EOP = Convert.ToInt32(workSheet.Cells[rowIterator, 27].Value.ToString());
                            AccountantDetail.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Management Staff").Select(x => x.Id).FirstOrDefault();
                            db.AspNetEmployees.Add(AccountantDetail);
                            db.SaveChanges();

                            var roleStore = new RoleStore<IdentityRole>(context);
                            var roleManager = new RoleManager<IdentityRole>(roleStore);
                            var userStore = new UserStore<ApplicationUser>(context);
                            var userManager = new UserManager<ApplicationUser>(userStore);
                            userManager.AddToRole(user.Id, "Accountant");
                        }
                        else
                        {
                            dbTransaction.Dispose();
                            AddErrors(result);
                            return View("AccountantRegister", model);
                        }

                    }
                    dbTransaction.Commit();
                    return RedirectToAction("AccountantIndex", "AspNetUser");
                }
            }
            catch (Exception)
            { dbTransaction.Dispose(); }

            return RedirectToAction("AccountantRegister", model);
        }

        /*******************************************************************************************************************
        * 
        *                                   Parent's Functions
        *                                    
        *******************************************************************************************************************/

        public ActionResult ParentRegister()
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ParentRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                try
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    IEnumerable<string> selectedstudents = Request.Form["StudentID"].Split(',');
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["fatherCell"] };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        AspNetParent parent = new AspNetParent();
                        parent.FatherName = Request.Form["fatherName"];
                        parent.FatherCellNo = Request.Form["fatherCell"];
                        parent.FatherEmail = Request.Form["fatherEmail"];
                        parent.FatherOccupation = Request.Form["fatherOccupation"];
                        parent.FatherEmployer = Request.Form["fatherEmployer"];
                        parent.MotherName = Request.Form["motherName"];
                        parent.MotherCellNo = Request.Form["motherCell"];
                        parent.MotherEmail = Request.Form["motherEmail"];
                        parent.MotherOccupation = Request.Form["motherOccupation"];
                        parent.MotherEmployer = Request.Form["motherEmployer"];
                        parent.UserID = user.Id;
                        db.AspNetParents.Add(parent);
                        db.SaveChanges();


                        foreach (var item in selectedstudents)
                        {
                            AspNetParent_Child par_stu = new AspNetParent_Child();
                            par_stu.ChildID = item;
                            par_stu.ParentID = user.Id;
                            db.AspNetParent_Child.Add(par_stu);
                            db.SaveChanges();
                        }

                        var roleStore = new RoleStore<IdentityRole>(context);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);

                        var userStore = new UserStore<ApplicationUser>(context);
                        var userManager = new UserManager<ApplicationUser>(userStore);
                        userManager.AddToRole(user.Id, "Parent");

                        dbTransaction.Commit();
                       
                    }
                    else
                    {
                        dbTransaction.Dispose();
                        AddErrors(result);
                        return View(model);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.InnerException);
                    dbTransaction.Dispose();
                    return View(model);
                }
            }
            string Error = "Parent successfully saved";
            return RedirectToAction("ParentsIndex", "AspNetUser", new { Error});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ParentEdit()
        {
            string id = Request.Form["Id"];
            var parent = db.AspNetParents.Where(x => x.UserID == id).Select(x => x).FirstOrDefault();
            var user = db.AspNetUsers.Where(x => x.Id == id).Select(x => x).FirstOrDefault();

            IEnumerable<string> selectedstudents = Request.Form["StudentID"].Split(',');
            user.UserName = Request.Form["UserName"];
            user.Name = Request.Form["Name"];
            user.Email = Request.Form["Email"];

            parent.FatherName = Request.Form["fatherName"];
            parent.FatherCellNo = Request.Form["fatherCell"];
            parent.FatherEmail = Request.Form["fatherEmail"];
            parent.FatherOccupation = Request.Form["fatherOccupation"];
            parent.FatherEmployer = Request.Form["fatherEmployer"];
            parent.MotherName = Request.Form["motherName"];
            parent.MotherCellNo = Request.Form["motherCell"];
            parent.MotherEmail = Request.Form["motherEmail"];
            parent.MotherOccupation = Request.Form["motherOccupation"];
            parent.MotherEmployer = Request.Form["motherEmployer"];

            var childs = db.AspNetParent_Child.Where(x => x.ParentID == user.Id).ToList();
            foreach (var item in childs)
            {
                db.AspNetParent_Child.Remove(item);
            }

            db.SaveChanges();

            db.AspNetUsers.Where(p => p.Id == id).FirstOrDefault().PhoneNumber = Request.Form["fatherCell"];
            db.SaveChanges();
            foreach (var item in selectedstudents)
            {
                AspNetParent_Child par_stu = new AspNetParent_Child();
                par_stu.ChildID = item;
                par_stu.ParentID = user.Id;
                db.AspNetParent_Child.Add(par_stu);
            }

            db.SaveChanges();

            return RedirectToAction("ParentIndex", "AspNetUser");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ParentRegisterFromFile(RegisterViewModel model)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["parents"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ApplicationDbContext context = new ApplicationDbContext();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var parent = new RegisterViewModel();
                            parent.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                            parent.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            parent.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                            parent.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                            parent.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();

                            var user = new ApplicationUser { UserName = parent.UserName, Email = parent.Email, Name = parent.Name };
                            var result = await UserManager.CreateAsync(user, parent.Password);
                            if (result.Succeeded)
                            {
                                AspNetParent parentDetail = new AspNetParent();
                                parentDetail.UserID = user.Id;
                                parentDetail.FatherName= workSheet.Cells[rowIterator, 6].Value.ToString(); 
                                parentDetail.FatherCellNo= workSheet.Cells[rowIterator, 7].Value.ToString();
                                parentDetail.FatherEmail= workSheet.Cells[rowIterator, 8].Value.ToString();
                                parentDetail.FatherOccupation= workSheet.Cells[rowIterator, 9].Value.ToString();
                                parentDetail.FatherEmployer= workSheet.Cells[rowIterator, 10].Value.ToString();
                                parentDetail.MotherName= workSheet.Cells[rowIterator, 11].Value.ToString();
                                parentDetail.MotherCellNo= workSheet.Cells[rowIterator, 12].Value.ToString();
                                parentDetail.MotherEmail= workSheet.Cells[rowIterator, 13].Value.ToString();
                                parentDetail.MotherOccupation= workSheet.Cells[rowIterator, 14].Value.ToString();
                                parentDetail.MotherEmployer= workSheet.Cells[rowIterator, 15].Value.ToString();
                                db.AspNetParents.Add(parentDetail);
                                db.SaveChanges();

                                var childUsernames = new List<string>();
                                childUsernames.Add(workSheet.Cells[rowIterator, 16].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 17].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 18].Value.ToString());
                                childUsernames.Add(workSheet.Cells[rowIterator, 19].Value.ToString());

                                var childIDs = (from student in db.AspNetUsers
                                                where childUsernames.Contains(student.UserName)
                                                select student.Id).ToList();
                                foreach (var item in childIDs)
                                {
                                    AspNetParent_Child par_stu = new AspNetParent_Child();
                                    par_stu.ChildID = item;
                                    par_stu.ParentID = user.Id;
                                    db.AspNetParent_Child.Add(par_stu);
                                    db.SaveChanges();
                                }

                                var roleStore = new RoleStore<IdentityRole>(context);
                                var roleManager = new RoleManager<IdentityRole>(roleStore);

                                var userStore = new UserStore<ApplicationUser>(context);
                                var userManager = new UserManager<ApplicationUser>(userStore);
                                userManager.AddToRole(user.Id, "Parent");

                            }
                            else
                            {
                                dbTransaction.Dispose();
                                AddErrors(result);
                                return View("ParentRegister", model);
                            }
                        }
                        dbTransaction.Commit();
                    }
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.InnerException);
                dbTransaction.Dispose();
                return View("ParentRegister", model);
            }
            return RedirectToAction("ParentIndex", "AspNetUser");
        }

        /*******************************************************************************************************************
         * 
         *                                    Teacher's Functions
         *                                    
         *******************************************************************************************************************/


        public ActionResult TeacherRegister()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult TeacherEdit(string id)
        {
            var user = db.AspNetUsers.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            
            user.Name = Request.Form["Name"];
            user.UserName = Request.Form["UserName"];
            user.Email = Request.Form["Email"];
            user.PhoneNumber = Request.Form["cellNo"];

            var emp = db.AspNetEmployees.Where(x => x.UserId == id).Select(x => x).FirstOrDefault();

            emp.Name = user.Name;
            emp.UserName = user.UserName;
            emp.Email = user.Email;
            emp.PositionAppliedFor = Request.Form["appliedFor"];
            emp.DateAvailable = Request.Form["dateAvailable"];
            emp.JoiningDate = Request.Form["JoiningDate"];
            emp.BirthDate = Request.Form["birthDate"];
            emp.Nationality = Request.Form["nationality"];
            emp.Religion = Request.Form["religion"];
            emp.Gender = Request.Form["gender"];
            emp.MailingAddress = Request.Form["mailingAddress"];
            emp.CellNo = Request.Form["cellNo"];
            emp.Landline = Request.Form["landLine"];
            emp.SpouseName = Request.Form["spouseName"];
            emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
            emp.SpouseOccupation = Request.Form["spouseOccupation"];
            emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];

            emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
            emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
            emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
            emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
            emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
            emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
            emp.EOP = Convert.ToInt32(Request.Form["EOP"]);

            db.SaveChanges();

            return RedirectToAction("TeachersIndex", "AspNetUser");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeacherRegister(RegisterViewModel model)
        {
            if (1==1)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name=model.Name, PhoneNumber=Request.Form["cellNo"]  };
                var result = await UserManager.CreateAsync(user, model.Password);

                AspNetUser Teacher = new AspNetUser();
                Teacher.Name = user.Name;
                Teacher.UserName = user.UserName;
                Teacher.Email = user.Email;
                Teacher.PasswordHash = user.PasswordHash;
                Teacher.Status = "Active";
                Teacher.PhoneNumber = Request.Form["cellNo"];

                AspNetEmployee emp = new AspNetEmployee();
                emp.Name = Teacher.Name;
                emp.UserName = Teacher.UserName;
                emp.Email = Teacher.Email;
                emp.PositionAppliedFor = Request.Form["appliedFor"];
                emp.DateAvailable = Request.Form["dateAvailable"];
                emp.JoiningDate =Request.Form["JoiningDate"];
                emp.BirthDate = Request.Form["birthDate"];
                emp.Nationality = Request.Form["nationality"];
                emp.Religion = Request.Form["religion"];
                emp.Gender = Request.Form["gender"];
                emp.MailingAddress = Request.Form["mailingAddress"];
                emp.CellNo = Request.Form["cellNo"];
                emp.Landline = Request.Form["landLine"];
                emp.SpouseName = Request.Form["spouseName"];
                emp.SpouseHighestDegree = Request.Form["spouseHighestDegree"];
                emp.SpouseOccupation = Request.Form["spouseOccupation"];
                emp.SpouseBusinessAddress = Request.Form["spouseBusinessAddress"];
                
                emp.GrossSalary = Convert.ToInt32(Request.Form["GrossSalary"]);
                emp.BasicSalary = Convert.ToInt32(Request.Form["BasicSalary"]);
                emp.MedicalAllowance = Convert.ToInt32(Request.Form["MedicalAllowance"]);
                emp.Accomodation = Convert.ToInt32(Request.Form["Accomodation"]);
                emp.ProvidedFund = Convert.ToInt32(Request.Form["ProvidedFund"]);
                emp.Tax = Convert.ToInt32(Request.Form["Tax"]);
                emp.EOP = Convert.ToInt32(Request.Form["EOP"]);
                
                emp.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Teaching Staff").Select(x => x.Id).FirstOrDefault();
                emp.UserId = user.Id;
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(user.Id, "Teacher");
                    
                    db.AspNetEmployees.Add(emp);
                    db.SaveChanges();
                    string Error = "Teacher successfully saved.";
                    return RedirectToAction("TeacherIndex", "AspNetUser", new { Error });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeacherfromFile(RegisterViewModel model)
        {
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["teachers"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var teacherList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    ApplicationDbContext context = new ApplicationDbContext();
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var teacher = new RegisterViewModel();
                        teacher.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        teacher.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        teacher.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        teacher.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                        teacher.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();
                        string number = workSheet.Cells[rowIterator, 14].Value.ToString();
                        var user = new ApplicationUser { UserName = teacher.UserName, Email = teacher.Email, Name = teacher.Name, PhoneNumber = number };
                        var result = await UserManager.CreateAsync(user, teacher.Password);
                        if (result.Succeeded)
                        {
                            AspNetEmployee teacherDetail = new AspNetEmployee();
                            teacherDetail.Name = teacher.Name;
                            teacherDetail.Email = teacher.Email;
                            teacherDetail.UserName = teacher.UserName;
                            teacherDetail.UserId = user.Id;
                            teacherDetail.PositionAppliedFor = workSheet.Cells[rowIterator, 6].Value.ToString();
                            teacherDetail.DateAvailable = workSheet.Cells[rowIterator, 7].Value.ToString();
                            teacherDetail.JoiningDate = workSheet.Cells[rowIterator, 8].Value.ToString();
                            teacherDetail.BirthDate = workSheet.Cells[rowIterator, 9].Value.ToString();
                            teacherDetail.Nationality = workSheet.Cells[rowIterator, 10].Value.ToString();
                            teacherDetail.Religion = workSheet.Cells[rowIterator, 11].Value.ToString();
                            teacherDetail.Gender = workSheet.Cells[rowIterator, 12].Value.ToString(); ;
                            teacherDetail.MailingAddress = workSheet.Cells[rowIterator, 13].Value.ToString();
                            teacherDetail.CellNo = workSheet.Cells[rowIterator, 14].Value.ToString();
                            teacherDetail.Landline = workSheet.Cells[rowIterator, 15].Value.ToString();
                            teacherDetail.SpouseName = workSheet.Cells[rowIterator, 16].Value.ToString();
                            teacherDetail.SpouseHighestDegree = workSheet.Cells[rowIterator, 17].Value.ToString();
                            teacherDetail.SpouseOccupation = workSheet.Cells[rowIterator, 18].Value.ToString();
                            teacherDetail.SpouseBusinessAddress = workSheet.Cells[rowIterator, 19].Value.ToString();
                            teacherDetail.Illness = workSheet.Cells[rowIterator, 20].Value.ToString();
                            teacherDetail.GrossSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 21].Value.ToString());
                            teacherDetail.BasicSalary = Convert.ToInt32(workSheet.Cells[rowIterator, 22].Value.ToString());
                            teacherDetail.MedicalAllowance = Convert.ToInt32(workSheet.Cells[rowIterator, 23].Value.ToString());
                            teacherDetail.Accomodation = Convert.ToInt32(workSheet.Cells[rowIterator, 24].Value.ToString());
                            teacherDetail.ProvidedFund = Convert.ToInt32(workSheet.Cells[rowIterator, 25].Value.ToString());
                            teacherDetail.Tax = Convert.ToInt32(workSheet.Cells[rowIterator, 26].Value.ToString());
                            teacherDetail.EOP = Convert.ToInt32(workSheet.Cells[rowIterator, 27].Value.ToString());
                            teacherDetail.VirtualRoleId = db.AspNetVirtualRoles.Where(x => x.Name == "Teaching Staff").Select(x => x.Id).FirstOrDefault();
                            db.AspNetEmployees.Add(teacherDetail);
                            db.SaveChanges();

                            var roleStore = new RoleStore<IdentityRole>(context);
                            var roleManager = new RoleManager<IdentityRole>(roleStore);
                            var userStore = new UserStore<ApplicationUser>(context);
                            var userManager = new UserManager<ApplicationUser>(userStore);
                            userManager.AddToRole(user.Id, "Teacher");
                        }
                        else
                        {
                            dbTransaction.Dispose();
                            AddErrors(result);
                            return View("TeacherRegister", model);
                        }

                    }
                    dbTransaction.Commit();
                }
            }
            catch (Exception e)
            {
             //   ModelState.AddModelError("Error", e.InnerException);
                dbTransaction.Dispose();
                return View("TeacherRegister", model);
               
            }
            return RedirectToAction("TeachersIndex", "AspNetUser");
        }



        /*******************************************************************************************************************
         * 
         *                                    Student's Functions
         *                                    
         *******************************************************************************************************************/
        public ActionResult StudentRegister()
        {
            //var data = db.AspNetClasses 
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentRegister(RegisterViewModel model)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
               
                if (ModelState.IsValid)
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    IEnumerable<string> selectedsubjects = Request.Form["subjects"].Split(',');
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name, PhoneNumber = Request.Form["cellNo"] };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        AspNetStudent student = new AspNetStudent();
                        student.StudentID = user.Id;
                        student.SchoolName = Request.Form["SchoolName"];
                        student.BirthDate = Request.Form["BirthDate"];
                        student.Nationality = Request.Form["Nationality"];
                        student.Religion = Request.Form["Religion"];
                        student.Gender = Request.Form["Gender"];
                        student.ClassID = Convert.ToInt32(Request.Form["ClassID"]);
                        db.AspNetStudents.Add(student);
                        db.SaveChanges();

                        foreach (var item in selectedsubjects)
                        {
                            AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                            stu_sub.StudentID = user.Id;
                            stu_sub.SubjectID = Convert.ToInt32(item);
                            db.AspNetStudent_Subject.Add(stu_sub);
                            db.SaveChanges();
                        }

                        var roleStore = new RoleStore<IdentityRole>(context);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);

                        var userStore = new UserStore<ApplicationUser>(context);
                        var userManager = new UserManager<ApplicationUser>(userStore);
                        userManager.AddToRole(user.Id, "Student");

                        dbTransaction.Commit();
                        string Error = "Student successfully saved.";
                        return RedirectToAction("StudentIndex", "AspNetUser", new { Error } );
                    }
                    else
                    {
                        dbTransaction.Dispose();
                        AddErrors(result);
                    }
                }
            }
            catch (Exception e)
            {
                dbTransaction.Dispose();
                ModelState.AddModelError("", e.Message);
            }
             ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            return View(model);
        }


          

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentfromFile(RegisterViewModel model)
        {
            ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            // if (ModelState.IsValid)
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["students"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                }
                var studentList = new List<RegisterViewModel>();
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var student = new RegisterViewModel();
                        student.Email = workSheet.Cells[rowIterator, 1].Value.ToString();
                        student.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                        student.UserName = workSheet.Cells[rowIterator, 3].Value.ToString();
                        student.Password = workSheet.Cells[rowIterator, 4].Value.ToString();
                        student.ConfirmPassword = workSheet.Cells[rowIterator, 5].Value.ToString();

                        ApplicationDbContext context = new ApplicationDbContext();
                        var user = new ApplicationUser { UserName = student.UserName, Email = student.Email, Name = student.Name };
                        var result = await UserManager.CreateAsync(user, student.Password);
                        if (result.Succeeded)
                        {
                            var subjects = new List<string>();
                            var Class = workSheet.Cells[rowIterator, 6].Value.ToString();
                            subjects.Add(workSheet.Cells[rowIterator, 7].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 8].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 9].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 10].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 11].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 12].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 13].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 14].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 15].Value.ToString());
                            subjects.Add(workSheet.Cells[rowIterator, 16].Value.ToString());

                            var subjectIDs = (from subject in db.AspNetSubjects
                                     join Classes in db.AspNetClasses on subject.ClassID equals Classes.Id
                                     where Classes.ClassName == Class && subjects.Contains(subject.SubjectName)
                                     select subject).ToList();

                            foreach (var subjectid in subjectIDs)
                            {
                                AspNetStudent_Subject stu_sub = new AspNetStudent_Subject();
                                stu_sub.StudentID = user.Id;
                                stu_sub.SubjectID = subjectid.Id;
                                db.AspNetStudent_Subject.Add(stu_sub);
                                db.SaveChanges();
                            }

                            AspNetStudent studentDetail = new AspNetStudent();
                            studentDetail.StudentID = user.Id;
                            studentDetail.SchoolName= workSheet.Cells[rowIterator, 17].Value.ToString();
                            studentDetail.BirthDate= workSheet.Cells[rowIterator, 18].Value.ToString();
                            studentDetail.Nationality= workSheet.Cells[rowIterator, 19].Value.ToString();
                            studentDetail.Religion= workSheet.Cells[rowIterator, 20].Value.ToString();
                            studentDetail.Gender= workSheet.Cells[rowIterator, 21].Value.ToString();
                            studentDetail.ClassID = db.AspNetClasses.Where(x => x.ClassName == Class).Select(x => x.Id).FirstOrDefault();
                            db.AspNetStudents.Add(studentDetail);
                            db.SaveChanges();

                            var roleStore = new RoleStore<IdentityRole>(context);
                            var roleManager = new RoleManager<IdentityRole>(roleStore);
                            var userStore = new UserStore<ApplicationUser>(context);
                            var userManager = new UserManager<ApplicationUser>(userStore);
                            userManager.AddToRole(user.Id, "Student");

                        }
                        else
                        {
                            dbTransaction.Dispose();
                            AddErrors(result);
                            return View("StudentRegister", model);
                        }
                    }
                    dbTransaction.Commit();
                }  
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.InnerException);
                dbTransaction.Dispose();
                return View("StudentRegister", model);
            }
            return RedirectToAction("StudentsIndex", "AspNetUser");
        }

        /**********************************************************************************************************************************/


        [HttpGet]
        public JsonResult SubjectsByClass(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AspNetSubject> sub = db.AspNetSubjects.Where(r => r.ClassID == id).OrderByDescending(r => r.Id).ToList();
            ViewBag.Subjects = sub;
            return Json(sub, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult StudentsByClassMethod(int id)
        {
            string ClassHead = db.AspNetClasses.Where(x => x.Id == id).Select(x => x.TeacherID).First();
            string currentTeacher = User.Identity.GetUserId();

            // if(String.Compare( ClassHead , currentTeacher) == 0)

            var students = (from student in db.AspNetUsers
                            join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                            join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                            where subject.ClassID == id
                            select new { student.Id, student.UserName, student.Name }).Distinct().ToList();

            return Json(students, JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public JsonResult StudentsByClass(string[] bdoIds)
        {
            try
            {
                List<int?> ids = new List<int?>();
                foreach (var item in bdoIds)
                {
                    int a = Convert.ToInt32(item);
                    ids.Add(a);
                }

                var aIDs = db.AspNetParent_Child.AsEnumerable().Select(r => r.ChildID);

                var students = (from student in db.AspNetUsers.AsEnumerable()
                                join student_subject in db.AspNetStudent_Subject on student.Id equals student_subject.StudentID
                                join subject in db.AspNetSubjects on student_subject.SubjectID equals subject.Id
                                where ids.Contains(subject.ClassID)
                                orderby subject.ClassID ascending
                                select new { student.Id, student.Name, student.UserName } ).Distinct().OrderBy(x=> x.Name).ToList();

               // var diff = aIDs.Except(students);
               

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

       

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion



    }

}