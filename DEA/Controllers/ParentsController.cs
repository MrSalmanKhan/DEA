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
    public class ParentsController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Parents
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Where(x => x.RoleID == 5 && x.Status == true);
            return View(await users.ToListAsync());
        }

        // GET: Parents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent par = await db.Parents.SingleOrDefaultAsync(x => x.UserID == id);
            if (par == null)
            {
                return HttpNotFound();
            }
            return View(par);
        }


        // GET: Parents/Create
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
                ViewBag.maxParentID = (db.Parents.Max(x => x.ParentID)) + 1;
            }
            catch
            {
                ViewBag.maxParentID = 1;
            }
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID == 5), "RoleID", "RoleName");
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user,parent")] UserParent up)
        {
            if (ModelState.IsValid)
            {
                //putting RoleID of parent in User
                var role = db.Roles.Where(x => x.RoleName == "Parent").FirstOrDefault();
                up.user.RoleID = role.RoleID;
                //
                up.parent.UserID = up.user.UserID;
                db.Users.Add(up.user);
                db.Parents.Add(up.parent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID == 5), "RoleID", "RoleName", up.user.RoleID);
            return View(up);
        }

        // GET: Parents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Parent par = await db.Parents.Where(x => x.UserID == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }
            UserParent up = new UserParent();
            up.user = user;
            up.parent= par;
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID == 5), "RoleID", "RoleName", up.user.RoleID);
            return View(up);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "user,parent")] UserParent up)
        {
            if (ModelState.IsValid)
            {
                //putting RoleID of parent in User
                var role = db.Roles.Where(x => x.RoleName == "Parent").FirstOrDefault();
                up.user.RoleID = role.RoleID;
                up.parent.UserID = up.user.UserID;
                db.Entry(up.parent).State = EntityState.Modified;
                db.Entry(up.user).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles.Where(x => x.RoleID == 5), "RoleID", "RoleName", up.user.RoleID);
            return View(up);
        }

        // GET: Parents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            Parent par = await db.Parents.SingleOrDefaultAsync(x => x.UserID == id);
            UserParent up = new UserParent();
            up.user = user;
            up.parent= par;
            if (user == null && par == null)
            {
                return HttpNotFound();
            }
            return View(up);
        }

        // POST: Parents/Delete/5
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
