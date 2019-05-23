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
    public class AccountsModelsController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: AccountsModels
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: AccountsModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountsModel accountsModel = db.Accounts.Find(id);
            if (accountsModel == null)
            {
                return HttpNotFound();
            }
            return View(accountsModel);
        }

        // GET: AccountsModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountsModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Country,CountryCode,PhoneNumber,Email,Password,Salt,Type,DirectoryName,Status,Oauth,AccountVerification,DateCreated")] AccountsModel accountsModel)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(accountsModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accountsModel);
        }

        // GET: AccountsModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountsModel accountsModel = db.Accounts.Find(id);
            if (accountsModel == null)
            {
                return HttpNotFound();
            }
            return View(accountsModel);
        }

        // POST: AccountsModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName,Country,CountryCode,PhoneNumber,Email,Password,Salt,Type,DirectoryName,Status,Oauth,AccountVerification,DateCreated")] AccountsModel accountsModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountsModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountsModel);
        }

        // GET: AccountsModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountsModel accountsModel = db.Accounts.Find(id);
            if (accountsModel == null)
            {
                return HttpNotFound();
            }
            return View(accountsModel);
        }

        // POST: AccountsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountsModel accountsModel = db.Accounts.Find(id);
            db.Accounts.Remove(accountsModel);
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
