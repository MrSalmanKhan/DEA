using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DEA.Models;
using OfficeOpenXml;
using System.Security.Principal;

namespace DEA
{
    [Authorize(Roles = "Admin,Principal")]
    public class AspNetClassController : Controller
    {
        private DEA_DBEntities db = new DEA_DBEntities();

        // GET: AspNetClass
        public ActionResult Index()
        {
            var aspNetClasses = db.AspNetClasses.Include(a => a.AspNetUser);
            return View(aspNetClasses.ToList());
        }
        public void ClassExcelRecord()
        {
            DEA_DBEntities db = new DEA_DBEntities();
            List<AspNetClass> ClassList;

            ClassList = db.AspNetClasses.Select(x => x).ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Class Name";
            ws.Cells["B1"].Value = "Teacher";
            ws.Cells["C1"].Value = "Class";
            ws.Cells["D1"].Value = "Section";


            int rowStart = 2;
            foreach (var item in ClassList)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ClassName;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.AspNetUser.UserName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Class;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Section;
                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment:filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            //return RedirectToAction("TeacherIndex");
        }


        // GET: AspNetClass/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            AspNetClass aspNetClass = db.AspNetClasses.Where(x => x.Id==id).Select(x => x).FirstOrDefault();
            if (aspNetClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class = new SelectList(Classes, "option", "value", aspNetClass.Class);
            ViewBag.Section = new SelectList(Sections, "option", "value", aspNetClass.Section);
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetClass.TeacherID);
            return View(aspNetClass);
        }

        public JsonResult ClassExist(string ClassName)
        {
            int count = 0;

            try
            {
                var check = db.AspNetClasses.Where(x => x.ClassName == ClassName).ToList();

                if (check.Count > 0)
                {
                    count = 1;
                } else
                    count = 0;
            }
            catch
            {
                count = 0;
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        // GET: AspNetClass/Create
        public ActionResult Create()
        {
            ViewBag.Class = new SelectList(Classes, "option", "value");
            ViewBag.Section = new SelectList(Sections, "option", "value");
          
            ViewBag.TeacherID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Teacher") && x.Status!="False"), "Id", "Name");
            return View();
        }

        // POST: AspNetClass/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassName,Class,Section,TeacherID")] AspNetClass aspNetClass)
        {

            var transactionObj = db.Database.BeginTransaction();
            try
            { 
            if (ModelState.IsValid)
            {
                db.AspNetClasses.Add(aspNetClass);
                db.SaveChanges();
            }
                transactionObj.Commit();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var UserNameLog = User.Identity.Name;
                AspNetUser userObjLog = db.AspNetUsers.First(x => x.UserName == UserNameLog);
                string UserIDLog = userObjLog.Id;
                AspNetUser ClassTeacherLog = db.AspNetUsers.First(x => x.Id == aspNetClass.TeacherID);
                string ClassTeacherNameLog = ClassTeacherLog.Name;
                string user = db.AspNetUsers.Where(x => x.Id == aspNetClass.TeacherID).Select(x => x.Name).FirstOrDefault();
                var logMessage = "A new Class Added, Name: " + aspNetClass.ClassName + ", Class Teacher: " + ClassTeacherNameLog + ", Teacher Name: " + user;

                var LogControllerObj = new AspNetLogsController();
                LogControllerObj.CreateLogSave(logMessage, UserIDLog);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                transactionObj.Dispose();
            }

            ViewBag.Class = new SelectList(Classes, "option", "value");
            ViewBag.Section = new SelectList(Sections, "option", "value");
            ViewBag.TeacherID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Teacher")), "Id", "UserName", aspNetClass.TeacherID);
            return View(aspNetClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClassfromFile(AspNetClass aspNetClass)
        {
            var dbTransaction = db.Database.BeginTransaction();
            try
            {
                HttpPostedFileBase file = Request.Files["classes"];
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
                        var Class = new AspNetClass();
                        string teacherName = workSheet.Cells[rowIterator, 2].Value.ToString();
                        var Teacher = (from users in db.AspNetUsers
                                       where users.UserName == teacherName
                                       select users).First();
                        Class.ClassName = workSheet.Cells[rowIterator, 1].Value.ToString();
                        Class.Class= workSheet.Cells[rowIterator, 3].Value.ToString();
                        Class.Section=workSheet.Cells[rowIterator, 4].Value.ToString();
                        if (Class.Section=="-")
                        {
                            Class.Section = "";
                        }
                        Class.TeacherID = Teacher.Id;
                        db.AspNetClasses.Add(Class);
                        db.SaveChanges();

                    }
                }
                dbTransaction.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception) { dbTransaction.Dispose(); }

            // ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetClass.TeacherID);
            return RedirectToAction("Create", aspNetClass);
        }

        public class selectclass
        {
            public selectclass(string value,string option)
            {
                this.value = value;
                this.option = option;
            }
            public string value { get; set; }
            public string option { get; set; }
        }

        public class selectSection
        {
            public selectSection (string value,string option)
            {
                this.value = value;
                this.option = option;
            }
            public string value { get; set; }
            public string option { get; set; }
        }
        
        List<selectclass> Classes = new List<selectclass> {
            new selectclass("PREPARATORY", "PREPARATORY"),
            new selectclass("PLAYGROUP", "PLAYGROUP"),
            new selectclass("CRE CHE SUPERVISOR", "CRE CHE SUPERVISOR"),
            new selectclass("RECEPTION CRASH", "RECEPTION CRASH"),
            new selectclass("RECEPTION", "RECEPTION"),
            new selectclass("GRADE", "GRADE"),

        };

        List<selectSection> Sections = new List<selectSection>
        {
             new selectSection("", ""),
             new selectSection("A", "A"),
            new selectSection("B", "B"),
            new selectSection("C", "C"),
            new selectSection("1", "1"),
            new selectSection("2", "2"),
            new selectSection("3", "3"),

        };

        // GET: AspNetClass/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetClass aspNetClass = db.AspNetClasses.Find(id);
            if (aspNetClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class = new SelectList(Classes, "option", "value",aspNetClass.Class);
            ViewBag.Section = new SelectList(Sections, "option", "value", aspNetClass.Section);
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetClass.TeacherID);
            return View(aspNetClass);
        }

        // POST: AspNetClass/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassName,Class,Section,TeacherID")] AspNetClass aspNetClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Class = new SelectList(Classes, "option", "value", aspNetClass.Class);
            ViewBag.Section = new SelectList(Sections, "option", "value", aspNetClass.Section);
            ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetClass.TeacherID);
            return View(aspNetClass);
        }

        // GET: AspNetClass/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetClass aspNetClass = db.AspNetClasses.Find(id);
            if (aspNetClass == null)
            {
                return HttpNotFound();
            }
            return View(aspNetClass);
        }

        // POST: AspNetClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AspNetClass aspNetClass = db.AspNetClasses.Find(id);
                db.AspNetClasses.Remove(aspNetClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "This Class has subjects and students in it. It Can't be deleted";
                return View("Delete", db.AspNetClasses.Find(id));
            }
            
        }
        public ActionResult DeleteClass(int id)
        {
            var clas = db.AspNetClasses.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            try
            {
                db.AspNetClasses.Remove(clas);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.TeacherID = new SelectList(db.AspNetUsers, "Id", "Name", clas.TeacherID);
                ViewBag.Error = "This Class can't be deleted. It contains subjects";
                return View("Details", clas);
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
