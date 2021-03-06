﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using CertificateRepository;
using Inspinia_MVC5.Models;
using System.Text.RegularExpressions;

namespace Inspinia_MVC5.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            var mgr = new UserProfileRepository();
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = mgr.GetUser(int.Parse(User.Identity.Name));
            vm.Organizations = mgr.GetOrganization(int.Parse(User.Identity.Name));
            Contact c = mgr.GetContact(int.Parse(User.Identity.Name));

            if (c != null)
            {
                vm.Contact = c;
            }
            return View(vm);
        }

        public ActionResult CheckPassword(string password)
        {
            var mgr = new UserProfileRepository();

            return Json(mgr.CheckPassword(int.Parse(User.Identity.Name), password));
        }

        [HttpPost]
        public ActionResult UpdateAccount(int userid, string name, string email, string phone)
        {
            var mgr = new UserProfileRepository();
            string onlyNumericPhone = Regex.Replace(phone, @"[^0-9]", "");
            mgr.UpdateAccount(userid, name, email, onlyNumericPhone);
            TempData["success"] = "Your account was updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePassword(int userid, string newPassword, string oldPassword, string confirmNewPassword)
        {
            var mgr = new UserProfileRepository();
            mgr.UpdatePassword(userid, newPassword);
            TempData["success"] = "Your password was updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateMethodNotification(int userid, string radio)
        {
            var mgr = new UserProfileRepository();
            User u = mgr.UpdateMethodNotification(userid, radio);
            TempData["success"] = "Your notification method was updated successfully!";
            SMSManager manager = new SMSManager();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateNotification(int userid, string name, string phone, string email)
        {
            var mgr = new UserProfileRepository();
            User u = mgr.UpdateNotification(userid, name, phone, email);
            TempData["success"] = "Your notification contact  info was updated successfully!";
            SMSManager manager = new SMSManager();
            //if (radio == "2" || radio == "3")
            //{
            //    string message = "This notification is to confirm that you have updated your notification settings to enable SMS reminders.";
            //    manager.Notification(u.PhoneNumber, message);
            //}
            return RedirectToAction("Index");
        }
    }
}