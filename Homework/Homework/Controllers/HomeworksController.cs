﻿using Homework.Filters;
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
using PagedList.Mvc;
using PagedList;
using Homework.Utils;
using System.Text;

namespace Homework.Controllers {
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeworksController : Controller {

        public bool isProf() {
            return (bool)Session["prof"];
        }

        public int userId() {
            return (int)Session["UserId"];
        }

        public int liceuId() {
            return (int)Session["LiceuId"];
        }

        /*
        public ActionResult Licee(int ? page)

        {
            using (var db = new HomeworkContext())
            {
                var model = new LiceeModel();
                List <Homework.Liceu> l = db.Liceus.ToList();
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                model.licee = new PagedList<Liceu>(l, pageNumber, pageSize);
                //model.licee = db.Liceus.ToList();

                return View(model);

            }
        }
        */

       public ActionResult ListaTeme(int id_prof, string sort, int ? page)
       {
           using (var db = new HomeworkContext())
           {
               var model = new List<TemaModel>();
               var model2 = new TemeProfModel();
               foreach (var t in db.Temas.Where(a => (a.id_prof == id_prof)))
               {
                   var tm = new TemaModel();
                   tm.data = t.deadline;
                   tm.titlu = t.titlu;
                   var prof = db.Users.Where(a => a.id_user == t.id_prof).FirstOrDefault();
                   tm.prof = prof.nume + "  " + prof.prenume;
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
                              
               var model3 = new TemeProfModel();
               model3.id_prof = id_prof;
               var mo = new List<TemaModel>();

               if (sort == "Dupa rating")
               {
                   var ceva = model.OrderByDescending(m => m.rating).ToList();
                   foreach (var t in ceva)
                   {
                       mo.Add(t);
                   }
               }
               else
               {
                   var ceva = model.OrderBy(m => m.data).ToList();
                   foreach (var t in ceva)
                   {
                       mo.Add(t);
                   }
               }

               int pageSize = 5;
               int pageNumber = (page ?? 1);
               var PagedModel = mo.ToPagedList(pageNumber, pageSize);
               model3.teme = (PagedList<TemaModel>)PagedModel;

               return View(model3);
           }
       }


       public ActionResult Sorteaza(string Sorting_Order, int? page)
       {
           using (var db = new HomeworkContext())
           {
               if (String.IsNullOrEmpty(Sorting_Order)) Sorting_Order = "Alfabetic";

              /* ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Alfabetic" : "";
               ViewBag.SortingRating = Sorting_Order == "Dupa rating";*/


               var model = new List<LiceuModel>();
             

               if (Sorting_Order == "Rating")
                   foreach (var liceu in db.Liceus.OrderByDescending(m => m.rating_total))
                   {
                       var l = new LiceuModel();
                       l.nume = liceu.nume;
                       l.rating_total = liceu.rating_total;
                       l.id_liceu = liceu.id_liceu;
                       model.Add(l);
               }
                   
               else 
                 foreach(var liceu in db.Liceus.OrderBy(m => m.nume))
               {
                   var l = new LiceuModel();
                   l.nume = liceu.nume;
                   l.rating_total = Math.Truncate(liceu.rating_total * 100) / 100;
                   l.id_liceu = liceu.id_liceu;
                   model.Add(l);
               }

               int pageSize = 5;
               int pageNumber = (page ?? 1);
               return View(model.ToPagedList(pageNumber, pageSize));
               //return View(model);
                      
               }
               
           }

        /*
       private ActionResult View(LiceeModel model, string Sorting_Order)
       {
           throw new NotImplementedException();
       }
        */

        [HttpGet]
       public ActionResult CompileError(string errorMsg)
       {
           var model = new CompileErrorModel
           {
               error = errorMsg
           };
           return View(model);
       }
       [HttpPost]
       public ActionResult AddComment(SeeHomeworkModel model)
       {
           using (var db = new HomeworkContext())
           {
              // if (ModelState.IsValid)
               {
                   try
                   {
                       var f = new Comentariu();
                       f.data = System.DateTime.Now;
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
                    return RedirectToAction("ShowHomework", new RouteValueDictionary(new
                    {
                        controller = "Homeworks",
                        action = "ShowHomework",
                        id_tema = model.id_tema
                    } ) );
                }
               // return View( model );
            }
        }

        [HttpPost]
        public ActionResult SubmitSource(SeeHomeworkModel model)
        {
            int id_tema = (int)Session["id_tema"];

            //Should not happen
            if( (bool)Session["prof"] ) {
                return View( "~/Views/Home/Index.cshtml" );
            }
            using (var db = new HomeworkContext())
            {
                var defaultDirectory = Server.MapPath("~/App_Data/uploads");
                var sourceDirectory = String.Format("source_codes/homework{0}/{1}", id_tema, userId());
                var sourceFileName = String.Format("submission{0}.txt", db.Submits.Count());

                var sourceRelativePath = Path.Combine(sourceDirectory, sourceFileName);

                var sourceDirectoryPath = Path.Combine(defaultDirectory, sourceDirectory);
                var sourcePath = Path.Combine(defaultDirectory, sourceRelativePath);

                System.IO.Directory.CreateDirectory(sourceDirectoryPath);
                model.source_code.SaveAs(sourcePath);

                var sourceFile = new Fisier();
                sourceFile.cale = sourceRelativePath;
                db.Fisiers.Add(sourceFile);

                //Source code
                string sourceCode;
                using (StreamReader sr = new StreamReader(sourcePath))
                {
                    sourceCode = sr.ReadToEnd();
                }

                //Input output
                var inOutRelDir = Path.Combine("input_output", String.Format("homework{0}", id_tema));
                var inputOutputDir = Path.Combine(defaultDirectory, inOutRelDir);
                string[] files = Directory.GetFiles(inputOutputDir, "*.txt");
               
                //input
                List<string> inputs = new List<string>();
                //output
                List<string> outputs = new List<string>();
                foreach (string filePath in files)
                {
                    string text = string.Empty;
                    using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    if (filePath.Contains("_input_"))
                        inputs.Add(text);
                    if (filePath.Contains("_output_"))
                        outputs.Add(text);
                }
                List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
                int points = 0;
                for (var i = 0; i < inputs.Count(); i ++)
                {
                    var input = inputs[i];
                    var result = SubmissionHelper._Instance.uploadSource(sourceCode, input);
                    results.Add(result);
                    var output = result["output"];
                    if (result["cmpinfo"] != "")
                    {
                        return RedirectToAction("CompileError", new RouteValueDictionary(new { controller = "Homeworks", action = "CompileError", errorMsg = result["cmpinfo"]}));
                    }
                    if (output == outputs[i])
                        points++;
                }
                db.SaveChanges();
                Submit submit = new Submit();
                submit.Fisier = sourceFile;
                submit.id_tema = id_tema;
                submit.id_user = userId();
                submit.rezultat = points * 100 / outputs.Count();
                submit.data = System.DateTime.Now;
                db.Submits.Add(submit);
                db.SaveChanges();

                return RedirectToAction("Sources", new RouteValueDictionary(new { controller = "Homeworks", action = "Sources", id_tema = id_tema }));

            }
        }

        [HttpPost]
        public ActionResult SeeStudents2(SeeHomeworkModel model, string[] tags)
        {

            if (tags == null)
            {
                return View("Index");
            }

            using (var db = new HomeworkContext())
            {
                foreach (var tag in tags)
                {
                    int id = int.Parse(tag);

                    db.Participas.Add(new Participa
                    {
                        id_tema = model.id_tema,
                        id_user = id

                    });

                        db.SaveChanges();

                }

            }

            return RedirectToAction("ShowHomework", new RouteValueDictionary(new { controller = "Homeworks", action = "ShowHomework", id_tema = model.id_tema }));

           // return View("~/Views/Home/Index.cshtml");

        }


        [HttpPost]
        public ActionResult SeeStudents(SeeHomeworkModel model)
        {
            using (var db = new HomeworkContext())
            {
                string classes = model.clase.Replace(" ", "").Replace(",", "");
                int idLiceu = (int)Session["LiceuId"];
                var users = db.Users.Where(a => a.an_studiu == model.an && classes.Contains(a.clasa) && a.id_liceu == idLiceu && a.tip == 1).ToList();

                List<Elev> lista = new List<Elev>();
                foreach (var e in users)
                {
                    Elev p = new Elev();
                    p.nume = e.nume;
                    p.prenume = e.prenume;
                    p.id_elev = e.id_user;
                    p.isChecked = false;
                    lista.Add(p);
                }
                model.elev = lista;
            }

            return View(model);

        }

        /*
        [HttpPost]
        public ActionResult ShowHomework(SeeHomeworkModel model)
        {
            using (var db = new HomeworkContext())
            {
                if (model.Show)
                {
                    string classes = model.clase.Replace(" ", "").Replace(",", "");
                    int idLiceu = (int)Session["LiceuId"];
                    var users = db.Users.Where(a => a.an_studiu == model.an && classes.Contains(a.clasa) && a.id_liceu == idLiceu && a.tip == 1).ToList();

                    List<Elev> lista = new List<Elev>();
                    foreach(var e in users)
                    {
                        Elev p = new Elev();
                        p.nume = e.nume;
                        p.prenume = e.prenume;
                        p.id_elev = e.id_user;
                        p.isChecked = false;
                        lista.Add(p);
                    }
                    model.elev = lista;
                    ViewBag.Show = true;
                    return RedirectToAction("ShowHomework", new RouteValueDictionary(new { controller = "Homeworks", action = "ShowHomework", id_tema = model.id_tema }));

                }
                else
                {
                    string classes = model.clase.Replace(" ", "").Replace(",", "");
                    int idLiceu = (int)Session["LiceuId"];
                    var users = db.Users.Where(a => a.an_studiu == model.an && classes.Contains(a.clasa) && a.id_liceu == idLiceu && a.tip == 1).ToList();
                    foreach (var user in users)
                    {
                        db.Participas.Add(new Participa
                        {
                            id_tema = model.id_tema,
                            id_user = user.id_user
                        });
                    }

                    try
                    {
                        db.SaveChanges();
                        return RedirectToAction("ShowHomework", new RouteValueDictionary(new { controller = "Homeworks", action = "ShowHomework", id_tema = model.id_tema }));

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
            }

            return View(model);
        }
        */

        public ActionResult ShowHomework(int id_tema, int? page)
        {
            Session["id_tema"] = id_tema;
            using (var db = new HomeworkContext())
            {
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
                m.id_prof = id_prof;

                var rating = db.Ratings.Where( t => t.id_tema == id_tema ).ToList();

                if( rating.Count == 0 ) {
                    model.rating = 0.0;
                } else {
                    var rat = rating.Average( a => a.rating1 );
                    model.rating = rat;
                }


                
                 model.help = tema.id_help;
                
                model.in_out = tema.id_in_out;


                //model.comentariu = new List<CommentModel>();
                var comm = new List<CommentModel>();

                var lista_com = db.Comentarius.Where(a => a.id_tema == id_tema).OrderBy(a => a.data).ToList();
                foreach (var c in lista_com)
                {
                    CommentModel com = new CommentModel();
                    com.data = c.data;
                    com.text = c.text;
                    var sel = db.Users.Where( t => t.id_user == c.id_user ).FirstOrDefault();
                    com.username = sel.nume + "  " + sel.prenume;
                    comm.Add(com);

                    //model.comentariu.Add( com );

                }
                model.current_grade = 0; // ------------------------- Aici e harcodat

                 //Session["user_id"] = 1; // ------------------------- Aici e harcodat
                var id = (int)Session["UserId"];
                if ((bool)Session["prof"] == false)
                {
                    var ceva = db.Submits.Where(a => a.id_user == id).FirstOrDefault();
                    if (ceva != null)
                    { model.grade = ceva.rezultat; }
                    else
                    { model.grade = 0; }
                }


               
                var nota= db.Submits.Where(c => (c.id_user == id && c.id_tema == id_tema)).OrderByDescending(c => c.rezultat).FirstOrDefault();
                if (nota != null)
                {
                    model.grade = nota.rezultat;
                }
                else
                {
                    model.grade = 0;
                }

				

                model.id_tema = id_tema;
                m.Hm = model;
                m.r = rt;
                m.c = cm;
                m.id_tema = id_tema;
                //m.Show = false;

                //verific daca am mai votat
                Session["votat"] = "nu";
                int user_id = userId();
                var r = db.Ratings.Where(a => (a.id_tema == id_tema && a.id_user == user_id)).FirstOrDefault();
                if (r != null)
                    Session["votat"] = "da";

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                model.comentariu = new PagedList<CommentModel>(comm, pageNumber, pageSize);

                return View( m );

            }
        }

        public ActionResult ArhivaTeme(int ? page)
        {
            using (var db = new HomeworkContext())
            {
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

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
                //return View(model);
            }
        }

        public FileResult DownloadIn_Out( int id_io ) {
            using( var db = new HomeworkContext() ) {
                var tema = db.Temas.Where( a => a.id_in_out == id_io ).FirstOrDefault();
                var id_tema = tema.id_in_out;

                var file = db.Fisiers.Where( a => a.id_fisier == id_tema ).FirstOrDefault();

                var defaultDirectory = Path.Combine( "input_output", String.Format( "homework{0}", id_tema ) );
                string path = Path.Combine( defaultDirectory, file.cale );

                byte[] fileBytes = System.IO.File.ReadAllBytes( path );
                string fileName = System.IO.Path.GetFileName( path );
                return File( fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "input_output" + " - " + tema.titlu + ".zip" );
            }
        }

        public FileResult Download(int id_submit)
        {
            using (var db = new HomeworkContext())
            {
                var submit = db.Submits.Where(a => a.id_submit == id_submit).FirstOrDefault();
                var id_sursa = submit.id_sursa;
                var file = db.Fisiers.Where(a => a.id_fisier == id_sursa).FirstOrDefault();
                var defaultDirectory = Server.MapPath("~/App_Data/uploads");
                string path = Path.Combine(defaultDirectory, file.cale);

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
                var t = db.Temas.Where(a => a.id_tema == id_tema).FirstOrDefault();
                ViewBag.titlu = t.titlu;
                foreach (var c in rez)
                {
                    SourceModel source = new SourceModel();
                    source.result = c.rezultat;
                    var user = db.Users.Where(a => a.id_user == c.id_user).FirstOrDefault();
                   
                    source.username = user.nume + " " + user.prenume;
                    source.id_source = c.id_sursa;
                    source.id_submit = c.id_submit;
                    source.data = (DateTime)c.data;
                   // source.titlu = t.titlu;
                    
                    model.Add(source);
                }
                return View(model);
            }
        }



        [HttpGet]
        public ActionResult AddHomework() {


            if( !(bool)Session["prof"] ) {

                //TO DO: De pus 'Index.cshtml' la shared ?
                //return View( "~/Views/Home/Index.cshtml" );
                return RedirectToAction("Index","Home");
            }

            ViewBag.Title = "Creeaza Tema";
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHomework(AddHomeworkModel model)
        {
            //Unlikely event
            if( !(bool)Session["prof"] ) {

                //TO DO: De pus 'Index.cshtml' la shared ?
                return RedirectToAction("Index", "Home");
               // return View("~/Views/Home/Index.cshtml");
            }
            ViewBag.Title = "Creeaza Tema";
            using (var db = new HomeworkContext())
            {
                var tema = new Tema();
                tema.deadline = model.deadline;
                tema.enunt = model.enunt;
                var fisierInOut = new Fisier();
                var fisierHelp = new Fisier();
                var fileName = Path.GetFileName(model.in_out.FileName) + "_" + db.Fisiers.Count();
                var inputOutputPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = inputOutputPath;
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

                tema.id_prof = (int)Session["UserId"];
                tema.privat = model.privat == true ? 1 : 0;
                tema.titlu = model.title;

                string classes = model.clasa.Replace( " ", "" ).Replace( ",", "" );

                int idLiceu = (int)Session["LiceuId"];

                var users = db.Users.Where( a => a.an_studiu == model.an && classes.Contains( a.clasa ) && a.id_liceu == idLiceu && a.tip == 1 ).ToList();

                db.Temas.Add(tema);
                db.SaveChanges();

                //Decompress input-output
                var defaultDirectory = Server.MapPath("~/App_Data/uploads");
                var sourceDirectory = Path.Combine("input_output", String.Format("homework{0}", tema.id_tema));

                var sourceDirectoryPath = Path.Combine(defaultDirectory, sourceDirectory);
                System.IO.Directory.CreateDirectory(sourceDirectoryPath);

                ZipUtil.unzip(inputOutputPath, sourceDirectoryPath);
                
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


        public ActionResult ProfesoriLiceu(int idd_liceu, int ? page)
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
                        prof.id_liceu = liceu.id_liceu;
                    }
                    ViewBag.liceu = prof.liceu;

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
                        prof.rating = Math.Truncate(list.Average() * 100) / 100;



                    profi.Add(prof);
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(profi.ToPagedList(pageNumber, pageSize));
                //return View(profi);
            }

        }

        

        [HttpPost]
        public ActionResult Profesori(ProfesoriModel model)
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
                            prof.id_liceu = liceu.id_liceu;
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

        /*
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
        */

        /*
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
        */

        [HttpGet]
        public ActionResult TemeleMele(int? page) {
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

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
                //return View( model );
            }
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


        [HttpPost]
        public ActionResult AddRating(SeeHomeworkModel model, int id_tema)
        {
            using (var db = new HomeworkContext())
            {
                

                //if (ModelState.IsValid)
                {
                    try
                    {
                        var r = new Rating();
                        r.id_tema = id_tema;
                        r.id_user = (int)Session["UserId"];
                        r.rating1 = model.rating;

                        db.Ratings.Add(r);
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

                    //calculez rating liceu
                    var tm = db.Temas.Where(a => a.id_tema == id_tema).FirstOrDefault();
                    var user = db.Users.Where(a => a.id_user == tm.id_prof).FirstOrDefault();
                    var liceu = db.Liceus.Where(a => a.id_liceu == user.id_liceu).FirstOrDefault();

                    var list2 = new List<int>();
                    var list = new List<double>();
                    var list3 = new List<double>();

                    foreach(var prof in db.Users.Where(a => a.id_liceu == liceu.id_liceu))
                    {
                        foreach (var tema in db.Temas.Where(a => a.id_prof == prof.id_user))
                        {
                            foreach (var rat in db.Ratings.Where(r => r.id_tema == tema.id_tema))
                                list2.Add(rat.rating1);

                            if (list2.Count() != 0)
                            {
                                list.Add(list2.Average());
                            }                           
                        }

                        if (list.Count() != 0)
                            list3.Add(list.Average());
                    }


                    liceu.rating_total = list3.Average();
                    db.SaveChanges();


                    return RedirectToAction("ShowHomework", new RouteValueDictionary(new
                    {
                        controller = "Homeworks",
                        action = "ShowHomework",
                        id_tema = model.id_tema
                    })); 

                }
                return RedirectToAction("ShowHomework", new RouteValueDictionary(new
                {
                    controller = "Homeworks",
                    action = "ShowHomework",
                    id_tema = model.id_tema
                })); 
            }
        }

    }

}

