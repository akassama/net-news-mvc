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
    public class CategoriesController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriesModel categoriesModel = db.Categories.Find(id);
            if (categoriesModel == null)
            {
                return HttpNotFound();
            }
            return View(categoriesModel);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryName,CategoryText,DateAdded")] CategoriesModel categoriesModel)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(categoriesModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriesModel);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriesModel categoriesModel = db.Categories.Find(id);
            if (categoriesModel == null)
            {
                return HttpNotFound();
            }
            return View(categoriesModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryName,CategoryText,DateAdded")] CategoriesModel categoriesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriesModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriesModel);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriesModel categoriesModel = db.Categories.Find(id);
            if (categoriesModel == null)
            {
                return HttpNotFound();
            }
            return View(categoriesModel);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriesModel categoriesModel = db.Categories.Find(id);
            db.Categories.Remove(categoriesModel);
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
    }
}
