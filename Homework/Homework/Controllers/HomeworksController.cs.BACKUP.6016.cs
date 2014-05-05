using Homework.Filters;
using Homework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Routing;
using PagedList;
using PagedList.Mvc;

namespace Homework.Controllers {
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeworksController : Controller {

<<<<<<< HEAD
        public bool isProf() {
            return (bool)Session["prof"];
        }

        public int userId() {
            return (int)Session["UserId"];
        }

        public int liceuId() {
            return (int)Session["LiceuId"];
        }

        public ActionResult Teme() {
            using( var db = new HomeworkContext() ) {
=======
       
>>>>>>> origin/master


<<<<<<< HEAD
                var stuff= db.Temas.Where( a => a.id_tema <= 2 ).ToPagedList( 5, 5 );

                return View( model );
=======
        public ActionResult Licee()

        {
            using (var db = new HomeworkContext())
            {
                var model = new LiceeModel();
                model.licee = db.Liceus.ToList();

                return View(model);

>>>>>>> origin/master
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
                   tm.prof = prof.nume + " " + prof.prenume;
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
        

                


        [HttpPost]
        public ActionResult AddComment( SeeHomeworkModel model ) {
            using( var db = new HomeworkContext() ) {
                if( ModelState.IsValid ) {
                    try {
                        var f = new Comentariu();
                        f.data = DateTime.Now;
                        f.id_tema = model.id_tema;
                        f.id_user = (int)Session["UserId"];
                        f.text = model.c.text;


                        db.Comentarius.Add( f );
                        db.SaveChanges();
                    } catch( DbEntityValidationException dbEx ) {
                        foreach( var validationErrors in dbEx.EntityValidationErrors ) {
                            foreach( var validationError in validationErrors.ValidationErrors ) {
                                Trace.TraceInformation( "Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage );
                            }
                        }
                    }
                    return RedirectToAction( "ShowHomework", new RouteValueDictionary( new {
                        controller = "Homeworks",
                        action = "ShowHomework",
                        id_tema = model.id_tema
                    } ) );
                }
                return View( model );
            }
        }
        public ActionResult ShowHomework( int id_tema ) {
            using( var db = new HomeworkContext() ) {
                var m = new SeeHomeworkModel();
                var model = new HomeworkModel();
                var rt = new RatingModel();
                var cm = new CommentModel();

                var tema = db.Temas.Where( t => t.id_tema == id_tema ).FirstOrDefault();
                model.Title = tema.titlu;
                model.Text = tema.enunt;
                model.privat = tema.privat;

                int id_prof = tema.id_prof;
                var nume_prof = db.Users.Where( t => t.id_user == id_prof ).FirstOrDefault();
                model.Professor = nume_prof.nume + " " + nume_prof.prenume;

<<<<<<< HEAD
                var rating = db.Ratings.Where( t => t.id_tema == id_tema ).ToList();

                if( rating.Count == 0 ) {
                    model.rating = 0.0;
                } else {
                    var rat = rating.Average( a => a.rating1 );
                    model.rating = rat;
                }


                if( tema.id_help != null ) {
                    int id_fis = (int)tema.id_help;
                    var fisier = db.Fisiers.Where( a => a.id_fisier == id_fis ).FirstOrDefault();
                    model.help = fisier.cale;
                }
=======
                var rating = db.Ratings.Where(t => t.id_tema == id_tema).ToList();
                var rat = 0.0;
                if (rating.Count > 0)
                { rat = rating.Average(a => a.rating1); }

                model.rating = rat;

                model.help = tema.id_help;
                
                model.in_out = tema.id_in_out;
>>>>>>> origin/master


                model.comentariu = new List<CommentModel>();
                var lista_com = db.Comentarius.Where( a => a.id_tema == id_tema ).OrderBy( a => a.data ).ToList();
                foreach( var c in lista_com ) {
                    CommentModel com = new CommentModel();
                    com.data = c.data;
                    com.text = c.text;
                    var sel = db.Users.Where( t => t.id_user == c.id_user ).FirstOrDefault();
                    com.username = sel.nume + "  " + sel.prenume;
                    model.comentariu.Add( com );
                }
                model.current_grade = 0; // ------------------------- Aici e harcodat
<<<<<<< HEAD
                // Session["user_id"] = 1; // ------------------------- Aici e harcodat
                model.grade = db.Submits.Where( a => a.id_user == 1 ).FirstOrDefault().rezultat;
=======
                var id = (int)Session["UserId"]; 
               
                var nota= db.Submits.Where(c => (c.id_user == id && c.id_tema == id_tema)).OrderByDescending(c => c.rezultat).FirstOrDefault();
                if (nota != null)
                {
                    model.grade = nota.rezultat;
                }
                else
                {
                    model.grade = 0;
                }
>>>>>>> origin/master
                model.id_tema = id_tema;
                m.Hm = model;
                m.r = rt;
                m.c = cm;
                m.id_tema = id_tema;
                return View( m );


            }
        }
<<<<<<< HEAD

        public ActionResult ArhivaTeme() {
            using( var db = new HomeworkContext() ) {
=======
        public ActionResult ArhivaTeme()
        {
            using (var db = new HomeworkContext())
            {
>>>>>>> origin/master
                var model = new List<TemaAModel>();

                foreach( var t in db.Temas.Where( a => (a.deadline < DateTime.Now && a.privat == 0) ) ) {
                    var tm = new TemaAModel();
                    tm.data = t.deadline;
                    tm.titlu = t.titlu;

                    var prof = db.Users.Where( a => a.id_user == t.id_prof ).FirstOrDefault();
                    tm.prof = prof.nume + " " + prof.prenume;

                    var l = db.Liceus.Where( a => a.id_liceu == prof.id_liceu ).FirstOrDefault();
                    tm.liceu = l.nume;

                    var list2 = new List<double>();
                    foreach( var rat in db.Ratings.Where( a => a.id_tema == t.id_tema ) )
                        list2.Add( rat.rating1 );

                    if( list2.Count > 0 ) {
                        var p = list2.Average();
                        tm.rating = p;
                    } else {
                        tm.rating = 0;
                    }

                    tm.id_tema = t.id_tema;

                    model.Add( tm );
                }

                return View( model );
            }
        }


        public FileResult Download(int id_submit)
        {
            using (var db = new HomeworkContext())
            {
                var submit = db.Submits.Where(a => a.id_submit == id_submit).FirstOrDefault();
                var id_sursa = submit.id_sursa;
                var file = db.Fisiers.Where(a => a.id_fisier == id_sursa).FirstOrDefault();
                string path = file.cale;

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = System.IO.Path.GetFileName(path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        public ActionResult Sources(int id_tema)
        {
            using (var db = new HomeworkContext())
            {
                var model = new List <SourceModel> ();
                var rez = db.Submits.Where(a => a.id_tema == id_tema).ToList();
                foreach (var c in rez)
                {
                    SourceModel source = new SourceModel();
                    source.result = c.rezultat;
                    var user = db.Users.Where(a => a.id_user == c.id_user).FirstOrDefault();
                    source.username = user.nume + " " + user.prenume;
                    source.id_source = c.id_sursa;
                    source.id_submit = c.id_submit;
                    model.Add(source);
                }
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult AddHomework() {

<<<<<<< HEAD
            if( !(bool)Session["prof"] ) {
=======
            if( !(bool)Session ["isProf"] ) {
>>>>>>> origin/master
                //TO DO: De pus 'Index.cshtml' la shared ?
                return View( "~/Views/Home/Index.cshtml" );
            }

            ViewBag.Title = "Creeaza Tema";
            return View();
        }

        [HttpPost]

        [Authorize]



   
        public ActionResult AddHomework(AddHomeworkModel model)
        {
            //Unlikely event
<<<<<<< HEAD
            if( !(bool)Session["prof"] ) {
=======
            if (!(bool)Session["prof"])
            {
>>>>>>> origin/master
                //TO DO: De pus 'Index.cshtml' la shared ?
                return View("~/Views/Home/Index.cshtml");
            }
            ViewBag.Title = "Creeaza Tema";
            using (var db = new HomeworkContext())
            {
                var tema = new Tema();
                tema.deadline = model.deadline;
                tema.enunt = model.enunt;
                var fisierInOut = new Fisier();
                var fisierHelp = new Fisier();
                var fileName = Path.GetFileName(model.in_out.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                model.in_out.SaveAs(path);
                fisierInOut.cale = path;
                db.Fisiers.Add(fisierInOut);
                tema.Fisier = fisierInOut;
                tema.id_in_out = fisierInOut.id_fisier;
                if (model.help != null)
                {
                    fileName = Path.GetFileName(model.help.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    model.help.SaveAs(path);
                    fisierHelp.cale = path;
                    db.Fisiers.Add(fisierHelp);
                    tema.Fisier1 = fisierHelp;
                    tema.id_help = fisierHelp.id_fisier;
                }
                else
                {
                    tema.Fisier1 = null;
                    tema.id_help = null;
                }
<<<<<<< HEAD

=======
>>>>>>> origin/master
                tema.id_prof = (int)Session["UserId"];
                tema.privat = model.privat == true ? 1 : 0;
                tema.titlu = model.title;
<<<<<<< HEAD

                db.Temas.Add( tema );

                string classes = model.clasa.Replace( " ", "" ).Replace( ",", "" );

                int idLiceu = (int)Session["LiceuId"];

                var users = db.Users.Where( a => a.an_studiu == model.an && classes.Contains( a.clasa ) && a.id_liceu == idLiceu && a.tip == 1 ).ToList();

=======
                db.Temas.Add(tema);
                string classes = model.clasa.Replace(" ", "").Replace(",", "");
                int idLiceu = (int)Session["LiceuId"];
                var users = db.Users.Where(a => a.an_studiu == model.an && classes.Contains(a.clasa) && a.id_liceu == idLiceu && a.tip == 1).ToList();
>>>>>>> origin/master
                //TO DO: Un 'bulk insert'
                foreach (var user in users)
                {
                    db.Participas.Add(new Participa
                    {
                        id_tema = tema.id_tema,
                        id_user = user.id_user
                    });
                }
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


        public ActionResult ProfesoriLiceu(int idd_liceu)
        {
            using (var db = new HomeworkContext())
            {

                var profi = new List<ProfesoriModel>();
                foreach (var u in (db.Users.Where(a => a.tip == 2 && a.id_liceu == idd_liceu).ToList()))
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

                    var list2 = new List<double>();
                    var list = new List<double>();

                    foreach (var tema in db.Temas.Where(a => a.id_prof == id_prof))
                    {

                        foreach (var rat in db.Ratings.Where(r => r.id_tema == tema.id_tema))
                            list2.Add(rat.rating1);
                        if (list2.Count() != 0)
                        {
                            list.Add(list2.Average());
                        }
                        else
                        {
                            list.Add(0);
                        }
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
        public ActionResult Profesori(SearchModel model)
        {
            using (var db = new HomeworkContext())
            {

                var profi = new List<ProfesoriModel>();
                foreach (var u in (db.Users.Where(a => (a.nume + a.prenume).Contains(model.NumeProfesor)).ToList()))
                    if (u.tip == 2)
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

                            if(list2.Count() != 0) {
                                list.Add(list2.Average());
                            }
                            else
                            {
                                list.Add(0);
                            }
                             
                        }

                        if (list.Count() == 0) prof.rating = 0;
                        else
                            prof.rating = list.Average();

                        profi.Add(prof);
                    }

                return View(profi);
            }

        }


        [HttpGet]
        public ActionResult ChangeInfo() {
            ViewBag.Title = "Creeaza Tema";
            var model = new ChangeInfo();

            using( var db = new HomeworkContext() ) {
                var user = new User();
                var id = (int)Session["UserId"];
                user = db.Users.Where( a => a.id_user == id ).FirstOrDefault();

                model.nume = user.nume;
                model.prenume = user.prenume;
                model.email = user.email;
                model.parola = user.parola;
                model.clasa = user.clasa;
                model.anStudiu = (int)user.an_studiu;

            }

            return View( model );


        }

        [HttpPost]
        public ActionResult ChangeInfo( ChangeInfo model ) {
            ViewBag.Title = "Creeaza Tema";

            using( var db = new HomeworkContext() ) {
                var user = new User();
                var id = (int)Session["UserId"];
                user = db.Users.Where( a => a.id_user == id ).FirstOrDefault();

                user.nume = model.nume;
                user.prenume = model.prenume;
                user.email = model.email;
                user.parola = model.parola;
                user.clasa = model.clasa;
                user.an_studiu = model.anStudiu;

                db.SaveChanges();

            }

            return View();

        }


        [HttpGet]
        public ActionResult TemeleMele() {
            using( var db = new HomeworkContext() ) {
                var model = new List<TemaAModel>();
<<<<<<< HEAD

                var id = (int)Session["UserId"];
                bool isProf = (bool)Session["prof"];

=======
                var id = (int)Session ["UserId"];
                bool isProf = (bool)Session ["isProf"];
>>>>>>> origin/master
                var user = db.Users.Where( a => a.id_user == id ).FirstOrDefault();
                //TO DO: De scris metode pt isProf is userId
                if( isProf ) {
                    var teme = db.Temas.Where( a => a.id_prof == id ).ToList();
                    foreach( var t in teme ) {
                        var tm = new TemaAModel();
                        tm.data = t.deadline;
                        tm.titlu = t.titlu;
                        var prof = db.Users.Where( a => a.id_user == t.id_prof ).FirstOrDefault();
                        tm.prof = prof.nume + " " + prof.prenume;
                        var l = db.Liceus.Where( a => a.id_liceu == prof.id_liceu ).FirstOrDefault();
                        tm.liceu = l.nume;
                        var list2 = new List<double>();
                        foreach( var rat in db.Ratings.Where( a => a.id_tema == t.id_tema ) ) {
                            list2.Add( rat.rating1 );
                        }
                        if( list2.Count > 0 ) {
                            var p = list2.Average();
                            tm.rating = p;
                        } else {
                            tm.rating = 0;
                        }
                        tm.id_tema = t.id_tema;
                        model.Add( tm );
                    }
                } else {
                    var teme = db.Temas.Join( db.Participas, a => a.id_tema, b => b.id_tema, ( a, b ) => new {
                        user = b,
                        tema = a
                    } ).Where( a => a.user.id_user == id ).ToList();
                    foreach( var t in teme ) {
                        var tm = new TemaAModel();
                        tm.data = t.tema.deadline;
                        tm.titlu = t.tema.titlu;
                        var prof = db.Users.Where( a => a.id_user == t.tema.id_prof ).FirstOrDefault();
                        tm.prof = prof.nume + " " + prof.prenume;
                        var l = db.Liceus.Where( a => a.id_liceu == prof.id_liceu ).FirstOrDefault();
                        tm.liceu = l.nume;
                        var list2 = new List<double>();
                        foreach( var rat in db.Ratings.Where( a => a.id_tema == t.tema.id_tema ) ) {
                            list2.Add( rat.rating1 );
                        }
                        if( list2.Count > 0 ) {
                            var p = list2.Average();
                            tm.rating = p;
                        } else {
                            tm.rating = 0;
                        }
                        tm.id_tema = t.tema.id_tema;
                        model.Add( tm );
                    }
                }
                return View( model );
            }
        }


        public ActionResult SeeHomework()
        {
            return View();
        }




        public ActionResult VeziNote(int id_tema)
        {
            using (var db = new HomeworkContext())
            {
                var model = new List<NotaModel>();

                var participanti  = new List<int>();
                foreach (var p in db.Participas.Where(a => a.id_tema == id_tema))
                {
                    participanti.Add(p.id_user);
                }

                foreach (var submit in db.Submits.Where(a => a.id_tema == id_tema).GroupBy(b => b.id_user))
                {
                    var nota = new NotaModel();
                    int user_id = submit.Key;
                    if (participanti.Contains(user_id))
                    {
                        var name = db.Users.Where(c => c.id_user == user_id).FirstOrDefault();
                        nota.Nume = name.nume + " " + name.prenume;
                        nota.An = (int)name.an_studiu;
                        nota.Clasa = name.clasa;
                        nota.Nota = db.Submits.Where(c => (c.id_user == user_id && c.id_tema == id_tema)).OrderByDescending(c => c.rezultat).First().rezultat;

                        model.Add(nota);
                    }

                }

                return View(model);
            }
        }

      
    }

}

