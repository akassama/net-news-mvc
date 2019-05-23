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
    [Table("Adverts")]
    public class AdvertsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Advert Text Identifier")]
        public string AdvertTextIdentifier { get; set; }

        [Required]
        [Display(Name = "Advert By")]
        public string AdvertBy { get; set; }

        [Required]
        [Display(Name = "Advert Category")]
        public int AdvertCategory { get; set; }

        [Required]
        [Display(Name = "Advert Date")]
        public DateTime AdvertDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Advert Healine")]
        public string AdvertHeadline { get; set; }

        [Required]
        [Display(Name = "Advert Image")]
        public string AdvertImage { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Advert Body")]
        public string AdvertBody { get; set; }

        [Display(Name = "Delete Status")]
        public bool DeleteStatus { get; set; } = false;

        [Display(Name = "Advert Expiry Date")]
        public DateTime AdvertExpiryDate { get; set; }
    }
}