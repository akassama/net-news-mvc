using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GambiaReview.Models
{
    public class DBConnection : DbContext
    {
        public DBConnection()
    : base("name=DBConnection")
        {

        }

        public DbSet<NewsArticlesModel> NewsArticles { get; set; }

        //public DbSet<PostsModel> Posts { get; set; }

        public DbSet<ArticleMediaUploadsModel> ArticleMediaUploads { get; set; }

        public DbSet<ArticleCommentsModel> ArticleComments { get; set; }

        public DbSet<ContactMessageModel> ContactMessages { get; set; }

        public DbSet<PostReviewsModel> PostReviews { get; set; }

        public DbSet<PageVisitsModel> PageVisits { get; set; }

        public DbSet<ActivityLogModel> ActivityLog { get; set; }

        public DbSet<ErrorLogModel> ErrorLog { get; set; }

        public DbSet<TagsModel> Tags { get; set; }

        public DbSet<CategoriesModel> Categories { get; set; }

        public DbSet<CountryModel> Country { get; set; }

        public DbSet<AccountsModel> Accounts { get; set; }

        public DbSet<RolesModel> Roles { get; set; }

        public DbSet<ProfilePicturesModel> ProfilePictures { get; set; }

        public DbSet<SubscribersModel> Subcribers { get; set; }

        public DbSet<AccountVerificationInfoModel> AccountVerificationInfo { get; set; }

        public DbSet<LoginInfoModel> LoginInfo { get; set; }

        public DbSet<PasswordRecoveryInfoModel> PasswordRecoveryInfo { get; set; }

        public DbSet<GroupsModel> Groups { get; set; }

        public DbSet<AdvertsModel> Adverts { get; set; }
    }
}