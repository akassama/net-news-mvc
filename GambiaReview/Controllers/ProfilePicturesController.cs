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
    public class ProfilePicturesController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: ProfilePictures
        public ActionResult Index()
        {
            return View(db.ProfilePictures.ToList());
        }

        // GET: ProfilePictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilePicturesModel profilePicturesModel = db.ProfilePictures.Find(id);
            if (profilePicturesModel == null)
            {
                return HttpNotFound();
            }
            return View(profilePicturesModel);
        }

        // GET: ProfilePictures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfilePictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserEmail,ProfilePicture,DateAdded")] ProfilePicturesModel profilePicturesModel)
        {
            if (ModelState.IsValid)
            {
                db.ProfilePictures.Add(profilePicturesModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profilePicturesModel);
        }

        // GET: ProfilePictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilePicturesModel profilePicturesModel = db.ProfilePictures.Find(id);
            if (profilePicturesModel == null)
            {
                return HttpNotFound();
            }
            return View(profilePicturesModel);
        }

        // POST: ProfilePictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserEmail,ProfilePicture,DateAdded")] ProfilePicturesModel profilePicturesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profilePicturesModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profilePicturesModel);
        }

        // GET: ProfilePictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilePicturesModel profilePicturesModel = db.ProfilePictures.Find(id);
            if (profilePicturesModel == null)
            {
                return HttpNotFound();
            }
            return View(profilePicturesModel);
        }

        // POST: ProfilePictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProfilePicturesModel profilePicturesModel = db.ProfilePictures.Find(id);
            db.ProfilePictures.Remove(profilePicturesModel);
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
