using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace Homework.Utils
{
    public class AccountUtil
    {
        public static void registerUser(HomeworkContext db, RegisterModel model, int userId)
        {
            var user = new User();
            user.id_user = userId;
            user.nume = model.Name;
            user.prenume = model.Surname;
            user.parola = model.Password;
            user.email = model.UserName;
            user.activ = 1;
            user.tip = model.type;
            user.clasa = model.selectedClass;
            user.an_studiu = model.selectedYear;
            user.Liceu = db.Liceus.Where(a => a.id_liceu == model.selectedHighschool).First();
            db.Users.Add(user);
        }
    }
}