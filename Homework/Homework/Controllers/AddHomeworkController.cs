using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework.Controllers
{
   
    public class AddHomeworkController : Controller
    {
        //
        // GET: /AddHomework/

        [HttpGet]
        public ActionResult AddHomework()
        {
            ViewBag.Title = "Creeaza Tema";
            return View();
        }

    }
}
