using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace GambiaReview.Models
{
    [Table("NewsArticles")]
    public class NewsArticlesModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Article Text Identifier")]
        public string ArticleTextIdentifier { get; set; }

        [Required]
        [Display(Name = "Article By")]
        public string ArticleBy { get; set; }

        [Required]
        [Display(Name = "Article Category")]
        public int ArticleCategory { get; set; }

        [Required]
        [Display(Name = "Article Date")]
        public DateTime ArticleDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Article Healine")]
        public string ArticleHeadline { get; set; }

        [Required]
        [Display(Name = "Article Healine Image")]
        public string ArticleHeadlineImage { get; set; }

        [Required]
        [Display(Name = "Image Description")]
        public string HeadlineImageDescription { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Article Body")]
        public string ArticleBody { get; set; }

        [Display(Name = "Delete Status")]
        public bool DeleteStatus { get; set; } = false;

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        //[Display(Name = "Review Comments")]
        //public string ReviewComments { get; set; }

        [Display(Name = "Review Status")]
        public int ReviewStatus { get; set; }
        //public IEnumerable<MultiSelectList> Tags { get; set; }
        //public IEnumerable<int> Tags { get; set; }
    }


    [Table("Tags")]
    public class TagsModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Tag Name")]
        public string TagName { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }

    [Table("PostReviews")]
    public class PostReviewsModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Post ID")]
        public int PostID { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        [Display(Name = "Review Comments")]
        public string ReviewComments { get; set; }

        [Display(Name = "Addressed By")]
        public string AddressedBy { get; set; }

        [Display(Name = "Addressed By Comments")]
        public string AddressedByComments { get; set; }

        [Display(Name = "Review Status")]
        public int ReviewStatus { get; set; }

        [Display(Name = "Review Date")]
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }

    [Table("vw_Posts")]
    public class PostsModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ArticleID")]
        public int ArticleID { get; set; }

        [Required]
        [Display(Name = "Article Text Identifier")]
        public string ArticleTextIdentifier { get; set; }

        [Required]
        [Display(Name = "Article By")]
        public string ArticleBy { get; set; }

        [Required]
        [Display(Name = "Article Category")]
        public int ArticleCategory { get; set; }

        [Required]
        [Display(Name = "Article Date")]
        public DateTime ArticleDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Article Healine")]
        public string ArticleHeadline { get; set; }

        [Required]
        [Display(Name = "Article Healine Image")]
        public string ArticleHeadlineImage { get; set; }

        [Required]
        [Display(Name = "Image Description")]
        public string HeadlineImageDescription { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Article Body")]
        public string ArticleBody { get; set; }

        [Display(Name = "Delete Status")]
        public bool DeleteStatus { get; set; } = false;

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Review ID")]
        public int ReviewID { get; set; }

        [Display(Name = "Post ID")]
        public int PostID { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        //[Display(Name = "Review Comments")]
        //public string ReviewComments { get; set; }

        [Display(Name = "Review Status")]
        public int ReviewStatus { get; set; }
    }

}