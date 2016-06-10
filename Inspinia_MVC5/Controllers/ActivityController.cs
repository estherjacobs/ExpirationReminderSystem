using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;
using CertificateRepository;

namespace Inspinia_MVC5.Controllers
{
    public class ActivityController : Controller
    {
        
        public ActionResult Index()
        {
            var vm = new ActivityViewModel();
            var mgr = new ActivityRepository();
            vm.Actions = mgr.GetAllActivity(int.Parse(User.Identity.Name));
            return View(vm);
        }
    }
}