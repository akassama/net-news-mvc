﻿using System;
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
    public class AdvertsController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Adverts
        public ActionResult Index()
        {
            DateTime todays_date = DateTime.Now;
            var data = db.Adverts.Where(s => s.DeleteStatus == false && s.AdvertExpiryDate >= todays_date).OrderByDescending(x => x.ID);
            return View(data);
        }

        // GET: Adverts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertsModel advertsModel = db.Adverts.Find(id);
            if (advertsModel == null)
            {
                return HttpNotFound();
            }
            return View(advertsModel);
        }

        // GET: Adverts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adverts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AdvertTextIdentifier,AdvertBy,AdvertCategory,AdvertDate,AdvertHeadline,AdvertImage,AdvertBody,DeleteStatus,AdvertExpiryDate")] AdvertsModel advertsModel)
        {
            if (ModelState.IsValid)
            {
                db.Adverts.Add(advertsModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(advertsModel);
        }

        // GET: Adverts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertsModel advertsModel = db.Adverts.Find(id);
            if (advertsModel == null)
            {
                return HttpNotFound();
            }
            return View(advertsModel);
        }

        // POST: Adverts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AdvertTextIdentifier,AdvertBy,AdvertCategory,AdvertDate,AdvertHeadline,AdvertImage,AdvertBody,DeleteStatus,AdvertExpiryDate")] AdvertsModel advertsModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advertsModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(advertsModel);
        }

        // GET: Adverts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertsModel advertsModel = db.Adverts.Find(id);
            if (advertsModel == null)
            {
                return HttpNotFound();
            }
            return View(advertsModel);
        }

        // POST: Adverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertsModel advertsModel = db.Adverts.Find(id);
            db.Adverts.Remove(advertsModel);
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
