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
    public class AfricaController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Africa
        public ActionResult Index(int page = 0)
        {
            string category = "Africa";
            ViewBag.Category = category;
            int category_id = AppFunctions.GetCategoryIDFromText(category);
            var dataSource = db.NewsArticles.Where(s => s.ArticleCategory == category_id && s.ReviewStatus == 1 && s.DeleteStatus == false).OrderByDescending(s => s.ID);

            const int PageSize = 6; // you can always do something more elegant to set this

            var count = dataSource.Count();

            var data = dataSource.Skip(page * PageSize).Take(PageSize).ToList();

            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            ViewBag.Page = page;

            return View(data);
        }

        // GET: Africa/News/5
        public ActionResult News(string id)
        {
            string category = "Africa";
            ViewBag.Category = category;

            ViewBag.HasMedia = AppFunctions.ArticleHasMediaFile(AppFunctions.GetArticleIdFromIdentifier(id));
            ViewBag.ArticleMediaFileName =  AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileName");
            ViewBag.ArticleMediaFileCaption = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileCaption");
            ViewBag.ArticleMediaFileType = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(id), "FileType");

            string text_id = id;
            if (String.IsNullOrEmpty(text_id))
            {
                return PartialView("~/Views/Home/_404.cshtml");
            }
            int article_id = AppFunctions.GetNewsArticleIDFromTextID(text_id);
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(article_id);
            if (newsArticlesModel == null)
            {
                return PartialView("~/Views/Home/_404.cshtml");
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
