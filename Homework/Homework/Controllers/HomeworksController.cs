using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;

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

        public ActionResult ShowHomework(int id_tema)
        {
            using (var db = new HomeworkContext())
            {
                var model = new HomeworkModel();
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
                var lista_com = db.Comentarius.Where(a => a.id_tema == id_tema).ToList();
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

                return View(model);


            }
        }



    }
}
