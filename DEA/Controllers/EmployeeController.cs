﻿using System;
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
    public class EmployeeController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Where(u => u.RoleID != 2 && u.RoleID != 3 && u.Status == true);
            return View(await users.ToListAsync());
        }
        //{
        //    var employees = db.Employees.Include(e => e.Role).Include(e => e.User).Except(e =>;
        //    return View(await employees.ToListAsync());
        //}

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = await db.Employees.SingleOrDefaultAsync(x => x.UserID == id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // GET: Employee/Create
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
                ViewBag.maxEmployeeID = (db.Employees.Max(x => x.EmployeeID)) + 1;
            }
            catch
            {
                ViewBag.maxEmployeeID = 1;
            }
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID != 2 && x.RoleID != 3), "RoleID", "RoleName");
            ViewBag.RoleIDemp = new SelectList(db.Roles.Where(x => x.RoleID == 7), "RoleID", "RoleName");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user,employee,RoleID")] UserEmployee ue)
        {
            if (ModelState.IsValid)
            {
                //putting RoleIDs except of accountant and teacher in User
                var role = db.Roles.Where(x => x.RoleName == "Employee").FirstOrDefault();
                ue.user.RoleID = role.RoleID;
                //
                ue.employee.RoleID = ue.RoleID;
                ue.employee.UserID = ue.user.UserID;
                db.Users.Add(ue.user);
                db.Employees.Add(ue.employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID != 2 && x.RoleID != 3), "RoleID", "RoleName", ue.employee.RoleID);
            ViewBag.RoleIDemp = new SelectList(db.Roles.Where(x => x.RoleID == 7), "RoleID", "RoleName", ue.user.RoleID);
            return View(ue);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Employee emp = await db.Employees.Where(x => x.UserID == id).FirstOrDefaultAsync();
            if (user == null && emp == null)
            {
                return HttpNotFound();
            }
            UserEmployee ue = new UserEmployee();
            ue.user = user;
            ue.employee = emp;
            ue.RoleID = emp.RoleID;
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID != 2 && x.RoleID != 3), "RoleID", "RoleName", ue.employee.RoleID);
            ViewBag.RoleIDemp = new SelectList(db.Roles.Where(x => x.RoleID == 7), "RoleID", "RoleName", ue.user.RoleID);
            return View(ue);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "user,employee,RoleID")] UserEmployee ue)
        {
            if (ModelState.IsValid)
            {
                //putting RoleIDs except of accountant and teacher in User
                var role = db.Roles.Where(x => x.RoleName == "Employee").FirstOrDefault();
                ue.user.RoleID = role.RoleID;
                //
                ue.employee.RoleID = ue.RoleID;
                ue.employee.UserID = ue.user.UserID;
                db.Entry(ue.employee).State = EntityState.Modified;
                db.Entry(ue.user).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID != 2 && x.RoleID != 3), "RoleID", "RoleName", ue.employee.RoleID);
            ViewBag.RoleIDemp = new SelectList(db.Roles.Where(x => x.RoleID == 7), "RoleID", "RoleName", ue.user.RoleID);
            return View(ue);
        }

        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Employee emp = await db.Employees.SingleOrDefaultAsync(x => x.UserID == id);
            UserEmployee ue = new UserEmployee();
            ue.user = user;
            ue.employee = emp;
            if (user == null && emp == null)
            {
                return HttpNotFound();
            }
            return View(ue);
        }

        // POST: Employee/Delete/5
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
