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
    public class ClassController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Class
        public async Task<ActionResult> ClassIndex()
        {
            return View(await db.Classes.ToListAsync());
        }

        // GET: Class/Details/5
        public async Task<ActionResult> ClassDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Class/Create
        public ActionResult CreateClass()
        {
            var id = db.Classes.Max(x => x.ClassID);
            id++;
            ViewBag.id = id;
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateClass([Bind(Include = "ClassID,Class1,Section,ClassName")] Class @class)
        {
            if (ModelState.IsValid)
            {
                int id = db.Classes.Max(x => x.ClassID);
                id++;
                @class.ClassID = id;
                db.Classes.Add(@class);
                await db.SaveChangesAsync();
                return RedirectToAction("ClassIndex");
            }

            return View(@class);
        }

        // GET: Class/Edit/5
        public async Task<ActionResult> EditClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditClass([Bind(Include = "ClassID,Class1,Section,ClassName")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ClassIndex");
            }
            return View(@class);
        }

        // GET: Class/Delete/5
        public async Task<ActionResult> DeleteClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }


        // POST: Class/Delete/5
        [HttpPost, ActionName("DeleteClass")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Class @class = await db.Classes.FindAsync(id);
            db.Classes.Remove(@class);
            await db.SaveChangesAsync();
            return RedirectToAction("ClassIndex");
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
