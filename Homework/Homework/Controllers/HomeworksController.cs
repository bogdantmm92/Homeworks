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

namespace Homework.Controllers {
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeworksController : Controller {

        public ActionResult Teme() {
            using( var db = new HomeworkContext() ) {
                var model = new TemeModel();
                model.teme = db.Temas.Where( a => a.id_tema <= 2 ).ToList();

                return View( model );
            }
        }

        [HttpGet]
        public ActionResult AddHomework() {

            if( !(bool)Session ["prof"] ) {
                //TO DO: De pus 'Index.cshtml' la shared ?
                return View( "~/Views/Home/Index.cshtml" );
            }

            ViewBag.Title = "Creeaza Tema";
            return View();
        }

        [HttpPost]
        public ActionResult AddHomework( AddHomeworkModel model ) {

            //Unlikely event
            if( !(bool)Session ["prof"] ) {
                //TO DO: De pus 'Index.cshtml' la shared ?
                return View( "~/Views/Home/Index.cshtml" );
            }

            ViewBag.Title = "Creeaza Tema";
            using( var db = new HomeworkContext() ) {
                var tema = new Tema();

                tema.deadline = model.deadline;
                tema.enunt = model.enunt;

                var fisierInOut = new Fisier();
                var fisierHelp = new Fisier();

                var fileName = Path.GetFileName( model.in_out.FileName );
                var path = Path.Combine( Server.MapPath( "~/App_Data/uploads" ), fileName );
                model.in_out.SaveAs( path );

                fisierInOut.cale = path;

                db.Fisiers.Add( fisierInOut );

                tema.Fisier = fisierInOut;
                tema.id_in_out = fisierInOut.id_fisier;

                if( model.help != null ) {

                    fileName = Path.GetFileName( model.help.FileName );
                    path = Path.Combine( Server.MapPath( "~/App_Data/uploads" ), fileName );
                    model.help.SaveAs( path );

                    fisierHelp.cale = path;


                    db.Fisiers.Add( fisierHelp );

                    tema.Fisier1 = fisierHelp;
                    tema.id_help = fisierHelp.id_fisier;

                } else {
                    tema.Fisier1 = null;
                    tema.id_help = null;
                }

                tema.id_prof = (int)Session ["UserId"];
                tema.privat = model.privat == true ? 1 : 0;

                tema.titlu = model.title;

                db.Temas.Add( tema );

                string classes = model.clasa.Replace( " ", "" ).Replace( ",", "" );

                int idLiceu = (int)Session ["LiceuId"];

                var users = db.Users.Where( a => a.an_studiu == model.an && classes.Contains( a.clasa ) && a.id_liceu == idLiceu && a.tip == 1 ).ToList();

                //TO DO: Un 'bulk insert'
                foreach( var user in users ) {
                    db.Participas.Add( new Participa {
                        id_tema = tema.id_tema,
                        id_user = user.id_user
                    } );
                }

                try {
                    db.SaveChanges();
                } catch( DbEntityValidationException e ) {

                    foreach( var eve in e.EntityValidationErrors ) {
                        Debug.WriteLine( "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State );
                        foreach( var ve in eve.ValidationErrors ) {
                            Debug.WriteLine( "- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage );
                        }
                    }
                    throw;
                }
            }

            return View();


        }

        [HttpGet]
        public ActionResult ChangeInfo() {
            ViewBag.Title = "Creeaza Tema";
            var model = new ChangeInfo();

            using( var db = new HomeworkContext() ) {
                var user = new User();
                var id = (int)Session ["UserId"];
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
                var id = (int)Session ["UserId"];
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

                var id = (int)Session ["UserId"];
                bool isProf = (bool)Session ["prof"];

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

    }

}
