using PagedList;
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
        [Display( Name = "Nume" )]
        public string nume {
            get;
            set;
        }

        [Display( Name = "Prenume" )]
        public string prenume {
            get;
            set;
        }

        [Display( Name = "Email" )]
        public string email {
            get;
            set;
        }

        [Display( Name = "Clasa" )]
        public string clasa {
            get;
            set;
        }

        [Display( Name = "An" )]
        public int an {
            get;
            set;
        }

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
    public class TemaModel
    {
        public string titlu { get; set; }
        public string prof { get; set; }
        public string liceu { get; set; }
        public double rating { get; set; }
        public DateTime data { get; set; }
        public int id_tema { get; set; }
        public int id_prof { get; set; }
    }

  

    public class ProfesoriModel
    {
        public int id_prof { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string liceu { get; set; }
        public double rating { get; set; }
        public int numar_teme { get; set; }
        public string NumeProfesor { get; set; }
        public int id_liceu { get; set; }
    }




    public class UserModel
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string highschool { get; set; }
        public char className { get; set; }
        public int year { get; set; }
        public int type { get; set; }
    }


    public class LiceuModel
    {
        public int id_liceu;
        public string nume;
        public double rating_total;
    }

    public class LiceeModel
    {
        public PagedList<Liceu> licee { get; set; }
    }

   public class SearchModel
   {
       public string NumeProfesor { get; set; }
      
   }

    public class HomeworkModel
    {
        public string Title { get; set; }
        public string Professor { get; set; }
        public DateTime deadline { get; set; }
        public int grade { get; set; }
        public double rating { get; set; }
        public string Text { get; set; }
        public int? help { get; set; }
        public int in_out { get; set; }
        public int current_grade { get; set; }
        //public List<CommentModel> comentariu { get; set; }
        public PagedList<CommentModel> comentariu { get; set; }
        public int id_tema { get; set; }
        public int privat { get; set; }
    }

    public class CommentModel
    {
        public string username { get; set; }
        public string text { get; set; }
        public DateTime data { get; set; }
    }
    public class RatingModel
    {
        public int rating { get; set; }
        public int id_tema { get; set; }
        public int id_user { get; set; }
    }
    
    public class SeeHomeworkModel
    {
        public HomeworkModel Hm { get; set; }
        public RatingModel r { get; set; }
        public CommentModel c { get; set; }

        [Required]
        [Display(Name = "Source code")]
        public HttpPostedFileBase source_code { get; set; }

        public int id_tema { get; set; }
        public int id_prof { get; set; }

        public string clase { get; set; }
        public int an { get; set; }

        public List<Elev> elev;

        public bool send { get; set; }
        public int rating { get; set; }
    }

    public class Elev {
        public string nume { get; set; }
        public int id_elev { get; set; }
        public string prenume { get; set; }
        public bool isChecked { get; set; }

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
        [Display(Name = "Nume")]
        public string nume { get; set; }
        [Display(Name = "Prenume")]
        public string prenume { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Parola")]
        public string parola { get; set; }
        [Display(Name = "Clasa")]
        public string clasa { get; set; }
        [Display(Name = "An de Studiu")]
        public int anStudiu { get; set; }

    }

    public class TemaAModel { 
        public string titlu { get; set; } 
        public string prof { get; set; } 
        public string liceu { get; set; } 
        public double rating { get; set; } 
        public DateTime data { get; set; } 
        public int id_tema { get; set; }
    }
    public class TemeProfModel 
    { public PagedList<TemaModel> teme { get; set; } 
        public int id_prof { get; set; } 
    }
    public class NotaModel {
        public string Nume { get; set; }
        public int Nota { get; set; }
        public int An { get; set; }
        public string Clasa { get; set; }
    }
	
	    public class SourceModel
    {
            public int result { get; set; }
        public string titlu { get; set; }
        public string username { get; set; }
        public int id_source { get; set; }
        public int id_submit { get; set; }
        public DateTime data { get; set; }
    }

        public class IndexModel
        {
            public PagedList<TemaAModel> teme { get; set; }
            public string NumeProfesor { get; set; }
        }



}
