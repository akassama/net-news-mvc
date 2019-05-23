using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GambiaReview.Models
{
    [Table("ArticleComments")]
    public class ArticleCommentsModel
    {

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        //[Required]
        [Display(Name = "Article ID")]
        public int ArticleID { get; set; }

        [Display(Name = "Commenter Name")]
        public string Name { get; set; }

        //[Required]
        [Display(Name = "UserEmail")]
        public string UserEmail { get; set; }

        //[Required]
        //[Display(Name = "Article Category")]
        //public int ArticleCategory { get; set; }

        [Display(Name = "IsReply")]
        public bool IsReply { get; set; }

        [Display(Name = "Reply Comment ID")]
        public int ReplyCommentID { get; set; }

        [Required]
        [Display(Name = "CommentText")]
        public string CommentText { get; set; }

        //[Required]
        [Display(Name = "UniqueCommentID")]
        public string UniqueCommentID { get; set; }

        [Display(Name = "Comment Date")]
        public DateTime CommentDate { get; set; } = DateTime.Now;
    }

}