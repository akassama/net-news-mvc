using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GambiaReview.Models
{
    [Table("Subscribers")]
    public class SubscribersModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Subscriber Email")]
        public string SubscriberEmail { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Date Subscribed")]
        public DateTime? DateSubscribed { get; set; } = DateTime.Now;
    }
}