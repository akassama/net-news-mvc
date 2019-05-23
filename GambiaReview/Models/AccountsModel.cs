using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GambiaReview.Models
{
    [Table("Accounts")]
    public class AccountsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Country Code")]
        public int? CountryCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,}", ErrorMessage = "Must contain at least one number and one uppercase and lowercase letter, and at least 6 or more characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Salt")]
        public string Salt { get; set; }

        //[Display(Name = "Type")]
        //public int? Type { get; set; }

        [Display(Name = "Directory Name")]
        public string DirectoryName { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        [Display(Name = "Oauth")]
        public int? Oauth { get; set; }

        [Display(Name = "Account Verification")]
        public int? AccountVerification { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }

    [Table("LoginInfo")]
    public class LoginInfoModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Required]
        [Display(Name = "Failed Login Count")]
        public int FailedLoginCount { get; set; }

        [Required]
        [Display(Name = "Locked Status")]
        public int LockedStatus { get; set; }

        [Display(Name = "LockPeriod")]
        public DateTime? LockPeriod { get; set; } 

        [Required]
        [Display(Name = "Total Logins")]
        public int TotalLogins { get; set; }

        [Required]
        [Display(Name = "Login Session ID")]
        public string LoginSessionID { get; set; }

        [Required]
        [Display(Name = "First Login")]
        public DateTime FirstLogin { get; set; } = DateTime.Now;
    }


    [Table("AccountVerificationInfo")]
    public class AccountVerificationInfoModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Verification Sent Date")]
        public DateTime VerificationSentDate { get; set; }

        [Required]
        [Display(Name = "Verification Expiry Date")]
        public DateTime VerificationExpiryDate { get; set; }

        [Display(Name = "Verification Status")]
        public int VerificationStatus { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
    }


    [Table("PasswordRecoveryInfo")]
    public class PasswordRecoveryInfoModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Recovery HashKey")]
        public string RecoveryHashKey { get; set; }

        [Required]
        [Display(Name = "Recovery Text Key")]
        public string RecoveryTextKey { get; set; }

        [Display(Name = "Recovery Sent Date")]
        public DateTime? RecoverySentDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Recovery Expiry Date")]
        public DateTime RecoveryExpiryDate { get; set; }

        [Required]
        [Display(Name = "Recovery Status")]
        public int RecoveryStatus { get; set; }
    }


    [Table("Country")]
    public class CountryModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Iso")]
        public string Iso { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nice Name")]
        public string NiceName { get; set; }

        [Required]
        [Display(Name = "Iso3")]
        public string Iso3 { get; set; }

        [Required]
        [Display(Name = "Numcode")]
        public int Numcode { get; set; }

        [Required]
        [Display(Name = "Phonecode")]
        public int Phonecode { get; set; }
    }

    [Table("Roles")]
    public class RolesModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; } = DateTime.Now;

    }

    [Table("ActivityLog")]
    public class ActivityLogModel
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "ActivityType")]
        public string ActivityType { get; set; }

        [Display(Name = "Activity Description")]
        public string ActivityDescription { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "ActionID")]
        public string ActionID { get; set; }

        [Required]
        [Display(Name = "ActivityDate")]
        public DateTime ActivityDate { get; set; } = DateTime.Now;
    }

}