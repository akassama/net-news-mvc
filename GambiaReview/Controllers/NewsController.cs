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
    public class NewsController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: News
        public ActionResult Index()
        {
            return View(db.NewsArticles.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(id);
            if (newsArticlesModel == null)
            {
                return HttpNotFound();
            }
            return View(newsArticlesModel);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ArticleTextIdentifier,ArticleBy,ArticleCategory,ArticleDate,ArticleHeadline,ArticleHeadlineImage,HeadlineImageDescription,ArticleBody,DeleteStatus,Tags")] NewsArticlesModel newsArticlesModel)
        {
            if (ModelState.IsValid)
            {
                db.NewsArticles.Add(newsArticlesModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsArticlesModel);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(id);
            if (newsArticlesModel == null)
            {
                return HttpNotFound();
            }
            return View(newsArticlesModel);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ArticleTextIdentifier,ArticleBy,ArticleCategory,ArticleDate,ArticleHeadline,ArticleHeadlineImage,HeadlineImageDescription,ArticleBody,DeleteStatus,Tags")] NewsArticlesModel newsArticlesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsArticlesModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsArticlesModel);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(id);
            if (newsArticlesModel == null)
            {
                return HttpNotFound();
            }
            return View(newsArticlesModel);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(id);
            db.NewsArticles.Remove(newsArticlesModel);
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
