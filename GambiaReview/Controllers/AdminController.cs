using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GambiaReview.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace GambiaReview.Controllers
{
    [SessionReload]
    public class AdminController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Admin
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult Index()
        {
            var admin_email = Session["sessionEmail"];
            ViewBag.TotalPageViews = AppFunctions.GetTotalAdminPostsViews(admin_email.ToString());
            ViewBag.TotalPendingReviews = AppFunctions.GetTotalPendingReviews(admin_email.ToString());
            ViewBag.TotalIncompletePosts = AppFunctions.GetTotalIncompletePosts(admin_email.ToString());

            int year = DateTime.Now.Year;
            //Total Page Views Per Month
            ViewBag.TotalPageViewsJan = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 01, year);
            ViewBag.TotalPageViewsFeb = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 02, year);
            ViewBag.TotalPageViewsMar = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 03, year);
            ViewBag.TotalPageViewsApr = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 04, year);
            ViewBag.TotalPageViewsMay = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 05, year);
            ViewBag.TotalPageViewsJun = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 06, year);
            ViewBag.TotalPageViewsJul = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 07, year);
            ViewBag.TotalPageViewsAug = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 08, year);
            ViewBag.TotalPageViewsSep = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 09, year);
            ViewBag.TotalPageViewsOct = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 10, year);
            ViewBag.TotalPageViewsNov = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 11, year);
            ViewBag.TotalPageViewsDec = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 12, year);

            //Total Posts Per Month
            ViewBag.TotalAdminPostJan = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 01, year);
            ViewBag.TotalAdminPostFeb = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 02, year);
            ViewBag.TotalAdminPostMar = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 03, year);
            ViewBag.TotalAdminPostApr = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 04, year);
            ViewBag.TotalAdminPostMay = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 05, year);
            ViewBag.TotalAdminPostJun = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 06, year);
            ViewBag.TotalAdminPostJul = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 07, year);
            ViewBag.TotalAdminPostAug = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 08, year);
            ViewBag.TotalAdminPostSep = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 09, year);
            ViewBag.TotalAdminPostOct = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 10, year);
            ViewBag.TotalAdminPostNov = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 11, year);
            ViewBag.TotalAdminPostDec = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 12, year);


            return View();
        }


        // GET: Blank
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult Blank()
        {
            return View();

        }

        // GET: Blank
        public ActionResult Auto()
        {
            ViewBag.AllAdminsCsv = AppFunctions.ListToCsv(db.Accounts.Select(x => x.FirstName).ToArray());
            return View();

        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------MANAGE NEWS ARTICLES------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        // GET: ManagePosts
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ManagePosts()
        {
            string admin_email = Session["sessionEmail"].ToString();
            var data = db.NewsArticles.Where(s => s.ArticleBy == admin_email && s.DeleteStatus != true).OrderByDescending(x => x.ID);
            return View(data);
        }


        // GET: Admin/NewPost
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult NewPost()
        {
            var admin_email = Session["sessionEmail"];
            ViewBag.ArticleBy = admin_email.ToString();

            ViewBag.CATEGORIES = GetCategoriesList();
            ViewBag.TAGS = GetTagsList();

            return View();
        }

        // POST: Admin/NewPost
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionAuthorize]
        [SessionHasRole]
        [ValidateAntiForgeryToken]
        public ActionResult NewPost([Bind(Include = "ID,ArticleTextIdentifier,ArticleBy,ArticleCategory,ArticleDate,ArticleHeadline,ArticleHeadlineImage,HeadlineImageDescription,ArticleBody,DeleteStatus,Tags,ReviewedBy,ReviewStatus")] NewsArticlesModel newsArticlesModel, HttpPostedFileBase ArticleHeadlineImage, HttpPostedFileBase ArticleMedia)
        {
            string admin_email = Session["sessionEmail"].ToString();
            ViewBag.ArticleBy = admin_email.ToString();

            var Tags = Request.Form["Tags"];

            if (ModelState.IsValid)
            {
                newsArticlesModel.ArticleTextIdentifier = newsArticlesModel.ArticleHeadline.Replace(' ', '-');
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                newsArticlesModel.ArticleTextIdentifier = rgx.Replace(newsArticlesModel.ArticleTextIdentifier, "");
                //newsArticlesModel.ArticleTextIdentifier = Regex.Replace(newsArticlesModel.ArticleTextIdentifier, @"(\s+|@*|&|'|:|,|\(|\)|<|>|#)", "");
                if (newsArticlesModel.ArticleTextIdentifier.Length > 250)
                {
                    newsArticlesModel.ArticleTextIdentifier = newsArticlesModel.ArticleTextIdentifier.Substring(0, 249);
                }


                //Upload Headline Image Document
                if (ArticleHeadlineImage != null && ArticleHeadlineImage.ContentLength > 0)
                {
                    string folder_directory = Server.MapPath("~/images/articles/") + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory);
                    //Server.MapPath("~/images/articles/" + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory));
                    Directory.CreateDirectory(folder_directory); //No need to check for existing directory as it is ignored if true
                    var Rand = AppFunctions.RandomInt(5);

                    var fullFileName = Path.GetFileName(ArticleHeadlineImage.FileName);
                    if (fullFileName.Length > 90 && fullFileName.Length <= 100)
                    {
                        var trimText = fullFileName.Substring(60, 25);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    else if (fullFileName.Length > 100)
                    {
                        var trimText = fullFileName.Substring(50, 50);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    var fileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                    var fileType = Path.GetExtension(ArticleHeadlineImage.FileName).Substring(1);
                    var updatedFileName = fileNameOnly + "_" + Rand + "_" + DateTime.Now.ToString("yyyyMMdd") + "." + fileType;
                    newsArticlesModel.ArticleHeadlineImage = updatedFileName;
                    var path = Path.Combine(folder_directory, updatedFileName);
                    ArticleHeadlineImage.SaveAs(path);
                }

                //If Media available, Upload Media
                var mediaFileType = "";
                var mediaFileName = "";
                if (ArticleMedia != null && ArticleMedia.ContentLength > 0)
                {
                    string folder_directory = Server.MapPath("~/images/articles/") + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory);
                    Directory.CreateDirectory(folder_directory);
                    var Rand = AppFunctions.RandomInt(5);

                    var fullFileName = Path.GetFileName(ArticleMedia.FileName);
                    if (fullFileName.Length > 90 && fullFileName.Length <= 100)
                    {
                        var trimText = fullFileName.Substring(60, 25);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    else if (fullFileName.Length > 100)
                    {
                        var trimText = fullFileName.Substring(50, 50);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    var fileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                    mediaFileType = Path.GetExtension(ArticleMedia.FileName).Substring(1);
                    mediaFileName = fileNameOnly + "_" + Rand + "_" + DateTime.Now.ToString("yyyyMMdd") + "." + mediaFileType;
                    var path = Path.Combine(folder_directory, mediaFileName);
                    ArticleMedia.SaveAs(path);
                }

                try
                {
                    newsArticlesModel.Tags = Tags;
                    newsArticlesModel.ReviewStatus = 0;

                    db.NewsArticles.Add(newsArticlesModel);
                    db.SaveChanges();

                    //Add Media File
                    if (ArticleMedia != null)
                    {
                        string FileDescription = Request.Form["MediaFileDescription"];
                        if (string.IsNullOrEmpty(FileDescription))
                        {
                            FileDescription = "Media File";
                        }
                        AppFunctions.AddMediaFile(newsArticlesModel.ID, mediaFileType, mediaFileName, FileDescription);
                    }


                    TempData["SuccessMessage"] = "Post added successfully.";
                    return RedirectToAction("ManagePosts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["ErrorMessage"] = "Failed to post, please try again. If you continue to get this error, please contact the Administrator.";
                    //Log Error
                    SecurityFunctions.LogError(ex, admin_email, "NewArticle", null);

                    return View(newsArticlesModel);
                }
            }

            ViewBag.CATEGORIES = GetCategoriesList();
            ViewBag.TAGS = GetTagsList();

            return View(newsArticlesModel);
        }


        // GET: Admin/EditArticle
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult EditArticle(int? id)
        {
            var admin_email = Session["sessionEmail"];
            ViewBag.ArticleBy = admin_email.ToString();

            ViewBag.CATEGORIES = GetCategoriesList();
            ViewBag.TAGS = GetTagsList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticlesModel articlesModel = db.NewsArticles.Find(id);
            if (articlesModel == null)
            {
                return HttpNotFound();
            }
            return View(articlesModel);
        }


        [HttpPost]
        //[SessionReload]
        [SessionAuthorize]
        [SessionHasRole]
        //[HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditArticle([Bind(Include = "ID,ArticleTextIdentifier,ArticleBy,ArticleCategory,ArticleDate,ArticleHeadline,ArticleHeadlineImage,HeadlineImageDescription,ArticleBody,DeleteStatus,Tags,ReviewedBy,ReviewStatus")] NewsArticlesModel newsArticlesModel, HttpPostedFileBase ArticleHeadlineImage, HttpPostedFileBase ArticleMedia)
        {
            string admin_email = Session["sessionEmail"].ToString();
            ViewBag.ArticleBy = admin_email.ToString();
            var Tags = Request.Form["Tags"];

            //Reset other data
            newsArticlesModel.ArticleBy = AppFunctions.GetNewsArticleData("ArticleBy", newsArticlesModel.ID);
            newsArticlesModel.ArticleDate = DateTime.Parse(AppFunctions.GetNewsArticleData("ArticleDate", newsArticlesModel.ID));
            newsArticlesModel.DeleteStatus = Boolean.Parse(AppFunctions.GetNewsArticleData("DeleteStatus", newsArticlesModel.ID));
            newsArticlesModel.ReviewedBy = AppFunctions.GetNewsArticleData("ReviewedBy", newsArticlesModel.ID);
            //newsArticlesModel.ReviewComments = AppFunctions.GetNewsArticleData("ReviewComments", newsArticlesModel.ID);
            newsArticlesModel.ReviewStatus = Int32.Parse(AppFunctions.GetNewsArticleData("ReviewStatus", newsArticlesModel.ID));

            bool model_state = ModelState.IsValid;

            if (model_state)
            {

                if (ArticleHeadlineImage == null || Request.Form["ArticleHeadlineImage"] == "Default")
                {
                    newsArticlesModel.ArticleHeadlineImage = AppFunctions.GetNewsArticleData("ArticleHeadlineImage", newsArticlesModel.ID);
                }

                newsArticlesModel.ArticleTextIdentifier = newsArticlesModel.ArticleHeadline.Replace(' ', '-');
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                newsArticlesModel.ArticleTextIdentifier = rgx.Replace(newsArticlesModel.ArticleTextIdentifier, "");
                //newsArticlesModel.ArticleTextIdentifier = Regex.Replace(newsArticlesModel.ArticleTextIdentifier, @"(\s+|@*|&|'|:|,|\(|\)|<|>|#)", "");
                if (newsArticlesModel.ArticleTextIdentifier.Length > 250)
                {
                    newsArticlesModel.ArticleTextIdentifier = newsArticlesModel.ArticleTextIdentifier.Substring(0, 249);
                }


                //Upload Headline Image Document
                if (ArticleHeadlineImage != null && ArticleHeadlineImage.ContentLength > 0)
                {
                    var old_headline_image = AppFunctions.GetNewsArticleData("ArticleHeadlineImage", newsArticlesModel.ID);

                    string folder_directory = Server.MapPath("~/images/articles/") + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory);
                    //Server.MapPath("~/images/articles/" + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory));
                    Directory.CreateDirectory(folder_directory); //No need to check for existing directory as it is ignored if true
                    var Rand = AppFunctions.RandomInt(5);

                    var fullFileName = Path.GetFileName(ArticleHeadlineImage.FileName);
                    if (fullFileName.Length > 90 && fullFileName.Length <= 100)
                    {
                        var trimText = fullFileName.Substring(60, 25);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    else if (fullFileName.Length > 100)
                    {
                        var trimText = fullFileName.Substring(50, 50);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    var fileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                    var fileType = Path.GetExtension(ArticleHeadlineImage.FileName).Substring(1);
                    var updatedFileName = fileNameOnly + "_" + Rand + "_" + DateTime.Now.ToString("yyyyMMdd") + "." + fileType;
                    newsArticlesModel.ArticleHeadlineImage = updatedFileName;
                    var path = Path.Combine(folder_directory, updatedFileName);
                    ArticleHeadlineImage.SaveAs(path);

                    //Remove Previous Image
                    var delete_path = Path.Combine(folder_directory, old_headline_image);
                    AppFunctions.DeleteFile(delete_path);
                }

                //If Media available, Upload Media
                var mediaFileType = "";
                var mediaFileName = "";
                if (ArticleMedia != null && ArticleMedia.ContentLength > 0)
                {
                    string folder_directory = Server.MapPath("~/images/articles/") + AppFunctions.GetCategoryName(newsArticlesModel.ArticleCategory);
                    Directory.CreateDirectory(folder_directory);
                    var Rand = AppFunctions.RandomInt(5);

                    var fullFileName = Path.GetFileName(ArticleMedia.FileName);
                    if (fullFileName.Length > 90 && fullFileName.Length <= 100)
                    {
                        var trimText = fullFileName.Substring(60, 25);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    else if (fullFileName.Length > 100)
                    {
                        var trimText = fullFileName.Substring(50, 50);
                        fullFileName = fullFileName.Replace(trimText, "");
                    }
                    var fileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                    mediaFileType = Path.GetExtension(ArticleMedia.FileName).Substring(1);
                    mediaFileName = fileNameOnly + "_" + Rand + "_" + DateTime.Now.ToString("yyyyMMdd") + "." + mediaFileType;
                    var path = Path.Combine(folder_directory, mediaFileName);
                    ArticleMedia.SaveAs(path);

                    //Remove Previous Media
                    if (db.ArticleMediaUploads.Any(s => s.ArticleID == newsArticlesModel.ID))
                    {
                        string old_media_file = db.ArticleMediaUploads.Where(s => s.ArticleID == newsArticlesModel.ID).FirstOrDefault().FileName;
                        var delete_path = Path.Combine(folder_directory, old_media_file);
                        AppFunctions.DeleteFile(delete_path);
                    }
                }

                try
                {
                    if (string.IsNullOrEmpty(Tags))
                    {
                        newsArticlesModel.Tags = AppFunctions.GetNewsArticleData("Tags", newsArticlesModel.ID);
                    }
                    else
                    {
                        newsArticlesModel.Tags = Tags;
                    }

                    db.Entry(newsArticlesModel).State = EntityState.Modified;
                    db.SaveChanges();

                    //Add Media File
                    if (ArticleMedia != null)
                    {
                        string FileDescription = Request.Form["MediaFileDescription"];
                        if (string.IsNullOrEmpty(FileDescription))
                        {
                            FileDescription = "Media File";
                        }
                        AppFunctions.UpdateMediaFile(newsArticlesModel.ID, mediaFileType, mediaFileName, FileDescription);
                    }


                    TempData["SuccessMessage"] = "Post updated successfully.";
                    return RedirectToAction("ManagePosts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["ErrorMessage"] = "Failed to update, please try again. If you continue to get this error, please contact the Administrator.";
                    //Log Error
                    SecurityFunctions.LogError(ex, admin_email, "EditArticle", null);

                    return View(newsArticlesModel);
                }
            }

            ViewBag.CATEGORIES = GetCategoriesList();
            ViewBag.TAGS = GetTagsList();

            return View(newsArticlesModel);
        }


        // GET: Admin/ArticleDetails/5
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ArticleDetails(int? id)
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

            int article_id = (int)id;
            //Not checking for Null! :-|
            string identifier = db.NewsArticles.Where(s => s.ID == article_id).FirstOrDefault().ArticleTextIdentifier;

            ViewBag.HasMedia = AppFunctions.ArticleHasMediaFile(AppFunctions.GetArticleIdFromIdentifier(identifier));
            ViewBag.ArticleMediaFileName = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileName");
            ViewBag.ArticleMediaFileCaption = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileCaption");
            ViewBag.ArticleMediaFileType = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileType");
            return View(newsArticlesModel);
        }


        // GET: PendingReview
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult PendingReview()
        {
            string admin_email = Session["sessionEmail"].ToString();
            var data = db.NewsArticles.Where(s => s.ArticleBy == admin_email && s.ReviewStatus == 0).OrderByDescending(s => s.ID);
            return View(data);
        }


        // GET: ReviewArticles
        [SessionAuthorize]
        [SessionHasRole]
        [SessionHasEditorRole]
        public ActionResult ReviewArticles()
        {
            string admin_email = Session["sessionEmail"].ToString();
            var data = db.NewsArticles.Where(s => s.ReviewStatus == 0 && s.DeleteStatus != true).OrderByDescending(s => s.ID);
            return View(data);
        }


        // GET: Admin/ReviewArticleDetails/5
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ReviewArticleDetails(int? id)
        {
            string category = "Africa";
            ViewBag.Category = category;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsArticlesModel newsArticlesModel = db.NewsArticles.Find(id);
            if (newsArticlesModel == null)
            {
                return HttpNotFound();
            }

            int article_id = (int)id;
            //Not checking for Null! :-|
            string identifier = db.NewsArticles.Where(s => s.ID == article_id).FirstOrDefault().ArticleTextIdentifier;

            ViewBag.HasMedia = AppFunctions.ArticleHasMediaFile(AppFunctions.GetArticleIdFromIdentifier(identifier));
            ViewBag.ArticleMediaFileName = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileName");
            ViewBag.ArticleMediaFileCaption = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileCaption");
            ViewBag.ArticleMediaFileType = AppFunctions.GetArticleMediaData(AppFunctions.GetArticleIdFromIdentifier(identifier), "FileType");
            //ViewBag.CurrentReviewComments = AppFunctions.GetNewsArticleData("ReviewComments", article_id);

            return View(newsArticlesModel);
        }


        // GET: Admin/ApprovePost
        [HttpPost]
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ApprovePost()
        {
            SecurityFunctions sf = new SecurityFunctions();
            var admin_email = Session["sessionEmail"].ToString();

            var PostID = Request.Form["PostID"];

            if (string.IsNullOrEmpty(PostID))
            {
                TempData["ErrorMessage"] = "Post id missing.";
                return RedirectToAction("ReviewArticles", "Admin");
            }

            try
            {
                AppFunctions.ApprovePostedArticle(Int32.Parse(PostID), admin_email);
                TempData["SuccessMessage"] = "Post approved.";
                var AdminFullName = sf.ReturnAccountData(admin_email, "FirstName") + " " + sf.ReturnAccountData(admin_email, "LastName");
                var PosterEmail = AppFunctions.GetNewsArticleData("ArticleBy", Int32.Parse(PostID));
                var PosterFullName = sf.ReturnAccountData(PosterEmail, "FirstName") + " " + sf.ReturnAccountData(PosterEmail, "LastName");

                //Notify Poster of Approval
                string to_name = PosterFullName;
                string h1_text = "Article Approved";
                string h2_text = null;
                string p1_text = "Dear "+ PosterFullName + ", your article has been approved by " + AdminFullName;
                string p2_text = null;
                AppEmailer.SendEmail(GMailer.GetGambiaReviewEmail(), AdminFullName, PosterEmail, PosterFullName, "Article Approved",
                    h1_text, h2_text, p1_text, p2_text, null, null, null, null, null);

                return RedirectToAction("ReviewArticles", "Admin");
            }
            catch (Exception ex) {
                // Log Error
                TempData["ErrorMessage"] = "Failed to approve post, please try again. If you continue to get this error, please contact the Administrator.";
                SecurityFunctions.LogError(ex, admin_email, "ApprovePost", null);
            }

            return View();
        }



        // GET: Admin/RejectPost
        [HttpPost]
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult RejectPost()
        {
            SecurityFunctions sf = new SecurityFunctions();
            var admin_email = Session["sessionEmail"].ToString();

            var RejectReason = Request.Form["RejectReason"];
            var PostID = Request.Form["PostID"];

            if (string.IsNullOrEmpty(PostID))
            {
                TempData["ErrorMessage"] = "Post id missing.";
                return RedirectToAction("ReviewArticles", "Admin");
            }

            if (string.IsNullOrEmpty(RejectReason))
            {
                TempData["ErrorMessage"] = "Reject reason missing.";
                return RedirectToAction("ReviewArticleDetails/" + PostID, "Admin");
            }


            try
            {
                //var CurrentReviewComments = AppFunctions.GetNewsArticleData("ReviewComments", Int32.Parse(PostID));
                var AdminFullName = sf.ReturnAccountData(admin_email, "FirstName") + " " + sf.ReturnAccountData(admin_email, "LastName");
                var PosterEmail = AppFunctions.GetNewsArticleData("ArticleBy", Int32.Parse(PostID));
                var PosterFullName = sf.ReturnAccountData(PosterEmail, "FirstName") + " " + sf.ReturnAccountData(PosterEmail, "LastName");

                AppFunctions.RejectPostedArticle(Int32.Parse(PostID), admin_email, PosterEmail, RejectReason);
                TempData["SuccessMessage"] = "Post rejected.";

                //Notify Poster of Rejection
                string to_name = PosterFullName;
                string h1_text = "Post Rejected";
                string h2_text = null;
                string p1_text = "Your post has been rejected by "+ AdminFullName + " with the following comments:";
                string p2_text = RejectReason;
                AppEmailer.SendEmail(GMailer.GetGambiaReviewEmail(), AdminFullName, PosterEmail, to_name, "Post Rejected",
                    h1_text, h2_text, p1_text, p2_text, null, null, null, null, null);


                return RedirectToAction("ReviewArticles", "Admin");
            }
            catch (Exception ex)
            {
                // Log Error
                TempData["ErrorMessage"] = "Failed to reject post, please try again. If you continue to get this error, please contact the Administrator.";
                SecurityFunctions.LogError(ex, admin_email, "ApprovePost", null);
            }

            return View();
        }

        // GET: ReviewArticlesComments
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ReviewArticlesComments()
        {
            string admin_email = Session["sessionEmail"].ToString();
            var data = db.PostReviews.Where(s => s.AddressedBy == admin_email && s.ReviewStatus == 0).OrderByDescending(s => s.ID);
            return View(data);
        }


        
        // GET: Admin/RejectPost
        [HttpPost]
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult AddReviewPostComment()
        {
            SecurityFunctions sf = new SecurityFunctions();
            var admin_email = Session["sessionEmail"].ToString();


            var AddressedByComments = Request.Form["AddressedByComments"];
            var PostReviewID = Request.Form["ModalPostReviewID"];
            var PostID = Request.Form["ModalPostID"];
            var ReviewerEmail = Request.Form["ReviewerEmail"];

            if (string.IsNullOrEmpty(PostReviewID))
            {
                TempData["ErrorMessage"] = "Post review id missing.";
                return RedirectToAction("ReviewArticlesComments", "Admin");
            }

            if (string.IsNullOrEmpty(PostID))
            {
                TempData["ErrorMessage"] = "Post id missing.";
                return RedirectToAction("ReviewArticlesComments", "Admin");
            }

            if (string.IsNullOrEmpty(AddressedByComments))
            {
                TempData["ErrorMessage"] = "Addressed by comments missing.";
                return RedirectToAction("ReviewArticlesComments" , "Admin");
            }


            try
            {
                var ReviewerFullName = sf.ReturnAccountData(ReviewerEmail, "FirstName") + " " + sf.ReturnAccountData(ReviewerEmail, "LastName");
                var PosterEmail = admin_email;
                var PosterFullName = sf.ReturnAccountData(PosterEmail, "FirstName") + " " + sf.ReturnAccountData(PosterEmail, "LastName");

                AppFunctions.AddReviewArticleComment(Int32.Parse(PostReviewID), AddressedByComments);
                TempData["SuccessMessage"] = "Review comment added.";

                //Notify Reviewer of Comment
                string to_name = ReviewerFullName;
                string h1_text = "Review Comment Added";
                string h2_text = null;
                string p1_text = PosterFullName + " added the following comments to a reviewed post:";
                string p2_text = AddressedByComments;
                AppEmailer.SendEmail(PosterEmail, PosterFullName, ReviewerEmail, ReviewerFullName, "Review Comment Added",
                    h1_text, h2_text, p1_text, p2_text, null, null, null, null, null);


                return RedirectToAction("ReviewArticlesComments", "Admin");
            }
            catch (Exception ex)
            {
                // Log Error
                TempData["ErrorMessage"] = "Failed to add comment, please try again. If you continue to get this error, please contact the Administrator.";
                SecurityFunctions.LogError(ex, admin_email, "ApprovePost", null);
            }

            return View();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------SETTINGS------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        // GET: MyProfile
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult MyProfile()
        {
            SecurityFunctions sf = new SecurityFunctions();
            int admin_id = Int32.Parse(sf.ReturnAccountData(Session["sessionEmail"].ToString(), "ID"));

            AccountsModel accountsModel = db.Accounts.Find(admin_id);
            if (accountsModel == null)
            {
                return HttpNotFound();
            }
            return View(accountsModel);
        }

        // GET: Adding/EditMyProfile/5
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult EditMyProfile()
        {
            SecurityFunctions sf = new SecurityFunctions();
            int admin_id = Int32.Parse(sf.ReturnAccountData(Session["sessionEmail"].ToString(), "ID"));

            AccountsModel accountsModel = db.Accounts.Find(admin_id);
            if (accountsModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.COUNTRY_ISOS = GetCountryIsoList();
            ViewBag.PHONE_CODES = GetPhoneCodeList();

            return View(accountsModel);
        }

        [HttpPost]
        [SessionAuthorize]
        [SessionHasRole]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyProfile([Bind(Include = "ID,FirstName,MiddleName,LastName,Country,CountryCode,PhoneNumber,Email,Password,Salt,Type,DirectoryName,Status,Oauth,AccountVerification,DateCreated")] AccountsModel accountsModel, HttpPostedFileBase ProfileImage)
        {
            ViewBag.COUNTRY_ISOS = GetCountryIsoList();
            ViewBag.PHONE_CODES = GetPhoneCodeList();

            if (ModelState.IsValid)
            {
                try
                {
                    SecurityFunctions sf = new SecurityFunctions();
                    accountsModel.Email = Session["sessionEmail"].ToString();

                    //Upload Headline Image Document
                    if (ProfileImage != null && ProfileImage.ContentLength > 0)
                    {
                        //Set old profile pic for delete
                        var old_profile = db.ProfilePictures.Where(s => s.UserEmail == accountsModel.Email).FirstOrDefault();

                        string folder_directory = Server.MapPath("~/images/accounts/") + sf.ReturnAccountData(accountsModel.Email, "DirectoryName");
                        Directory.CreateDirectory(folder_directory); //No need to check for existing directory as it is ignored if true
                        var Rand = AppFunctions.RandomInt(5);

                        var fullFileName = Path.GetFileName(ProfileImage.FileName);
                        if (fullFileName.Length > 90 && fullFileName.Length <= 100)
                        {
                            var trimText = fullFileName.Substring(60, 25);
                            fullFileName = fullFileName.Replace(trimText, "");
                        }
                        else if (fullFileName.Length > 100)
                        {
                            var trimText = fullFileName.Substring(50, 50);
                            fullFileName = fullFileName.Replace(trimText, "");
                        }
                        var fileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                        var fileType = Path.GetExtension(ProfileImage.FileName).Substring(1);
                        var updatedFileName = fileNameOnly + "_" + Rand + "_" + DateTime.Now.ToString("yyyyMMdd") + "." + fileType;
                        var profile_pic = updatedFileName;
                        var path = Path.Combine(folder_directory, updatedFileName);
                        ProfileImage.SaveAs(path);

                        if(!SecurityFunctions.AddProfilePic(accountsModel.Email, profile_pic))
                        {
                            TempData["ErrorMessage"] = "Failed to update profile info. Could not add profile picture. Please try again.";
                            return View(accountsModel);
                        }
                        Session["sessionProfilePic"] = sf.ReturnAccountData(accountsModel.Email, "DirectoryName") + "/" + profile_pic;
                        var pic = Session["sessionProfilePic"];
                        //Delete old pic
                        if (db.ProfilePictures.Any(s => s.UserEmail == accountsModel.Email))
                        {
                            var old_profile_pic = old_profile.ProfilePicture;
                            var filePath = Server.MapPath("~/images/accounts/" + sf.ReturnAccountData(accountsModel.Email, "DirectoryName") + "/" + old_profile_pic);
                            // Delete file
                            AppFunctions.DeleteFile(filePath);
                        }

                    }

                    accountsModel.Password = sf.ReturnAccountData(accountsModel.Email, "Password");
                    accountsModel.Salt = sf.ReturnAccountData(accountsModel.Email, "Salt");
                    accountsModel.Status = Int32.Parse(sf.ReturnAccountData(accountsModel.Email, "Status"));
                    accountsModel.DirectoryName = sf.ReturnAccountData(accountsModel.Email, "DirectoryName");


                    SecurityFunctions.UpdateProfileData(accountsModel.Email, accountsModel.FirstName, accountsModel.LastName, accountsModel.Country, accountsModel.CountryCode, accountsModel.PhoneNumber);

                    TempData["SuccessMessage"] = "Profile info updated.";
                    return RedirectToAction("MyProfile");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["ErrorMessage"] = "Failed to update profile info, please try again. If you continue to get this error, please contact the Administrator.";
                    //Log Error
                    SecurityFunctions.LogError(ex, accountsModel.Email, "EditMyProfile", null);

                    return View(accountsModel);
                }
            }
            else
            {
                StringBuilder result = new StringBuilder();
                foreach (var item in ModelState)
                {
                    string key = item.Key;
                    var errors = item.Value.Errors;

                    foreach (var error in errors)
                    {
                        result.Append(key + " " + error.ErrorMessage + "#");
                    }
                }
                TempData["ErrorMessage"] = "Opps!an  error occured. " + result.ToString();
                return View(accountsModel);
            }
            //return View(accountsModel);
        }


        // GET: Stats
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult Stats()
        {
            var admin_email = Session["sessionEmail"];
            var query_year = Request.Form["yr"];
            int year = DateTime.Now.Year;
            if(query_year != "" && query_year != null)
            {
               if(int.TryParse(query_year.ToString(), out year))
                {
                    year = Int32.Parse(query_year);
                }
            }

            //Total Page Views Per Month
            ViewBag.TotalPageViewsJan = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 01, year);
            ViewBag.TotalPageViewsFeb = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 02, year);
            ViewBag.TotalPageViewsMar = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 03, year);
            ViewBag.TotalPageViewsApr = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 04, year);
            ViewBag.TotalPageViewsMay = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 05, year);
            ViewBag.TotalPageViewsJun = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 06, year);
            ViewBag.TotalPageViewsJul = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 07, year);
            ViewBag.TotalPageViewsAug = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 08, year);
            ViewBag.TotalPageViewsSep = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 09, year);
            ViewBag.TotalPageViewsOct = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 10, year);
            ViewBag.TotalPageViewsNov = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 11, year);
            ViewBag.TotalPageViewsDec = AppFunctions.GetTotalViewsPerMonth(admin_email.ToString(), 12, year);

            //Total Posts Per Month
            ViewBag.TotalAdminPostJan = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 01, year);
            ViewBag.TotalAdminPostFeb = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 02, year);
            ViewBag.TotalAdminPostMar = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 03, year);
            ViewBag.TotalAdminPostApr = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 04, year);
            ViewBag.TotalAdminPostMay = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 05, year);
            ViewBag.TotalAdminPostJun = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 06, year);
            ViewBag.TotalAdminPostJul = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 07, year);
            ViewBag.TotalAdminPostAug = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 08, year);
            ViewBag.TotalAdminPostSep = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 09, year);
            ViewBag.TotalAdminPostOct = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 10, year);
            ViewBag.TotalAdminPostNov = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 11, year);
            ViewBag.TotalAdminPostDec = AppFunctions.GetTotalAdminPostPerMonth(admin_email.ToString(), 12, year);

            //Total views Per Category
            ViewBag.TotalAdminAfricaViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Africa");
            ViewBag.TotalAdminBusinessViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Business");
            ViewBag.TotalAdminHealthViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Health");
            ViewBag.TotalAdminInternationalViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "International");
            ViewBag.TotalAdminPoliticsViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Politics");
            ViewBag.TotalAdminSportsViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Sports");
            ViewBag.TotalAdminTechnologyViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Technology");
            ViewBag.TotalAdminGambiaViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Gambia");
            ViewBag.TotalAdminEntertainmentViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Entertainment");
            ViewBag.TotalAdminEducationViews = AppFunctions.GetTotalCategoryViews(admin_email.ToString(), "Education");

            //ViewData["TotalAdminPostsArray"] = AppFunctions.GetTotalAdminPostsArray(admin_email.ToString());
            //var TotalAdminPostsArray = ViewData["TotalAdminPostsArray"];

            return View();
        }

        // GET: Settings
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult Settings()
        {
            return View();
        }


        // GET: ActivityLog
        [SessionAuthorize]
        [SessionHasRole]
        public ActionResult ActivityLog()
        {
            string admin_email = Session["sessionEmail"].ToString();
            //SecurityFunctions.AddActivityLog(admin_email, "Test", "A Test Log", "/Admin/ActivityLog", null);
            var data = db.ActivityLog.Where(s => s.Email == admin_email);
            return View(data);
        }


        // GET: ManageUsers
        [SessionAuthorize]
        [SessionHasRole]
        [SessionHasSystemAdminRole]
        public ActionResult ManageUsers()
        {
            string admin_email = Session["sessionEmail"].ToString();

            var data = db.Accounts.ToList();
            return View(data);
        }


        // POST: /Dashboard/UpdateUserPassword/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpdateUserPassword()
        //{
        //    //Check if session valid
        //    if (!DBFunctions.IsSessionValid(Convert.ToString(Session["sessionID"]), Convert.ToString(Session["sessionEmail"])))
        //    {
        //        //Redirect to home page
        //        TempData["ProcessSessionFailureMessage"] = "Session expired. Please login again.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var user_id = DBFunctions.GetUserID(Convert.ToString(Session["sessionEmail"]));
        //    var OldPassword = Request.Form["OldPassword"];
        //    var OldShaPassword = Crypto.SHA256(OldPassword).ToString();
        //    var NewPassword = Request.Form["NewPassword"];
        //    var ConfirmPassword = Request.Form["ConfirmPassword"];

        //    if (!db.Users.Where(s => s.Id == user_id && s.Password == OldShaPassword).Any())
        //    {
        //        //Failure Message
        //        TempData["ProcessFailureMessage"] = "Old password did not match.";
        //        return RedirectToAction("Settings");
        //    }
        //    if (NewPassword != ConfirmPassword)
        //    {
        //        //Failure Message
        //        TempData["ProcessFailureMessage"] = "Passwords do not match!";
        //        return RedirectToAction("Settings");
        //    }


        //    if (DBFunctions.UpdateUserPassword(user_id, Crypto.SHA256(NewPassword).ToString()))
        //    {
        //        //Success Message
        //        Session.Remove("sessionID");
        //        Session.Remove("sessionEmail");
        //        //Redirect to home page
        //        TempData["ProcessSuccessMessage"] = "Pasword changed successfully. Login required...";

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        //Failure Message
        //        TempData["ProcessFailureMessage"] = "Failed to update password.";
        //        return RedirectToAction("Settings");
        //    }
        //}


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------LOGOUT------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        // GET: /Admin/Logout
        public ActionResult Logout()
        {
            Session.Remove("sessionID");
            Session.Remove("sessionEmail");
            Session.Remove("sessionFullName");

            return RedirectToAction("Index", "Home");
        }

        //Get List Of Categories
        public IEnumerable<SelectListItem> GetCategoriesList()
        {
            //Get the value from database and then set it to ViewBag to pass it View
            var categories_list = db.Categories.Select(w => new { ID = w.ID, CategoryName = w.CategoryName }).OrderBy(s => s.CategoryName).ToList();
            var categories = categories_list.Select(i => new SelectListItem
            {
                Value = i.ID.ToString(),
                Text = i.CategoryName
            });
            return categories;
        }

        //Get List Of Tags
        public IEnumerable<SelectListItem> GetTagsList()
        {
            //Get the value from database and then set it to ViewBag to pass it View
            var tags_list = db.Tags.Select(w => new { ID = w.ID, TagName = w.TagName }).OrderBy(s=> s.TagName).ToList();
            var tags = tags_list.Select(i => new SelectListItem
            {
                Value = i.TagName,
                Text = i.TagName
            });
            return tags;
        }

        //Get List Of Country Iso
        public IEnumerable<SelectListItem> GetCountryIsoList()
        {
            //Get the value from database and then set it to ViewBag to pass it View
            var iso_list = db.Country.Select(w => new { Iso = w.Iso, CountryName = w.Name }).OrderBy(s => s.CountryName).ToList();
            var isos = iso_list.Select(i => new SelectListItem
            {
                Value = i.Iso,
                Text = i.CountryName
            });
            return isos;
        }

        //Get List Of Country 
        public IEnumerable<SelectListItem> GetCountryList()
        {
            //Get the value from database and then set it to ViewBag to pass it View
            var country_list = db.Country.Select(w => new { CountryName = w.Name }).OrderBy(s => s.CountryName).ToList();
            var countries = country_list.Select(i => new SelectListItem
            {
                Value = i.CountryName,
                Text = i.CountryName
            });
            return countries;
        }

        //Get List Of Phone Code 
        public IEnumerable<SelectListItem> GetPhoneCodeList()
        {
            //Get the value from database and then set it to ViewBag to pass it View
            var phone_code_list = db.Country.Select(w => new { PhoneCode = w.Phonecode, CountryName = w.Name }).OrderBy(s => s.CountryName).ToList();
            var phone_codes = phone_code_list.Select(i => new SelectListItem
            {
                Value = i.PhoneCode.ToString(),
                Text = i.CountryName + " ("+i.PhoneCode.ToString() +")"
            });
            return phone_codes;
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
