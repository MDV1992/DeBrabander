using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeBrabander.Models;
using DeBrabander.DAL;
using DeBrabander.Utils;

namespace DeBrabander.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (Context db = new Context())
                {
                    account.Password = SecurityUtil.hashPassword(account.Password);
                    account.ConfirmPassword = SecurityUtil.hashPassword(account.ConfirmPassword);
                    db.UserAccounts.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + "Registratie is voltooid.";
            }
            return View();
        }

        //login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login (UserAccount user)
        {
            using (Context db = new Context())
            {
                var usr = db.UserAccounts.Where(u => u.Username == user.Username).FirstOrDefault();
                if (usr != null && usr.Password == SecurityUtil.hashPassword(user.Password))
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Gebruikersnaam"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Gebruikersnaam of wachtwoord zijn fout");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}