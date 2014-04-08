using Homework.Filters;
using Homework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
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
                
                var fileName = Path.GetFileName(model.in_out.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                model.in_out.SaveAs(path);

                fisierInOut.cale = path;

                fileName = Path.GetFileName(model.help.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                model.help.SaveAs(path);

                fisierHelp.cale = path;

                db.Fisiers.Add(fisierInOut);
                db.Fisiers.Add(fisierHelp);
                //db.SaveChanges();

                tema.Fisier = fisierInOut;
                tema.Fisier1 = fisierHelp;

                tema.id_in_out = fisierInOut.id_fisier;
                tema.id_help = fisierHelp.id_fisier;

                //WebSecurity.InitializeDatabaseConnection;
                tema.id_prof = (int)Session["UserId"];
                tema.privat = model.privat == true ? 1 : 0;

                tema.titlu = model.title;

                db.Temas.Add(tema);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

            return View();


        }

    }




}
