using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace GambiaReview.Models
{
    public class AppFunctions
    {
        //Returns Article data based on ID
        public static string GetNewsArticleData(string return_string, int article_id)
        {
            using (var db = new DBConnection())
            {
                //Dynamically get data of article, based on the position of record. 1 == Last record, 2 == Second to last record
                var data = db.NewsArticles.Where(s => s.ArticleTextIdentifier != null && s.ID == article_id);

                if (data.Any())
                {
                    if (return_string == "ArticleTextIdentifier")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleTextIdentifier;
                    }

                    else if (return_string == "ArticleBy")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleBy;
                    }

                    else if (return_string == "ArticleCategory")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleCategory.ToString();
                    }

                    else if (return_string == "ArticleDate")
                    {

                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleDate.ToString();
                    }

                    else if (return_string == "ArticleHeadline")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleHeadline;
                    }

                    if (return_string == "ArticleHeadlineImage")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleHeadlineImage;
                    }

                    else if (return_string == "HealineImageDescription")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.HeadlineImageDescription;
                    }
                    else if (return_string == "ArticleBody")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ArticleBody;
                    }
                    else if (return_string == "DeleteStatus")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.DeleteStatus.ToString();
                    }
                    else if (return_string == "Tags")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.Tags;
                    }
                    //else if (return_string == "ReviewComments")
                    //{
                    //    var record_data = data.FirstOrDefault();
                    //    return record_data.ReviewComments;
                    //}
                    else if (return_string == "ReviewStatus")
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ReviewStatus.ToString();
                    }
                }
                return null;
            }
        }

        //Returns Article data based on position and return string. [Needs Checking!]
        public static string GetNewsArticleData(string return_string, int position, bool return_id)
        {
            using (var db = new DBConnection())
            {
                //Dynamically get data of article, based on the position of record. 1 == Last record, 2 == Second to last record
                var data = db.NewsArticles.Where(s => s.ArticleTextIdentifier != null);
                if (position == 1)
                {
                    if (data.OrderByDescending(p => p.ID).Take(1) != null)
                    {
                        data = data.OrderByDescending(p => p.ID).Take(1);
                    }
                }
                else
                {
                    int skip_total = position - 1; //setting skip_total to -1 of any position
                    if (data.OrderByDescending(p => p.ID).Skip(skip_total).Take(1) != null)
                    {
                        data = data.OrderByDescending(p => p.ID).Skip(skip_total).Take(1);
                    }
                }

                if (data.Any())
                {
                    if (return_id)
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ID.ToString();
                    }
                    else { 
                        if (return_string == "ArticleTextIdentifier")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleTextIdentifier;
                        }

                        else if (return_string == "ArticleBy")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleBy;
                        }

                        else if (return_string == "ArticleCategory")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleCategory.ToString();
                        }

                        else if (return_string == "ArticleDate")
                        {

                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleDate.ToString();
                        }

                        else if (return_string == "ArticleHeadline")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleHeadline;
                        }

                        if (return_string == "ArticleHeadlineImage")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleHeadlineImage;
                        }

                        else if (return_string == "HealineImageDescription")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.HeadlineImageDescription;
                        }

                        else if (return_string == "ArticleBody")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleBody;
                        }
                        else if (return_string == "Tags")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.Tags;
                        }
                        //else if (return_string == "ReviewComments")
                        //{
                        //    var record_data = data.FirstOrDefault();
                        //    return record_data.ReviewComments;
                        //}
                        else if (return_string == "ReviewStatus")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ReviewStatus.ToString();
                        }
                    }
                }
                return null;
            }
        }

        //Returns Article data. Requires post to be approved and not deleted [Needs Checking!]
        public static string GetNewsArticleData(string return_string, int position, bool return_id, int review_ststus = 1)
        {
            using (var db = new DBConnection())
            {
                var data = db.NewsArticles.Where(s => s.ArticleTextIdentifier != null && s.ReviewStatus == 1 && s.DeleteStatus != true);
                if (position == 1)
                {
                    if (data.OrderByDescending(p => p.ID).Take(1) != null)
                    {
                        data = data.OrderByDescending(p => p.ID).Take(1);
                    }
                }
                else
                {
                    int skip_total = position - 1;
                    if (data.OrderByDescending(p => p.ID).Skip(skip_total).Take(1) != null)
                    {
                        data = data.OrderByDescending(p => p.ID).Skip(skip_total).Take(1);
                    }
                }

                if (data.Any())
                {
                    if (return_id)
                    {
                        var record_data = data.FirstOrDefault();
                        return record_data.ID.ToString();
                    }
                    else
                    {
                        if (return_string == "ArticleTextIdentifier")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleTextIdentifier;
                        }

                        else if (return_string == "ArticleBy")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleBy;
                        }

                        else if (return_string == "ArticleCategory")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleCategory.ToString();
                        }

                        else if (return_string == "ArticleDate")
                        {

                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleDate.ToString();
                        }

                        else if (return_string == "ArticleHeadline")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleHeadline;
                        }

                        if (return_string == "ArticleHeadlineImage")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleHeadlineImage;
                        }

                        else if (return_string == "HealineImageDescription")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.HeadlineImageDescription;
                        }

                        else if (return_string == "ArticleBody")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ArticleBody;
                        }
                        else if (return_string == "DeleteStatus")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.DeleteStatus.ToString();
                        }
                        else if (return_string == "Tags")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.Tags;
                        }
                        //else if (return_string == "ReviewComments")
                        //{
                        //    var record_data = data.FirstOrDefault();
                        //    return record_data.ReviewComments;
                        //}
                        else if (return_string == "ReviewStatus")
                        {
                            var record_data = data.FirstOrDefault();
                            return record_data.ReviewStatus.ToString();
                        }
                    }
                }
                return null;
            }
        }

        public static int GetNewsArticleIDFromTextID(string text_id)
        {
            int return_id = 0;
            if (string.IsNullOrEmpty(text_id))
            {
                return return_id;
            }

            using (var db = new DBConnection())
            {
                var newArticleQuery = db.NewsArticles.Where(s => s.ArticleTextIdentifier == text_id);
                if (newArticleQuery.Any())
                {
                    if (newArticleQuery.FirstOrDefault() != null)
                    {
                        var newArticleData = newArticleQuery.FirstOrDefault();
                        return_id = newArticleData.ID;
                    }
                }
            }
            return return_id;
        }


        public static int GetCategoryIDFromText(string category_text)
        {
            int return_id = 0;
            if (string.IsNullOrEmpty(category_text))
            {
                return return_id;
            }

            using (var db = new DBConnection())
            {
                var categoryIDQuery = db.Categories.Where(s => s.CategoryName == category_text);
                if (categoryIDQuery.Any())
                {
                    if (categoryIDQuery.FirstOrDefault() != null)
                    {
                        var categoryData = categoryIDQuery.FirstOrDefault();
                        return_id = categoryData.ID;
                    }
                }
            }
            return return_id;
        }


        //Add new subscriber
        public static bool AddNewSubscriber(string subscriber_email)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                // Create a new object.
                SubscribersModel subscriber = new SubscribersModel
                {
                    SubscriberEmail = subscriber_email,
                    Status = 1
                    // …
                };

                // Add the new object to the collection.
                db.Subcribers.Add(subscriber);

                // Submit the change to the database.
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Log error
                    SecurityFunctions.LogError(ex, subscriber_email, "AddNewSubscriber", null);
                }
            }
            return process_status;
        }

        //Update Subcriber Status
        public static bool UpdateSubcriberStatus(string subscriber_email, int status)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                // Query the database for the row to be updated.
                var query =
                    from subscriber in db.Subcribers
                    where subscriber.SubscriberEmail == subscriber_email
                    select subscriber;

                // Execute the query, and change the column values
                // you want to change.
                foreach (SubscribersModel subscriber_data in query)
                {
                    subscriber_data.Status = status;
                    // Insert any additional changes to column values.
                }

                // Submit the changes to the database.
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

            }
            return process_status;
        }

        //LINQ Random Alphunumeric Generator
        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //LINQ Random Number Generator
        public static string RandomInt(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // original by patel.milanb
        //https://stackoverflow.com/questions/11395775/clean-the-string-is-there-any-better-way-of-doing-it
        //public static string CleanString(string input_string)
        //{
        //    string removeChars = " ?&^$#@!()+-,:;<>’\'-_*";
        //    string result = input_string;

        //    foreach (char c in removeChars)
        //    {
        //        result = result.Replace(c.ToString(), string.Empty);
        //    }

        //    return result;
        //}

        public static string CleanString(string dirtyString)
        {
            HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
            StringBuilder result = new StringBuilder(dirtyString.Length);
            foreach (char c in dirtyString)
                if (!removeChars.Contains(c)) // prevent dirty chars
                    result.Append(c);
            return result.ToString();
        }

        public static string FirstLetterToUpper(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string first_char = text.Substring(0, 1);
                string rest_of_chars = text.Substring(1, text.Length-1);
                return first_char.ToUpper() + rest_of_chars.ToLower();
            }
            return text;
        }

        public static string GetUsernameFromEmail(string email)
        {
            if (!string.IsNullOrEmpty(email) && email.Contains("@"))
            {
                string[] directory_arr = email.Split(new[] { '@' });
                return directory_arr[0];
            }
            return email;
        }
        
        public static string GetDefaultProfileLink()
        {
            return "default/default.jpg";
        }


        //Get total AdminPage Views
        public static int GetTotalAdminPostsViews(string admin_email)
        {
            int total_count = 0;
            using(var db = new DBConnection())
            {
                IQueryable<NewsArticlesModel> articles = from p in db.NewsArticles
                                                         where p.ArticleBy == admin_email
                                                         orderby p.ID descending
                                                         select p;
                NewsArticlesModel[] articlesArray = articles.ToArray();

                foreach(var item in articlesArray)
                {
                    total_count = total_count + db.PageVisits.Count(s => s.ArticleID == item.ID);
                }
            }
            return total_count;
        }

        //Get total AdminPage Views
        public static int GetTotalViewsPerMonth(string admin_email, int month, int year)
        {
            int total_count = 0;
            using (var db = new DBConnection())
            {
                IQueryable<NewsArticlesModel> articles = from p in db.NewsArticles
                                                         where p.ArticleBy == admin_email
                                                         orderby p.ID descending
                                                         select p;
                NewsArticlesModel[] articlesArray = articles.ToArray();

                foreach (var item in articlesArray)
                {
                    total_count = total_count + db.PageVisits.Count(s => s.ArticleID == item.ID && s.VisitDate.Month == month && s.VisitDate.Year == year);
                }
            }
            return total_count;
        }


        //Get total Admin Post By Month
        public static int GetTotalAdminPostPerMonth(string admin_email, int month, int year)
        {
            using (var db = new DBConnection())
            {
                return db.NewsArticles.Count(s => s.ArticleBy == admin_email && s.DeleteStatus != true && s.ReviewStatus == 1 && s.ArticleDate.Month == month && s.ArticleDate.Year == year);
            }
        }

        //Get total Category Views
        public static int GetTotalCategoryViews(string admin_email, string category)
        {
            using (var db = new DBConnection())
            {
                int category_id = GetCategoryIDFromText(category);
                return db.NewsArticles.Count(s => s.ArticleBy == admin_email && s.DeleteStatus != true && s.ReviewStatus == 1 && s.ArticleCategory == category_id);
            }
        }




        public static int GetTotalPendingReviews(string admin_email)
        {
            //Incomplete
            using(var db = new DBConnection())
            {
                return db.NewsArticles.Count(s => s.ArticleBy == admin_email && s.DeleteStatus != true && s.ReviewStatus == 0);
            }
        }

        public static int GetTotalIncompletePosts(string admin_email)
        {
            //Incomplete
            return 0;
        }

        //Add Medai File To DB
        public static bool AddMediaFile(int article_id, string file_type, string file_name, string file_caption)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                ArticleMediaUploadsModel media = new ArticleMediaUploadsModel
                {
                    ArticleID = article_id,
                    FileType = file_type,
                    FileName = file_name,
                    FileCaption = file_caption,
                    DateAdded = DateTime.Now
                };
                
                db.ArticleMediaUploads.Add(media);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Log Error
                    SecurityFunctions.LogError(ex, null, "AddMediaFile", null);
                }

            }
            return process_status;
        }

        //Update media file
        public static bool UpdateMediaFile(int article_id, string file_type, string file_name, string file_caption)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                var query =
                    from media in db.ArticleMediaUploads
                    where media.ID == article_id
                    select media;
                foreach (ArticleMediaUploadsModel media_data in query)
                {
                    media_data.FileType = file_type;
                    media_data.FileName = file_name;
                    media_data.FileCaption = file_caption;
                }
                
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Log Error
                    SecurityFunctions.LogError(ex, null, "UpdateMediaFile", null);
                }

            }
            return process_status;
        }


        //Check if Article Has Media File
        public static bool ArticleHasMediaFile(int article_id)
        {
            using(var db = new DBConnection())
            {
                if(db.ArticleMediaUploads.Any(s=> s.ArticleID == article_id))
                {
                    return true;
                }
            }
            return false;
        }


        //Get Article MediaData
        public static string GetArticleMediaData(int article_id, string return_data)
        {
            using (var db = new DBConnection())
            {
                var query = db.ArticleMediaUploads.Where(s => s.ArticleID == article_id);
                if (query.Any())
                {
                    if(return_data == "FileType")
                    {
                        return query.FirstOrDefault().FileType;
                    }
                    else if (return_data == "FileCaption")
                    {
                        return query.FirstOrDefault().FileCaption;
                    }
                    else if (return_data == "DateAdded")
                    {
                        return query.FirstOrDefault().DateAdded.ToString();
                    }
                    return query.FirstOrDefault().FileName;
                }
            }
            return null;
        }

        //Get caterogy text
        public static string GetCategoryName(int category_id)
        {
            using(var db = new DBConnection())
            {
                string category;
                category = (db.Categories.Where(s => s.ID == category_id).FirstOrDefault() != null) ? db.Categories.Where(s => s.ID == category_id).FirstOrDefault().CategoryName : "unknown";
                return db.Categories.Where(s => s.ID == category_id).FirstOrDefault().CategoryName;
            }
        }

        //Get Approved Article ID
        public static int GetLatestArticleIds(int return_row)
        {
            using (var db = new DBConnection())
            {
                var lastFiveProducts = db.NewsArticles.Where(s => s.ReviewStatus == 1 && s.DeleteStatus == false).OrderByDescending(p => p.ArticleDate).Take(6).ToArray();
                if (lastFiveProducts.Any())
                {
                    
                    var fetch_row = lastFiveProducts[return_row];
                    return fetch_row.ID;
                }
                return 0;
            }
        }

        public static int GetLatestArticleIds(int return_row, int review_status)
        {
            using (var db = new DBConnection())
            {
                var lastFiveProducts = db.NewsArticles.Where(s => s.ReviewStatus == review_status && s.DeleteStatus == false).OrderByDescending(p => p.ArticleDate).Take(6).ToArray();
                if (lastFiveProducts.Any())
                {
                    var fetch_row = lastFiveProducts[return_row];
                    return fetch_row.ID;
                }
                return 0;
            }
        }

        //Get caterogy text
        public static int GetArticleIdFromIdentifier(string identifier)
        {
            using (var db = new DBConnection())
            {
                var query = db.NewsArticles.Where(s => s.DeleteStatus == false && s.ArticleTextIdentifier == identifier);
                if (query.Any())
                {
                    return query.FirstOrDefault().ID;
                }
                return 0;
            }
        }

        //Approve Post
        public static bool ApprovePostedArticle(int post_id, string approved_by)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                var query =
                    from post in db.NewsArticles
                    where post.ID == post_id
                    select post;

                foreach (NewsArticlesModel post_data in query)
                {
                    post_data.ReviewedBy = approved_by;
                    post_data.ReviewStatus = 1;
                }
                
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Log Error
                    SecurityFunctions.LogError(ex, approved_by, "ApprovePostedArticle", null);
                }

            }
            return process_status;
        }

        //Reject Post
        public static bool RejectPostedArticle(int post_id, string rejected_by, string poster_email, string reject_reason)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                PostReviewsModel reject = new PostReviewsModel
                {
                    PostID = post_id,
                    ReviewedBy = rejected_by,
                    AddressedBy = poster_email,
                    ReviewComments = reject_reason,
                    ReviewStatus = 0,
                    ReviewDate = DateTime.Now
                };
                
                db.PostReviews.Add(reject);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception ex)
                {
                    // Log Error
                    SecurityFunctions.LogError(ex, rejected_by, "RejectPostedArticle", null);
                }

            }
            return process_status;
        }


        //Add Review Comment
        public static bool AddReviewArticleComment(int post_review_id, string reviewer_comment)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                var query =
                    from review in db.PostReviews
                    where review.ID == post_review_id
                    select review;
                
                foreach (PostReviewsModel review_data in query)
                {
                    review_data.AddressedByComments = reviewer_comment;
                    review_data.ReviewStatus = 1;
                }
                
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    // Log Error
                    SecurityFunctions.LogError(ex, null, "AddReviewArticleComment", null);
                }

            }
            return process_status;
        }


        //Check if user has a specific role
        public static bool ValidateUserRole(int role_id)
        {
            if (HttpContext.Current.Session["sessionEmail"] == null)
            {
                return false;
            }

            string email = HttpContext.Current.Session["sessionEmail"].ToString();
            using (var db = new DBConnection())
            {
                if(db.Groups.Any(s=> s.RoleID == role_id && s.Email == email))
                {
                    return true;
                }
                return false;
            }
        }

        //Get role id based on role name
        public static int GetRoleID(string role_name)
        {
            using (var db = new DBConnection())
            {
                var query = db.Roles.Where(s => s.RoleName == role_name);
                if (query.Any())
                {
                    return query.FirstOrDefault().ID;
                }
                return 0;
            }
        }

        //Check if user has a specific role
        public static bool UserHasRole()
        {
            if (HttpContext.Current.Session["sessionEmail"] == null)
            {
                return false;
            }

            string email = HttpContext.Current.Session["sessionEmail"].ToString();
            using (var db = new DBConnection())
            {
                if (db.Groups.Any(s => s.Email == email))
                {
                    return true;
                }
                return false;
            }
        }

        //Add Page Visit
        public static bool AddArticlePageVisit(int article_id)
        {
            bool process_status = false;
            string user_email;
            user_email = (HttpContext.Current.Session["sessionEmail"] != null) ? HttpContext.Current.Session["sessionEmail"].ToString() : null;
            string ip_address = HttpContext.Current.Request.UserHostAddress;
            string country = RegionInfo.CurrentRegion.DisplayName;
            bool device = HttpContext.Current.Request.Browser.IsMobileDevice;
            string machineType = "Unknown";
            if (device)
            {
                machineType = "Mobile";
            }
            else if (device == false)
            {
                machineType = "Desktop";
            }
            string browser = HttpContext.Current.Request.Browser.Type;
            string device_details = null;


            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    //Insert record to db
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO [PageVisits] ([ArticleID] ,[UserEmail] ,[IpAddress] ,[Country] ,[Browser] ,[Device] ,[VisitDate] ,[DeviceDetails]) 
									VALUES 
							(@var0, @var1, @var2, @var3, @var4, @var5, @var6, @var7)";
                    cmd.Parameters.AddWithValue("@var0", article_id);
                    cmd.Parameters.AddWithValue("@var1", ((object)user_email) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var2", ((object)ip_address) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var3", ((object)country) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var4", ((object)browser) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var5", ((object)machineType) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var6", DateTime.Now);
                    cmd.Parameters.AddWithValue("@var7", ((object)device_details) ?? DBNull.Value);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        process_status = true;
                    }
                    else
                    {
                        process_status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Log Error
                SecurityFunctions.LogError(ex, user_email, "AddArticlePageVisit", null);
            }
            finally
            {
                if (conn != null)
                {
                    //cleanup connection i.e close 
                    conn.Close();
                }
            }
            return process_status;
        }

        //Add Page Visit
        public static bool AddArticlePageVisitOld(int article_id)
        {
            bool process_status = false;
            string user_email;
            user_email = (HttpContext.Current.Session["sessionEmail"] != null) ? HttpContext.Current.Session["sessionEmail"].ToString() : null;
            string ip_address = HttpContext.Current.Request.UserHostAddress;
            string country = RegionInfo.CurrentRegion.DisplayName;
            bool device = HttpContext.Current.Request.Browser.IsMobileDevice;
            string machineType = "Unknown";
            if (device)
            {
                machineType = "Mobile";
            }
            else if (device == false)
            {
                machineType = "Desktop";
            }
            string browser = HttpContext.Current.Request.Browser.Type;
            string device_details = null;

            using (var db = new DBConnection())
            {
                PageVisitsModel visits = new PageVisitsModel
                {
                    ArticleID = article_id,
                    UserEmail = user_email,
                    IpAddress = ip_address,
                    Country = country,
                    Browser = browser,
                    Device = machineType,
                    VisitDate = DateTime.Now,
                    DeviceDetails = device_details
                    // …
                };
                
                db.PageVisits.Add(visits);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Log Error
                    SecurityFunctions.LogError(ex, user_email, "AddArticlePageVisit", null);
                }

            }
            return process_status;
        }




        //Delete File
        public static bool DeleteFile(string file_path)
        {
            // Delete a file by using File class static method...
            if (System.IO.File.Exists(file_path))
            {
                try
                {
                    System.IO.File.Delete(file_path);
                    return true;
                }
                catch (Exception ex)
                {
                    //Log Error
                    SecurityFunctions.LogError(ex, null, "DeleteFile", null);
                }
            }
            return false;
        }



        public static string ListToCsv(string[] list)
        {
            var return_data = "";
            if (list != null)
            {
                foreach (var item in list)
                {
                    return_data = return_data+item + ",";
                }
                return return_data.TrimEnd(',');
            }
            return null;
        }









        ///DUMP///

        //public static NewsArticlesModel[] GetTotalAdminPostsArray(string admin_email)
        //{
        //    using (var db = new DBConnection())
        //    {
        //        IQueryable<NewsArticlesModel> articles = from p in db.NewsArticles
        //                                                 where p.ArticleBy == admin_email
        //                                                 orderby p.ID descending
        //                                                 select p;
        //        NewsArticlesModel[] articlesArray = articles.ToArray();

        //        return articlesArray;
        //    }
        //}


        //public static NewsArticlesModel[] GetAdminPostIDlist(string admin_email)
        //{
        //    using (var db = new DBConnection())
        //    {
        //        IQueryable<NewsArticlesModel> articles = from p in db.NewsArticles where p.ArticleBy == admin_email
        //                                       orderby p.ID descending
        //                                       select p;
        //        NewsArticlesModel[] articlesArray = articles.ToArray();
        //        return articlesArray;
        //    }
        //}

        //public static List<string> GetAdminPostIDlist(string admin_email)
        //{
        //    List<string> result = new List<string>();
        //    using (var db = new DBConnection())
        //    {
        //        var Articles = from q in db.NewsArticles
        //                    where q.ArticleBy == admin_email
        //                       select q;

        //        foreach (var c in Articles)
        //        {
        //            string row = c.ID.ToString();
        //            result.Add(row);
        //        }
        //    }
        //    return result;
        //}








    }
}