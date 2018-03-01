using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VYO.Models;
using VYODAL.DB.Repository;
using VYODomain.Entities;

namespace VYO.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult ValidateUser(User User)
        {
            UserModel UserDocModel = new UserModel();
            VYOUserRepository UserObj = new VYOUserRepository();
            UserDocModel.UserAdmin = UserObj.ValidateUser(User.UserEmail, User.UserPassword);
            int returnUser = UserDocModel.UserAdmin.Count();
            if (returnUser != 1)
            {
                ViewBag.Message = "User doesn't exist";
            }
            else
            {
                Session["User"] = User.UserEmail;
            }
            return View("LogIN");
            
        }
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
           
            return View("LogIn");
        }
    }
}