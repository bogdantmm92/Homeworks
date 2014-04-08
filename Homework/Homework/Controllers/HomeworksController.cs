using Homework.Filters;
using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Homework.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeworksController : Controller
    {

        public ActionResult Teme()
        {
            using (var db = new HomeworkContext())
            {
                var model = new TemeModel();
                model.teme = db.Temas.Where(a => a.id_tema <= 2).ToList();

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddHomework()
        {
            ViewBag.Title = "Creeaza Tema";
            return View();
        }

        [HttpPost]
        [Authorize]
        
        public ActionResult AddHomework(AddHomeworkModel model)
        {
            ViewBag.Title = "Creeaza Tema";
            using (var db = new HomeworkContext())
            {
                var tema = new Tema();
                var fisierInOut = new Fisier();
                var fisierHelp = new Fisier();

                tema.deadline = model.deadline;
                tema.enunt = model.enunt;
                
                fisierInOut.cale = model.title;
                //fisierInOut.
                fisierHelp.cale = model.title;

                db.Fisiers.Add(fisierInOut);
                db.Fisiers.Add(fisierHelp);
                //db.SaveChanges();

                tema.Fisier = fisierInOut;
                tema.Fisier1 = fisierHelp;

                tema.id_in_out = fisierInOut.id_fisier;
                tema.id_help = fisierHelp.id_fisier;

                //WebSecurity.InitializeDatabaseConnection;
                tema.id_prof = WebSecurity.GetUserId(User.Identity.Name);
                tema.privat = model.privat == true ? 1 : 0;

                tema.titlu = model.title;

                db.Temas.Add(tema);
                db.SaveChanges();
            }

            return View();


        }

    }




}
