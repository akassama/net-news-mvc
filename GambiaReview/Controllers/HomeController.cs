using GambiaReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GambiaReview.Controllers
{
    public class HomeController : Controller
    {
        private DBConnection db = new DBConnection();

        public ActionResult Index()
        {
            AppFunctions apf = new AppFunctions();

            ViewBag.HeadLine1 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(0));
            ViewBag.HeadLine1ID = AppFunctions.GetLatestArticleIds(0).ToString();
            //
            ViewBag.HeadLine2 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(1));
            ViewBag.HeadLine2ID = AppFunctions.GetLatestArticleIds(1).ToString();
            //
            ViewBag.HeadLine3 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(2));
            ViewBag.HeadLine3ID = AppFunctions.GetLatestArticleIds(2).ToString();
            //
            ViewBag.HeadLine4 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(3));
            ViewBag.HeadLine4ID = AppFunctions.GetLatestArticleIds(3).ToString();
            //
            ViewBag.HeadLine5 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(4));
            ViewBag.HeadLine5ID = AppFunctions.GetLatestArticleIds(4).ToString();
            //
            ViewBag.HeadLine6 = AppFunctions.GetNewsArticleData("ArticleHeadline", AppFunctions.GetLatestArticleIds(5));
            ViewBag.HeadLine6ID = AppFunctions.GetLatestArticleIds(5).ToString();

            ViewBag.ShowData = true;
            if(db.NewsArticles.Count(s=> s.ReviewStatus == 1 && s.DeleteStatus != true) < 6)
            {
                ViewBag.ShowData = false;
            }

            var data = db.NewsArticles.Where(s => s.ReviewStatus == 1 && s.DeleteStatus != true);
            return View(data);
            //var data = db.Posts.Where(s => s.ReviewStatus == 1 && s.DeleteStatus != true);
            //return View(db.Posts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(NewsArticlesModel subscribersModel)
        {
            var ReturnUrl = Request.Form["ReturnUrl"];
            var search = Request.Form["search"];

            if (search == null)
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            search = AppFunctions.CleanString(search);
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            search = rgx.Replace(search, "");

            Response.Redirect("~/Home/Search/?q=" + search);
            return RedirectToAction("Search/?q="+ search, "Home");
        }

        //Search
        public ActionResult Search()
        {
            var ReturnUrl = Request.Form["ReturnUrl"];
            var query = Request.QueryString["q"];

            if (query == null)
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            query = AppFunctions.CleanString(query);

            ViewBag.Search = query;

            ViewBag.CountTotal = db.NewsArticles.Count(s => (s.ArticleTextIdentifier.Contains(query) || s.ArticleHeadline.Contains(query) || s.ArticleBody.Contains(query) || s.Tags.Contains(query)) && (s.DeleteStatus == false && s.ReviewStatus == 1));
            var data = db.NewsArticles.Where(s => (s.ArticleTextIdentifier.Contains(query) || s.ArticleHeadline.Contains(query) || s.ArticleBody.Contains(query) || s.Tags.Contains(query)) && (s.DeleteStatus == false && s.ReviewStatus == 1)).OrderByDescending(s=> s.ArticleDate);

            return View(data);
        }

        //Tags
        public ActionResult Tags()
        {
            var ReturnUrl = Request.Form["ReturnUrl"];
            var tag = Request.QueryString["tag"];

            if (tag == null)
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            tag = AppFunctions.CleanString(tag);

            ViewBag.Tag = tag;

            ViewBag.CountTotal = db.NewsArticles.Count(s => (s.Tags.Contains(tag)) && (s.DeleteStatus == false && s.ReviewStatus == 1));
            var data = db.NewsArticles.Where(s => (s.Tags.Contains(tag)) && (s.DeleteStatus == false && s.ReviewStatus == 1)).OrderByDescending(s => s.ArticleDate);

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe([Bind(Include = "SubscriberEmail")] SubscribersModel subscribersModel)
        {
            var ReturnUrl = Request.Form["ReturnUrl"];
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = "Index";
            }

            TempData["SubsAlertDiv"] = "SubsAlertDiv";
            if (ModelState.IsValid)
            {
                subscribersModel.Status = 1;

                if(db.Subcribers.Any(s=> s.SubscriberEmail == subscribersModel.SubscriberEmail && s.Status != 0))
                {
                    TempData["ErrorMessage"] = "Already subscribed.";
                    return Redirect(ReturnUrl);
                }
                else if(db.Subcribers.Any(s => s.SubscriberEmail == subscribersModel.SubscriberEmail && s.Status == 0))
                {
                    //Update status
                    if(AppFunctions.UpdateSubcriberStatus(subscribersModel.SubscriberEmail, 1))
                    {
                        TempData["SuccessMessage"] = "Subscription successfull.";
                        return Redirect(ReturnUrl);
                    }
                    TempData["ErrorMessage"] = "Failed to re-subscribe.";
                    return Redirect(ReturnUrl);
                }

                db.Subcribers.Add(subscribersModel);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Subscription successfull.";

                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Opps!an  error occured.";
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Subscribe([Bind(Include = "SubscriberEmail")] SubscribersModel subscribersModel)
        //{
        //    var SubscriberEmail = Request.Form["SubscriberEmail"];
        //    if (!String.IsNullOrEmpty(SubscriberEmail))
        //    {
        //        //var SubscriberEmail = Request.Form["SubscriberEmail"];
        //        subscribersModel.SubscriberEmail = SubscriberEmail;
        //        if (AppFunctions.AddNewSubscriber(SubscriberEmail))
        //        {
        //            TempData["SuccessMessage"] = "Subscription successfull.";
        //        }
        //        return RedirectToAction("Gambia");
        //    }

        //    return RedirectToAction("Health");
        //}


        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountsModel usersmodel)
        {
            var returnController = "Home";
            var returnAction = "Index";
            if (!string.IsNullOrEmpty(Request.Form["returnController"]) && !string.IsNullOrEmpty(Request.Form["returnAction"]))
            {
                returnController = Request.Form["returnController"];
                returnAction = Request.Form["returnAction"];
            }

            //If both inputs not empty
            if (!string.IsNullOrEmpty(Request.Form["userEmail"]) && !string.IsNullOrEmpty(Request.Form["userPassword"]))
            {
                var user_email = Request.Form["userEmail"];
                var user_password = Request.Form["userPassword"]; //Hash password
                var remember = Request.Form["RememberMe"];
                //bool remember_me = false;   //Convert.ToBoolean(Request.Form["RememberMe"]);

                SecurityFunctions Security = new SecurityFunctions();
                if (Security.IsLoginValid(user_email, user_password))
                {
                    //Check if account is active
                    int AccountStatus = Security.ReturnAccountStatus(user_email);
                    if (AccountStatus == 0)
                    {
                        //Login valid but pending approval
                        TempData["ProcessLoginFailureMessage"] = "This account has not yet been approved by the administrator.";
                        TempData["displayModal"] = "loginModal";
                        return RedirectToAction(returnAction, returnController);
                    }

                    if (AccountStatus == 2)
                    {
                        //Login valid but pending approval
                        TempData["ProcessLoginFailureMessage"] = "This account is currently suspended. Please contact us at (+90) 5314950226 or send us a message.";
                        TempData["displayModal"] = "loginModal";
                        return RedirectToAction(returnAction, returnController);
                    }

                    //Valid Login Status
                    if (Security.IsAccountLocked(user_email))
                    {
                        //Login valid but account locked
                        TempData["ProcessLoginFailureMessage"] = "This account is currently locked. Try again later.";
                        TempData["displayModal"] = "loginModal";
                        return RedirectToAction(returnAction, returnController);
                    }

                    //If All Good
                    if (AccountStatus == 1)
                    {
                        //Login valid
                        var SessionID = SecurityFunctions.ComputeSha256Hash(AppFunctions.RandomString(12).ToString());
                        if (!SecurityFunctions.AddLoginInfo(user_email, DateTime.Now, 0, 0, null, 0, SessionID, DateTime.Now))
                        {
                            //Think of what to do here
                            //Log Error
                        }

                        Session["sessionID"] = SessionID;
                        Session["sessionEmail"] = user_email;
                        Session["sessionFullName"] = Security.ReturnAccountData(user_email, "FirstName") + " " + Security.ReturnAccountData(user_email, "LastName");
                        Session["sessionOauth"] = false;

                        if (string.IsNullOrEmpty(Security.ReturnAccountData(user_email, "FirstName")))
                        {
                            Session["sessionFullName"] = user_email;
                        }

                        Session["sessionProfilePic"] = Security.ReturnAccountData(user_email, "DirectoryName") + "/" + Security.ReturnAccountProfilePic(user_email);
                        if (string.IsNullOrEmpty(Security.ReturnAccountData(user_email, "DirectoryName")) || string.IsNullOrEmpty(Security.ReturnAccountProfilePic(user_email)))
                        {
                            Session["sessionProfilePic"] = AppFunctions.GetDefaultProfileLink();
                        }

                        //Set Admin Session Values
                        //System Admin
                        int sys_admin_id = AppFunctions.GetRoleID("SystemAdmin");
                        if (AppFunctions.ValidateUserRole(sys_admin_id)) { Session["sessionSystemAdmin"] = true; }

                        //Editor
                        int editor_id = AppFunctions.GetRoleID("Editor");
                        if (AppFunctions.ValidateUserRole(editor_id)) { Session["sessionEditor"] = true; }

                        //Author
                        int author_id = AppFunctions.GetRoleID("Author");
                        if (AppFunctions.ValidateUserRole(author_id)) { Session["sessionAuthor"] = true; }

                        //Censor
                        int censor_id = AppFunctions.GetRoleID("Censor");
                        if (AppFunctions.ValidateUserRole(censor_id)) { Session["sessionCensor"] = true; }

                        //Advertiser
                        int advitiser_id = AppFunctions.GetRoleID("Advertiser");
                        if (AppFunctions.ValidateUserRole(censor_id)) { Session["sessionAdvertiser"] = true; }

                        if (AppFunctions.UserHasRole())//ie user has an admin role
                        {
                            return RedirectToAction("Index", "Admin");
                        }

                        return RedirectToAction("Index", "User");
                    }

                }
                else
                {
                    //Login not valid
                    TempData["ProcessLoginFailureMessage"] = "Wrong username or password! Please try again.";
                    TempData["displayModal"] = "loginModal";

                    return RedirectToAction(returnAction, returnController);
                }
            }

            // If we got this far, something failed, redisplay form
            //Login not valid
            TempData["ProcessLoginFailureMessage"] = "Email and password field required.";
            TempData["displayModal"] = "loginModal";

            return RedirectToAction(returnAction, returnController);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountsModel accountmodel)
        {
            var returnController = "Home";
            var returnAction = "Index";
            if (!string.IsNullOrEmpty(Request.Form["returnController"]) && !string.IsNullOrEmpty(Request.Form["returnAction"]))
            {
                returnController = Request.Form["returnController"];
                returnAction = Request.Form["returnAction"];
            }

            if (ModelState.IsValid)
            {
                //Set default values for account
                var confirm_password = Request.Form["RepeatPassword"];
                //If passwords do not match
                if (accountmodel.Password != confirm_password)
                {
                    TempData["ProcessRegisterFailureMessage"] = "Passwords do not match.";
                    TempData["displayModal"] = "registerModal";
                    return RedirectToAction(returnAction, returnController);
                }

                //Check if email exist already
                if (db.Accounts.Where(s => s.Email == accountmodel.Email).Any())
                {
                    TempData["ProcessRegisterFailureMessage"] = "The email provided already exist, please try again with a different email.";
                    TempData["displayModal"] = "registerModal";
                    return RedirectToAction(returnAction, returnController);
                }

                //Add registration
                if (SecurityFunctions.AddNewRegistration(accountmodel.Email, accountmodel.Password, false))
                {
                    string to_name = AppFunctions.FirstLetterToUpper(AppFunctions.GetUsernameFromEmail(accountmodel.Email));
                    string h1_text = "Welcome to Gambia Review.";
                    string h2_text = null;
                    string p1_text = "You've successfully registered in Gambia Review application system.";
                    string p2_text = "You can contact us by phone +7 (495) 280-14- 81 (ext. 3397) or by using the feedback form, which is presented on the questionnaire page. Our staff will promptly provide answers to your questions. You can fill in the questionnaire sections in any order convenient for you.";
                    //Send email to registerer
                    AppEmailer.SendEmail(GMailer.GetGambiaReviewEmail(), "Gambia Review", accountmodel.Email, to_name, "Welcome to Gambia Review", 
                        h1_text, h2_text, p1_text, p2_text, null, null,null, null, null);

                    //Send email to Gambia Review
                    to_name = "Gambia Review Team";
                    h1_text = "New User Registered";
                    h2_text = null;
                    p1_text = "User with email :"+ accountmodel.Email +" has registered.";
                    p2_text = null;
                    AppEmailer.SendEmail(GMailer.GetGambiaReviewEmail(), "Gambia Review", "akassama@yahoo.com", to_name, "Welcome to Gambia Review",
                        h1_text, h2_text, p1_text, p2_text, null, null, null, null, null);


                    //Add Login data 
                    var SessionID = SecurityFunctions.ComputeSha256Hash(AppFunctions.RandomString(12).ToString());
                    if (!SecurityFunctions.AddLoginInfo(accountmodel.Email, DateTime.Now, 0, 0, null, 0, SessionID, DateTime.Now))
                    {
                        //Think of what to do here
                        //Log Error
                    }


                    TempData["ProcessSuccessMessage"] = "Registration successfull.";
                    SecurityFunctions Security = new SecurityFunctions();
                    Session["sessionID"] = SessionID;
                    Session["sessionEmail"] = accountmodel.Email;
                    Session["sessionFullName"] = Security.ReturnAccountData(accountmodel.Email, "FirstName") + " " + Security.ReturnAccountData(accountmodel.Email, "LastName");
                    if (string.IsNullOrEmpty(Security.ReturnAccountData(accountmodel.Email, "FirstName")))
                    {
                        Session["sessionFullName"] = accountmodel.Email;
                    }

                    Session["sessionProfilePic"] = Security.ReturnAccountData(accountmodel.Email, "DirectoryName") + "/" + Security.ReturnAccountProfilePic(accountmodel.Email);
                    if (string.IsNullOrEmpty(Security.ReturnAccountData(accountmodel.Email, "DirectoryName")))
                    {
                        Session["sessionProfilePic"] = AppFunctions.GetDefaultProfileLink();
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ProcessFailureMessage"] = "Registration failed, please try again.";
                    return RedirectToAction(returnAction, returnController);
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
                TempData["ProcessRegisterFailureMessage"] = result.ToString(); 
                TempData["displayModal"] = "registerModal";
            }



            return RedirectToAction(returnAction, returnController);

        }


        //ADD COMMENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "CommentText")] ArticleCommentsModel commentsModel)
        {
            var ReturnUrl = Request.Form["ReturnUrl"];
            var ArticleID = Request.Form["ArticleID"];
            var UserEmail = Session["sessionEmail"];
            var CommenterName = Request.Form["CommenterName"];
            if((CommenterName != "" && CommenterName.Length < 3) || (commentsModel.CommentText.Length < 2))
            {
                TempData["ErrorMessage"] = "Invalid inputs.";
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            bool IsReply = Convert.ToBoolean(Convert.ToInt32(Request.Form["IsReply"]));
            if (IsReply)
            {
                commentsModel.ReplyCommentID = Int32.Parse(Request.Form["ReplyCommentID"]);
            }

            if (ModelState.IsValid)
            {
                commentsModel.ArticleID = Int32.Parse(ArticleID);
                commentsModel.Name = CommenterName;
                if (UserEmail != null)
                {
                    commentsModel.UserEmail = UserEmail.ToString();
                }
                commentsModel.IsReply = IsReply;
                commentsModel.UniqueCommentID = AppFunctions.RandomString(2) + AppFunctions.RandomInt(8) + AppFunctions.RandomString(2);

                db.ArticleComments.Add(commentsModel);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Comment added.";

                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
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

                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return View("Index", "Home");
            }


        }


        // GET: Home/Contact
        public ActionResult Contact()
        {
            return View();
        }

        // GET: Home/PrivacyPolicy
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        // GET: Home/Advertise
        public ActionResult Advertise()
        {
            return View();
        }

        // GET: Home/Adverts
        public ActionResult Adverts()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ID,Name,Email,PhoneNumber,Subject,Message,ViewStatus,DateSent")] ContactMessageModel contactsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    contactsModel.ViewStatus = 0;

                    db.ContactMessages.Add(contactsModel);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Thank you for getting in touch!";

                    //Send email to Gambia Review
                    string to_name = GMailer.GetGambiaReviewName();
                    string h1_text = contactsModel.Subject;
                    string h2_text = null;
                    string p1_text = contactsModel.Message;
                    string p2_text = null;
                    AppEmailer.SendEmail(contactsModel.Email, contactsModel.Name, GMailer.GetGambiaReviewEmail(), to_name, contactsModel.Subject,
                        h1_text, h2_text, p1_text, p2_text, null, null, null, null, null);

                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to send message, please try again. If you continue to get this error, please send an email to gambiareview@gmail.com.";
                    SecurityFunctions.LogError(ex, contactsModel.Email, "Contact", null);
                }
            }

            return View(contactsModel);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


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

        /// <summary>
        /// ADD COMMENT
        /// </summary>
        /// <param name="returnUrl"></param>
        ///      
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddComment([Bind(Include = "CommentText")] ArticleCommentsModel commentsModel)
        //{
        //    var ReturnUrl = Request.Form["ReturnUrl"];
        //    var ArticleID = Request.Form["ArticleID"];
        //    var UserEmail = Session["sessionEmail"];
        //    bool IsReply = Convert.ToBoolean(Convert.ToInt32(Request.Form["IsReply"]));
        //    if (IsReply)
        //    {
        //        commentsModel.ReplyCommentID = Int32.Parse(Request.Form["ReplyCommentID"]);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        commentsModel.ArticleID = Int32.Parse(ArticleID);
        //        commentsModel.UserEmail = UserEmail.ToString();
        //        commentsModel.IsReply = IsReply;
        //        commentsModel.UniqueCommentID = AppFunctions.RandomString(2) + AppFunctions.RandomInt(8) + AppFunctions.RandomString(2);

        //        db.ArticleComments.Add(commentsModel);
        //        db.SaveChanges();
        //        TempData["SuccessMessage"] = "Comment added.";

        //        if (!string.IsNullOrEmpty(ReturnUrl))
        //        {
        //            return Redirect(ReturnUrl);
        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        StringBuilder result = new StringBuilder();
        //        foreach (var item in ModelState)
        //        {
        //            string key = item.Key;
        //            var errors = item.Value.Errors;

        //            foreach (var error in errors)
        //            {
        //                result.Append(key + " " + error.ErrorMessage + "#");
        //            }
        //        }
        //        TempData["ErrorMessage"] = "Opps!an  error occured. " + result.ToString();

        //        if (!string.IsNullOrEmpty(ReturnUrl))
        //        {
        //            return Redirect(ReturnUrl);
        //        }
        //        return View("Index", "Home");
        //    }


        //}

        //
    }
}