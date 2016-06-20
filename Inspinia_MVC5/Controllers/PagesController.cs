using System;
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
            //EmailManager em = new EmailManager();
            //em.SendWelcomeEmail(name, email);
            SMSManager SMS = new SMSManager();
            string message = "Welcome to Expiration Tracking App! Thank you for setting up an account with us and we look forward to working you.";
            SMS.Notification(u.PhoneNumber, message);
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
            return View();
        }

        public ActionResult OrgRegister()
        {
            OrgRegisterRepository mgr = new OrgRegisterRepository();
            var cats = mgr.GetAllCategories();
            return View(cats);
        }
        public ActionResult OrgSignUp(string name, string email, string phone, string password, string oname, string oemail, string oaddress, string ocity, string ostate, string ozip, string ophone, int year, IEnumerable<int> category)
        {
            var mgr = new UserAuthRepository();
            string onlyNumericNumber = Regex.Replace(phone, @"[^0-9]", "");
            User u = mgr.AddUser(name, password, onlyNumericNumber, email);
            //SMSManager SMS = new SMSManager();
            //string message = "Welcome to Expiration Reminder App! Thanks for setting up a new organization account with us and we look forward to working with you.";
            //SMS.Notification(u.PhoneNumber, message);
            Organization o = mgr.AddOrg(u.Id, oname, oaddress, oemail, ocity, ostate, ozip, ophone, year);
            mgr.CreateInitialUserOrdRel(o.Id, u.Id);
            foreach(int i in category)
            {
                mgr.CreateOrgReqItems(o.Id, i);
            }
            FormsAuthentication.SetAuthCookie(u.Id.ToString(), true);
            return RedirectToAction("index", "portal");
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