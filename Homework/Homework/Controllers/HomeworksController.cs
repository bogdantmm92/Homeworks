using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework.Controllers
{
    [Authorize]
    public class HomeworksController : Controller
    {

       

        public ActionResult Licee()
        {
            using (var db = new HomeworkContext())
            {
                var model = new LiceeModel();
                model.licee = db.Liceus.ToList();

                return View(model);

            }
        }

        public ActionResult ListaTeme(int id_prof)
        {
            using (var db = new HomeworkContext())
            {
                var model = new List<TemaModel>();
                foreach (var t in db.Temas.Where(a => (a.id_prof == id_prof)))
                {
                    var tm = new TemaModel();
                    tm.data = t.deadline;
                    tm.titlu = t.titlu;
                    var prof = db.Users.Where(a => a.id_user == t.id_prof).FirstOrDefault();
                    var list2 = new List<double>();
                    foreach (var rat in db.Ratings.Where(a => a.id_tema == t.id_tema))
                        list2.Add(rat.rating1);
                    if (list2.Count > 0)
                    {
                        var p = list2.Average();
                        tm.rating = p;
                    }
                    else
                    { tm.rating = 0; }
                    tm.id_tema = t.id_tema;
                    model.Add(tm);
                }
                return View(model);
            }
        }

        public ActionResult ProfesoriLiceu(int idd_liceu)
        {
            using (var db = new HomeworkContext())
            {
              
                var profi = new List<ProfesoriModel>();
                foreach (var u in (db.Users.Where(a => a.tip==2 && a.id_liceu == idd_liceu).ToList()))
                   
                {
                    var prof = new ProfesoriModel();
                    int id_liceu = u.id_liceu;
                    int id_prof = u.id_user;

                    prof.id_prof =u.id_user;
                    prof.nume = u.nume;
                    prof.prenume = u.prenume;
                    prof.numar_teme = db.Temas.Where(a => a.id_prof == id_prof).Count();

                    foreach (var liceu in (db.Liceus.Where(a => a.id_liceu == id_liceu)).ToList())
                    {
                        prof.liceu = liceu.nume;
                    }

                    var list2 = new List<double>();
                    var list = new List<double>();

                    foreach (var tema in db.Temas.Where(a => a.id_prof == id_prof))
                    {

                        foreach (var rat in db.Ratings.Where(r => r.id_tema == tema.id_tema))
                            list2.Add(rat.rating1);
                            list.Add(list2.Average());       
                    }

                    if (list.Count() == 0) prof.rating = 0;
                    else
                        prof.rating = list.Average();
                                  
                    profi.Add(prof);
                }

                return View(profi);
            }

        }



        [HttpPost]
        public ActionResult Profesori(ProfesoriModel model)
        {
            using (var db = new HomeworkContext())
            {
              
                var profi = new List<ProfesoriModel>();
                foreach (var u in (db.Users.Where(a =>(a.nume + a.prenume).Contains(model.NumeProfesor)).ToList()))
                    if (u.tip==2)
                {
                    var prof = new ProfesoriModel();
                    int id_liceu = u.id_liceu;
                    int id_prof = u.id_user;

                    prof.id_prof = u.id_user;
                    prof.nume = u.nume;
                    prof.prenume = u.prenume;
                    prof.numar_teme = db.Temas.Where(a => a.id_prof == id_prof).Count();

                    foreach (var liceu in (db.Liceus.Where(a => a.id_liceu == id_liceu)).ToList())
                    {
                        prof.liceu = liceu.nume;
                    }

                    var list2 = new List<int>();
                    var list = new List<double>();

                    foreach (var tema in db.Temas.Where(a => a.id_prof == id_prof))
                    {

                        foreach (var rat in db.Ratings.Where(r => r.id_tema == tema.id_tema))
                            list2.Add(rat.rating1);
                            list.Add(list2.Average());       
                    }

                    if (list.Count() == 0) prof.rating = 0;
                    else
                        prof.rating = list.Average();
                                  
                    profi.Add(prof);
                }

                return View(profi);
            }

        }
        

    }
}