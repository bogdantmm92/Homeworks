using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Web.Security;


namespace Homework.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }


    //Other models
    public class TemeModel
    {
        public List<Tema> teme;
    }

    public class HomeworkModel
    {
        public string Title;
        public string Professor;
        public DateTime deadline;
        public int grade;
        public double rating;
        public string Text;
        public string help;
        public int current_grade;
        public List<CommentModel> comentariu;
    }

    public class CommentModel
    {
        public string username { get; set; }
        public string text { get; set; }
        public DateTime data { get; set; } 
    }
    public class RatingModel 
    { public int rating { get; set; } 
        public int id_tema { get; set; } 
        public int id_user { get; set; } 
    }
    public class SeeHomeworkModel
    {
        public HomeworkModel Hm{ get; set; } 
        public RatingModel r{ get; set; } 
        public CommentModel c{ get; set; } 
    }
    public class AddHomeworkModel
    {
        public string title;
        public string enunt { get; set; }
        public DateTime deadline { get; set; }
        public FileStream help { get; set; }
        public FileStream in_out { get; set; }
        public int an { get; set; }
        public string clasa { get; set; }
        public bool privat;


    }

    public class TemaAModel {
        public string titlu;
        public string prof;
        public string liceu;
        public double rating;
        public DateTime data;
    }

   

}
