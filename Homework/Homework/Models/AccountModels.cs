using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.WebPages.Html;


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
        [Display(Name = "Nume")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Prenume")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma parola")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Liceu")]
        public int selectedHighschool { get; set; }
        public System.Web.Mvc.SelectList Highschools { get; set; }

        [Display(Name = "An studiu")]
        public int selectedYear { get; set; }
        public System.Web.Mvc.SelectList Years { get; set; }

        [Display(Name = "Clasa")]
        public string selectedClass { get; set; }
        public System.Web.Mvc.SelectList Classes { get; set; }

        [Display(Name = "Tip")]
        public int type { get; set; }

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
        public string username;
        public string text;
        public DateTime data;
    }

    public class AddHomeworkModel
    {
        [Required]
        [Display( Name = "Titlu" )]
        public string title { get; set; }

        [Required]
        [Display( Name = "Enunt" )]
        public string enunt { get; set; }
        
        [Required]
        [Display( Name = "Deadline" )]
        public DateTime deadline { get; set; }

        [Display( Name = "Fisier Help" )]
        public HttpPostedFileBase help { get; set; }

        [Required]
        [Display( Name = "Fisiere In-Out" )]
        public HttpPostedFileBase in_out { get; set; }

        [Required]
        [Display( Name = "An Studiu" )]
        public int an { get; set; }

        [Required]
        [Display( Name = "Clasa" )]
        public string clasa { get; set; }

        [Required]
        [Display( Name = "Privat" )]
        public bool privat { get; set; }


    }

    public class ChangeInfo
    {
        public string nume { get; set; }
        public string prenume { get; set; }
        public string email { get; set; }
        public string parola { get; set; }
        public string clasa { get; set; }
        public int anStudiu { get; set; }

    }

    public class TemaAModel { 
        public string titlu { get; set; } 
        public string prof { get; set; } 
        public string liceu { get; set; } 
        public double rating { get; set; } 
        public DateTime data { get; set; } 
        public int id_tema { get; set; } }

}
