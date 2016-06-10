﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertificateRepository;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult SearchResults()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string name, string password, string phone, string email)
        {
            var mgr = new UserAuthRepository();
            string onlyNumericNumber = Regex.Replace(phone, @"[^0-9]", "");
            User u = mgr.AddUser(name, password, onlyNumericNumber, email);
            FormsAuthentication.SetAuthCookie(u.Id.ToString(), true);
            return RedirectToAction("index", "portal");
        }
        public ActionResult CheckIfEmailExist(string email)
        {
            var mgr = new UserAuthRepository();
            return Json(mgr.checkIfEmailExist(email));
        }

        public ActionResult Login()
        {
            return View(false);
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var mgr = new UserAuthRepository();
            User u = mgr.GetUser(email, password);
            if (u == null)
            {
                // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myFunction()", true);
                return View(true);
            }
            else
            {
                mgr.AddAction(u.Id, "Log In", DateTime.Now);
                FormsAuthentication.SetAuthCookie(u.Id.ToString(), true);
                return RedirectToAction("index", "portal");
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult LockScreen()
        {
            return View();
        }

        public ActionResult Login_2()
        {
            return View();
        }

        public ActionResult Register()
        {
            OrgRegisterRepository mgr = new OrgRegisterRepository();
            var cats = mgr.GetAllCategories();
            return View(cats);
        }

        public ActionResult OrgRegister()
        {
            return View();
        }

        public ActionResult NotFoundError()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult EmptyPage()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var mgr = new EmailManager();
            mgr.ResetPassword(email);
            return RedirectToAction("ResetPasswordConfirmation");
        }
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public ActionResult CheckEmail(string email)
        {
            var mgr = new UserAuthRepository();
            return Json(mgr.CheckIfEmailExist(email), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetAuth(string token)
        {
            if (token == null)
            {
                return RedirectToAction("ForgotPassword");
            };
            var mgr = new ResetPasswordRepository();
            bool isValid = mgr.CheckTokenValidity(token);
            if (isValid)
            {
                User u = mgr.GetUserFromToken(token);
                return View(u.Id);
            }
            else
            {
                return RedirectToAction("InvalidToken");
            }
        }
        [HttpPost]
        public ActionResult ResetAuthPassword(string password, int userid)
        {
            var mgr = new UserAuthRepository();
            var rmgr = new ResetPasswordRepository();
            mgr.AddAction(userid, "reset password", DateTime.Now);
            rmgr.DeleteToken(userid);
            mgr.UpdatePassword(password, userid);
            return RedirectToAction("Login");
        }
        public ActionResult InvalidToken()
        {
            return View();
        }
    }
}