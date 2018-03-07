using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DEA.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace DEA
{
    public class AspNetStudentPerformanceReportController : Controller
    {
        private DEA_DBEntities db = new DEA_DBEntities();
        private string TeacherID;
        public AspNetStudentPerformanceReportController()
        {

            TeacherID = Convert.ToString(System.Web.HttpContext.Current.Session["TeacherID"]);
        }
        // GET: AspNetStudentPerformanceReport
        public ActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                var TeacherId = User.Identity.GetUserId();
                ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x=> x.TeacherID == TeacherId), "Id", "ClassName");
                var aspNetStudentPerformanceReports = db.AspNetStudentPerformanceReports.Where(x => x.AspNetSubject.AspNetClass.TeacherID == TeacherID).Select(x => x).ToList();
                return View(aspNetStudentPerformanceReports);
            }
            else if(User.IsInRole("Principal"))
            {
                var TeacherId = User.Identity.GetUserId();
                ViewBag.ClassID = new SelectList(db.AspNetClasses.Where(x => x.TeacherID == TeacherId), "Id", "ClassName");
                var aspNetStudentPerformanceReports = db.AspNetStudentPerformanceReports.Where(x => x.AspNetSubject.AspNetClass.TeacherID == TeacherID).Select(x => x).ToList();
                return View(aspNetStudentPerformanceReports);
            }
            else
            {
                var aspNetStudentPerformanceReports = db.AspNetStudentPerformanceReports.Select(x => x).ToList();
                return View(aspNetStudentPerformanceReports);
            }
        }

        // GET: AspNetStudentPerformanceReport/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudentPerformanceReport aspNetStudentPerformanceReport = db.AspNetStudentPerformanceReports.Find(id);
            if (aspNetStudentPerformanceReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetStudentPerformanceReport.SubjectID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetStudentPerformanceReport.StudentID);
            return View(aspNetStudentPerformanceReport);
        }

        // GET: AspNetStudentPerformanceReport/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.AspNetSubjects.Where(x => x.TeacherID == TeacherID).Select(x => x.AspNetClass).Distinct(), "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName");
            ViewBag.StudentID = new SelectList(db.AspNetStudents.Where(x=> x.AspNetUser.Status != "False"), "Id", "AspNetUser.Name");

            return View();
        }

        // POST: AspNetStudentPerformanceReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentID,SubjectID,Title,Description,LearningConcept,TimeManagement,Homework,ReadingSkills,InstructionFollowing,ProjectLines,Handwriting,PerformanceSkills,Punctuality,Regularity,Assessment")] AspNetStudentPerformanceReport aspNetStudentPerformanceReport)
        {
            if (ModelState.IsValid)
            {
                db.AspNetStudentPerformanceReports.Add(aspNetStudentPerformanceReport);
                aspNetStudentPerformanceReport.Date = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

             ViewBag.ClassID = new SelectList(db.AspNetClasses, "Id", "ClassName");
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetStudentPerformanceReport.SubjectID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudentPerformanceReport.StudentID);
            return View(aspNetStudentPerformanceReport);
        }

        // GET: AspNetStudentPerformanceReport/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudentPerformanceReport aspNetStudentPerformanceReport = db.AspNetStudentPerformanceReports.Find(id);
            if (aspNetStudentPerformanceReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetStudentPerformanceReport.SubjectID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Name", aspNetStudentPerformanceReport.StudentID);
            return View(aspNetStudentPerformanceReport);
        }

        // POST: AspNetStudentPerformanceReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentID,SubjectID,Title,Description,LearningConcept,TimeManagement,Homework,ReadingSkills,InstructionFollowing,ProjectLines,Handwriting,PerformanceSkills,Punctuality,Regularity,Assessment")] AspNetStudentPerformanceReport aspNetStudentPerformanceReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetStudentPerformanceReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectID = new SelectList(db.AspNetSubjects, "Id", "SubjectName", aspNetStudentPerformanceReport.SubjectID);
            ViewBag.StudentID = new SelectList(db.AspNetUsers, "Id", "Email", aspNetStudentPerformanceReport.StudentID);
            return View(aspNetStudentPerformanceReport);
        }

        // GET: AspNetStudentPerformanceReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetStudentPerformanceReport aspNetStudentPerformanceReport = db.AspNetStudentPerformanceReports.Find(id);
            if (aspNetStudentPerformanceReport == null)
            {
                return HttpNotFound();
            }
            return View(aspNetStudentPerformanceReport);
        }

        // POST: AspNetStudentPerformanceReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetStudentPerformanceReport aspNetStudentPerformanceReport = db.AspNetStudentPerformanceReports.Find(id);
            db.AspNetStudentPerformanceReports.Remove(aspNetStudentPerformanceReport);
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

        public JsonResult StudentBySubject(int id)
        {
            var students = (from student_subject in db.AspNetStudent_Subject
                            where student_subject.SubjectID == id && student_subject.AspNetUser.Status != "False"
                            select new { student_subject.AspNetUser.Id, student_subject.AspNetUser.Name }).ToList();
            return Json(students, JsonRequestBehavior.AllowGet);
        }


        public JsonResult PerformanceBySubject(int subjectID)
        {
            
            var TeacherId = User.Identity.GetUserId();
            var PerformanceReports = db.AspNetStudentPerformanceReports.Where(x => x.AspNetSubject.TeacherID == TeacherId && x.AspNetSubject.Id == subjectID).Select(x => new {
                x.AspNetSubject.SubjectName,
                StudentName = x.AspNetUser.Name,
                TeacherName = x.AspNetSubject.AspNetClass.AspNetUser.Name,
                x.AspNetSubject.AspNetClass.ClassName,
                x.Id,
                x.Date
            }).ToList();
            return Json(PerformanceReports, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubjectsProgerssByClass(int ClassID)
        {
            subjectPerfomance subjectPerfomance = new subjectPerfomance();
            subjectPerfomance.Progress = new List<Progress>();
            subjectPerfomance.Subjects = new List<Subjects>();

            var subjects = db.AspNetSubjects.Where(x => x.ClassID == ClassID).ToList();

            foreach (var item in subjects)
            {
                Subjects Subjects = new Subjects();
                Subjects.Id = item.Id;
                Subjects.Subjectname = item.SubjectName;
                subjectPerfomance.Subjects.Add(Subjects);
            }

            var TeacherId = User.Identity.GetUserId();
            var PerformanceReports = db.AspNetStudentPerformanceReports.Where(x => x.AspNetSubject.TeacherID == TeacherID && x.AspNetSubject.ClassID == ClassID).Select(x => x).ToList();

            foreach (var item in PerformanceReports)
            {
                Progress Progress = new Progress();
                Progress.ClassName = item.AspNetSubject.AspNetClass.ClassName;
                Progress.StudentName = item.AspNetUser.Name;
                Progress.TeacherName = item.AspNetSubject.AspNetClass.AspNetUser.Name;
                Progress.Id = item.Id;
                Progress.SubjectName = item.AspNetSubject.SubjectName;
                Progress.Date = (DateTime)item.Date;
                subjectPerfomance.Progress.Add(Progress);
            }

            return Json(subjectPerfomance, JsonRequestBehavior.AllowGet);
        }

        public class subjectPerfomance
        {
            public List<Subjects> Subjects { set; get; }
            public List<Progress> Progress { set; get; }
        }

        public class Subjects
        {
            public int Id { set; get; }
            public string Subjectname { set; get; }
        }

        public class Progress
        {
            public int Id { set; get; }
            public string ClassName { set; get; }
            public string TeacherName { set; get; }
            public string StudentName { set; get; }
            public string SubjectName { set; get; }
            public DateTime Date { set; get; }
        }
    }
}
