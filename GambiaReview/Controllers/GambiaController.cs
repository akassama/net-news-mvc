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
    public class GambiaController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Gambia
        public ActionResult Index(int page = 0)
        {
            string category = "Gambia";
            ViewBag.Category = category;
            int category_id = AppFunctions.GetCategoryIDFromText(category);

            //https://stackoverflow.com/a/27965790/5612132
            var dataSource = db.NewsArticles.Where(s => s.ArticleCategory == category_id && s.ReviewStatus == 1 && s.DeleteStatus == false).OrderByDescending(s => s.ID);

            const int PageSize = 6; // you can always do something more elegant to set this

            var count = dataSource.Count();

            var data = dataSource.Skip(page * PageSize).Take(PageSize).ToList();

            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            ViewBag.Page = page;

            return View(data);
        }


        public ActionResult IndexX()
        {
            string category = "Gambia";
            ViewBag.Category = category;

            int category_id = AppFunctions.GetCategoryIDFromText(category);
            var data = db.NewsArticles.Where(s => s.ArticleCategory == category_id && s.ReviewStatus == 1 && s.DeleteStatus == false).OrderByDescending(s => s.ID);
            ViewBag.total_records = db.NewsArticles.Count(s => s.ArticleCategory == category_id && s.DeleteStatus == false);
            return View(data);
        }

        // GET: Gambia/News/5
        public ActionResult News(string id)
        {
            string category = "Gambia";
            ViewBag.Category = category;

            ViewBag.HasMedia = AppFunctions.ArticleHasMediaFile(AppFunctions.GetArticleIdFromIdentifier(id));
            ViewBag.ArticleMediaFileName = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileName");
            ViewBag.ArticleMediaFileCaption = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileCaption");
            ViewBag.ArticleMediaFileType = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileType");

            string text_id = id;
            if (String.IsNullOrEmpty(text_id))
            {
                return PartialView("~/Views/Home/_404.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int article_id = AppFunctions.GetNewsArticleIDFromTextID(text_id);
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(article_id);
            if (newsArticlesModel == null)
            {
                return PartialView("~/Views/Home/_404.cshtml");
                //return HttpNotFound();
            }

            //Count page visit
            AppFunctions.AddArticlePageVisit(article_id);

            return View(newsArticlesModel);
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
