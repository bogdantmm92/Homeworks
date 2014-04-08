using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;
using WebMatrix.WebData;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Homework.Controllers
{
    [Authorize]
    public class HomeworksController : Controller
    {

      /*  public ActionResult Teme()
        {
            using (var db = new HomeworkContext())
            {
                var model = new TemeModel();
                model.teme = db.Temas.Where(a => a.id_tema <= 2).ToList();

                return View(model);
            }
        }*/
        [HttpPost]
        public ActionResult AddComment(/*int id_tema,*/ SeeHomeworkModel model) 
        { 
            using (var db = new HomeworkContext())
            { if (ModelState.IsValid) 
            {
                try
                {
                    var f = new Comentariu();
                    f.data = DateTime.Now;
                    f.id_tema = 1;
                    f.id_user = 1;
                    f.text = model.c.text;
                    

                    db.Comentarius.Add(f);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
              return RedirectToAction("ShowHomework"); } 
              return View(model); 
            } 
        }
        public ActionResult ShowHomework()
        {
            using (var db = new HomeworkContext())
            {
                int id_tema=1;
                var m = new SeeHomeworkModel();
                var model = new HomeworkModel();
                var rt = new RatingModel();
                var cm = new CommentModel();

                var tema = db.Temas.Where(t => t.id_tema == id_tema).FirstOrDefault();
                model.Title = tema.titlu;
                model.Text = tema.enunt;
                int id_prof = tema.id_prof;
                var nume_prof = db.Users.Where(t => t.id_user == id_prof).FirstOrDefault();
                model.Professor = nume_prof.nume +" "+nume_prof.prenume;
                var rating = db.Ratings.Where(t => t.id_tema == id_tema).ToList();
                var rat = rating.Average(a => a.rating1);
                model.rating = rat;
                int id_fis = (int)tema.id_help;
                var fisier = db.Fisiers.Where( a => a.id_fisier == id_fis).FirstOrDefault();
                model.help = fisier.cale;

                model.comentariu = new List<CommentModel>();
                var lista_com = db.Comentarius.Where(a => a.id_tema == id_tema).OrderBy( a=> a.data).ToList();
                foreach(var c in lista_com)
                {
                    CommentModel com = new CommentModel();
                    com.data = c.data;
                    com.text = c.text;
                    var sel = db.Users.Where(t => t.id_user == c.id_user).FirstOrDefault();
                    com.username = sel.nume + "  " + sel.prenume;
                    model.comentariu.Add(com);
                }
                model.current_grade = 0; // ------------------------- Aici e harcodat
               // Session["user_id"] = 1; // ------------------------- Aici e harcodat
                model.grade = db.Submits.Where(a => a.id_user == 1).FirstOrDefault().rezultat;

                m.Hm = model;
                m.r = rt;
                m.c = cm;
                return View(m);


            }
        }

        public ActionResult ArhivaTeme()
        {
            using (var db = new HomeworkContext())
            {
                var model = new List<TemaAModel>();

                foreach (var t in db.Temas.Where(a => (a.deadline < DateTime.Now && a.privat == 0)))
                {
                    var tm = new TemaAModel();
                    tm.data = t.deadline;
                    tm.titlu = t.titlu;
                    var prof = db.Users.Where(a => a.id_user == t.id_prof).FirstOrDefault();
                    tm.prof = prof.nume + " " + prof.prenume;
                    var l = db.Liceus.Where(a => a.id_liceu == prof.id_liceu).FirstOrDefault();
                    var r = db.Ratings.Where( a => a.id_tema == t.id_tema).ToList();
                    tm.rating = r.Average(a => a.rating1);
                    tm.liceu = l.nume;

                    model.Add(tm);
                }

                return View(model);
            }
        }




    }
}
