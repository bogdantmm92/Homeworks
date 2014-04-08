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

        public ActionResult Teme()
        {
            using (var db = new HomeworkContext())
            {
                var model = new TemeModel();
                model.teme = db.Temas.Where(a => a.id_tema <= 2).ToList();

                return View(model);
            }
        }


        public void createMyId(int id)
        {
            Session["userId"] = id;            
        }

        public int getMyId()
        {
            return (int)Session["userId"];
        }

        public void getUsername()
        {
            int id = getMyId();
            using (var db = new HomeworkContext())
            {
                var user = db.Users.Where(a => a.id_user == id).FirstOrDefault(); ;
                var name = user.nume + " " + user.prenume;
                ViewBag.username = name;
            }
        }

        
        /*
        public ActionResult SeeHomework(int id_tema)
        {
            using (var db = new HomeworkContext())
            {
                var model = new List <CommentModel>();
                var com = db.Comentarius.Where(a => a.id_tema == id_tema).ToList();
                foreach(var c in com)
                {
                    CommentModel comm = new CommentModel();
                    comm.data = c.data;
                    comm.text = c.text;
                    var user = db.Users.Where(a => a.id_user == c.id_user).FirstOrDefault();
                    comm.username = user.nume + " " + user.prenume;
                    model.Add(comm);
                }

                return View(model);
            }
        }*/

        //adauga comentariu
        
        //[HttpGet]
        public ActionResult SeeHomework()
        {
            //createMyId(1);
            return View();
        }
        /*
        [HttpPost]
        public ActionResult SeeHomework(/*int id_tema, CommentModel model)
        {
            using (var db = new HomeworkContext())
            {
                if (ModelState.IsValid)
                {
                    db.Comentarius.Add(new Comentariu
                    {
                        id_tema = 1,//id_tema,
                        text = model.text,
                        id_user = getMyId(),
                        data = DateTime.Now
                    });

                    db.SaveChanges();
                    return RedirectToAction("SeeHomework");
                }
            
                return View(model);
            }
        }
        */

    }
}
