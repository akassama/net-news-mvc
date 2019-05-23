using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GambiaReview.Models
{

    [Table("ArticleMediaUploads")]
    public class ArticleMediaUploadsModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Article ID")]
        public int ArticleID { get; set; }

        [Required]
        [Display(Name = "File Type")]
        public string FileType { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "File Caption")]
        public string FileCaption { get; set; }

        [Required]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }

}