using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DEA.Models;

namespace DEA.Controllers
{
    public class StudentsController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Students
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Where(x => x.RoleID == 4 && x.Status == true);
            return View(await users.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.Where(x => x.UserID == id).SingleOrDefaultAsync();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.maxUserID = (db.Users.Max(x => x.UserID)) + 1;
            }
            catch
            {
                ViewBag.maxUserID = 1;
            }
            try
            {
                ViewBag.maxStudentID = (db.Students.Max(x => x.StudentID)) + 1;
            }
            catch
            {
                ViewBag.maxStudentID = 1;
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            ViewBag.ParentID = new SelectList(db.Parents, "ParentID", "FatherName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user,student,ClassID,ParentID")] UserStudent us)
        {
            if (ModelState.IsValid)
            {
                //putting RoleID of Student in User
                var role = db.Roles.Where(x => x.RoleName == "Student").FirstOrDefault();
                us.user.RoleID = role.RoleID;
                //
                us.student.UserID = us.user.UserID;
                us.student.ClassID = us.ClassID;
                us.student.ParentID = us.ParentID;
                db.Users.Add(us.user);
                db.Students.Add(us.student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", us.student.ClassID);
            ViewBag.ParentID = new SelectList(db.Parents, "ParentID", "FatherName", us.student.ParentID);
            return View(us);
        }

        

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Student student = await db.Students.Where(x => x.UserID == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }
            UserStudent us = new UserStudent();
            us.user = user;
            us.student = student;
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", us.student.ClassID);
            ViewBag.ParentID = new SelectList(db.Parents, "ParentID", "FatherName", us.student.ParentID);
            return View(us);
        }
        
        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "user,student,ClassID, ParentID")] UserStudent us)
        {
            if (ModelState.IsValid)
            {
                //putting RoleID of student in User
                var role = db.Roles.Where(x => x.RoleName == "Student").FirstOrDefault();
                us.user.RoleID = role.RoleID;
                us.student.UserID = us.user.UserID;
                db.Entry(us.student).State = EntityState.Modified;
                db.Entry(us.user).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName", us.student.ClassID);
            ViewBag.ParentID = new SelectList(db.Parents, "ParentID", "FatherName", us.student.ParentID);
            return View(us);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Student student = await db.Students.SingleOrDefaultAsync(x => x.UserID == id);
            UserStudent us = new UserStudent();
            us.user = user;
            us.student = student;
            if (user == null && student == null)
            {
                return HttpNotFound();
            }
            return View(us);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            user.Status = false;
            db.Entry(user).State = EntityState.Modified;

            //db.Users.Remove(user);
            await db.SaveChangesAsync();
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
    }
}
