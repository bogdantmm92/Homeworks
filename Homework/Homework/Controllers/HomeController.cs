using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework.Controllers
{
    public class HomeController : Controller
    {
        /*
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }*/

        public ActionResult Index()
        {
            using (var db = new HomeworkContext())
            {
                var model = new List<TemaAModel>();

                try
                {
                    int id = (int)Session["UserId"];

                    //var id = 2;
                    var liceu = db.Liceus.Join(db.Users, a => a.id_liceu, b => b.id_liceu, (a, b) => new { liceu = a, user = b }).Where(a => a.user.id_user == id).FirstOrDefault();
                    var teme = db.Temas.Join(db.Users, a => a.id_prof, b => b.id_user, (a, b) => new { tema = a, user = b }).Where(a => a.user.id_liceu == liceu.liceu.id_liceu).ToList();

                    foreach (var t in teme)
                    {
                        var tm = new TemaAModel();
                        tm.data = t.tema.deadline;
                        tm.titlu = t.tema.titlu;

                        var prof = db.Users.Where(a => a.id_user == t.tema.id_prof).FirstOrDefault();
                        tm.prof = prof.nume + " " + prof.prenume;

                        var l = db.Liceus.Where(a => a.id_liceu == prof.id_liceu).FirstOrDefault();
                        tm.liceu = l.nume;

                        var list2 = new List<double>();

                        foreach (var rat in db.Ratings.Where(a => a.id_tema == t.tema.id_tema))
                        {
                            list2.Add(rat.rating1);
                        }

                        if (list2.Count > 0)
                        {
                            var p = list2.Average();
                            tm.rating = p;
                        }
                        else
                        {
                            tm.rating = 0;
                        }

                        tm.id_tema = t.tema.id_tema;

                        model.Add(tm);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

                return View(model);

            }


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
