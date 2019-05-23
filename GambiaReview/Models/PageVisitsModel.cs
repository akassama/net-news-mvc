using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GambiaReview.Models
{
    [Table("PageVisits")]
    public class PageVisitsModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Article ID")]
        public int ArticleID { get; set; }

        [Required]
        [Display(Name = "UserEmail")]
        public string UserEmail { get; set; }

        [Display(Name = "IpAddress")]
        public string IpAddress { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Browser")]
        public string Browser { get; set; }

        [Display(Name = "Device")]
        public string Device { get; set; }

        [Display(Name = "DeviceDetails")]
        public string DeviceDetails { get; set; }

        [Display(Name = "VisitDate")]
        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}