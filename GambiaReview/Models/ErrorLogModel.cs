using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GambiaReview.Models
{
    [Table("ErrorLogs")]
    public class ErrorLogModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }

        [Display(Name = "Function")]
        public string Function { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Exception")]
        public string Exception { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "LogDate")]
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}