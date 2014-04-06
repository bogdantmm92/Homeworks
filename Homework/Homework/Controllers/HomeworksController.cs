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
            return (int)Session["user_id"];
        }

        public void getUsername()
        {
            int id = getMyId();
            using (var db = new HomeworkContext())
            {
                var user = db.Users.Where(a => a.id_user == id).FirstOrDefault(); ;
                var name = user.nume + " " + user.prenume;
                ViewBag.username = name;
                //return View();
            }
        }

    }
}
