using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertificateRepository;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    [Authorize]
    public class PortalController : Controller
    {
        public void CheckIfInitialLogin()
        {
           

        }
        public ActionResult Index()
        {
            var mgr = new UserPortalRepository();
            PortalViewModel vm = new PortalViewModel();
            vm.ExpirationItems = mgr.GetAllExpirationItems(int.Parse(User.Identity.Name));
            vm.Categories = mgr.GetAllCategories();
            vm.NonExpirationItems = mgr.GetAllNonExpirationItems(int.Parse(User.Identity.Name)).ToList();
            vm.Courses = mgr.GetAllCourses(int.Parse(User.Identity.Name));
            vm.CoreCourses = mgr.GetAllCoreCourses(int.Parse(User.Identity.Name));
            vm.Reminders = mgr.GetAllReminders(int.Parse(User.Identity.Name));
            vm.UserOrganizations = mgr.GetAllUserRelationships(int.Parse(User.Identity.Name));
            if (Request.Cookies["tour"].Value == User.Identity.Name)
            {
                vm.InitialLogin = false;
            }
            else
            {
                vm.InitialLogin = true;
            }
            return View(vm);
        }

        public ActionResult GetItem(int itemid)
        {
            var mgr = new UserPortalRepository();
            return Json(mgr.GetExpirationItem(itemid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddItem(string name, int category, int reminder, DateTime? issuedate, DateTime expiredate, string notes, HttpPostedFileBase[] image)
        {
            var mgr = new UserPortalRepository();
            int identity = mgr.AddItem(int.Parse(User.Identity.Name), name, category, reminder, issuedate, expiredate, notes);

            if (image[0] != null)
            {
                Guid g = Guid.NewGuid();
                string gg = g.ToString();
                image[0].SaveAs(Server.MapPath("~/Images") + "\\" + gg + ".jpg");
                mgr.AddImage(gg + ".jpg", identity);
            }

            if (image[1] != null)
            {
                Guid g = Guid.NewGuid();
                string gg = g.ToString();
                image[1].SaveAs(Server.MapPath("~/Images") + "\\" + gg + ".jpg");
                mgr.AddImage2(gg + ".jpg", identity);
            }
            TempData["success"] = "The " + name + " item was added successfully";
            return RedirectToAction("index");
        }

        public ActionResult Getemtcredits()
        {
            var mgr = new UserPortalRepository();
            return Json(mgr.GetEMTCredits(int.Parse(User.Identity.Name)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateNEItem(string ENEName, DateTime ENEDate, string ENENotes, int ENEitemid)
        {
            var mgr = new UserPortalRepository();
            NonExpirationItem n = new NonExpirationItem();
            n.UserId = int.Parse(User.Identity.Name);
            n.Name = ENEName;
            n.Id = ENEitemid;
            if (ENENotes != null)
            {
                n.Notes = ENENotes;
            }
            n.Date = ENEDate;
            mgr.UpdateNEItem(n);
            TempData["success"] = "The item was updated successfully";
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult UpdateItem(string EIname, int EIcategory, DateTime? EIissuedate, DateTime EIexpiredate, string EInotes, int EIitemid, HttpPostedFileBase[] image)
        {
            var mgr = new UserPortalRepository();
            int identity = mgr.UpdateItem(int.Parse(User.Identity.Name), EIname, EIcategory, EIissuedate, EIexpiredate, EInotes, EIitemid);
            if (image[0] != null)
            {
                Guid g = Guid.NewGuid();
                string gg = g.ToString();
                image[0].SaveAs(Server.MapPath("~/Images") + "\\" + gg + ".jpg");
                mgr.UpdateImage1(gg + ".jpg", identity, int.Parse(User.Identity.Name));
            }

            if (image[1] != null)
            {
                Guid g = Guid.NewGuid();
                string gg = g.ToString();
                image[1].SaveAs(Server.MapPath("~/Images") + "\\" + gg + ".jpg");
                mgr.UpdateImage2(gg + ".jpg", identity, int.Parse(User.Identity.Name));
            }
            TempData["success"] = "The " + EIname + " item was successfully updated";
            return RedirectToAction("index");
        }

        [HttpPost]
        public void DeleteNEItem(int itemid)
        {
            var mgr = new UserPortalRepository();
            mgr.DeleteNEItem(itemid);
        }

        [HttpPost]
        public void DeleteEItem(int itemid)
        {
            var mgr = new UserPortalRepository();
            mgr.DeleteEItem(itemid);
        }

        [HttpPost]
        public ActionResult AddCourse(string courseName, DateTime courseDate, int courseCredits, string courseNotes)
        {
            var mgr = new UserPortalRepository();
            Course c = new Course();
            c.UserId = int.Parse(User.Identity.Name);
            c.Name = courseName;
            c.Notes = courseNotes;
            c.Date = courseDate;
            c.Credits = courseCredits;
            mgr.AddCourse(c);
            TempData["success"] = "A course was successfully added";
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult AddCoreCourse(string courseName, int coreCategory, DateTime courseDate, string courseNotes)
        {
            var mgr = new UserPortalRepository();
            CoreCourse c = new CoreCourse();
            c.UserId = int.Parse(User.Identity.Name);
            c.Name = courseName;
            c.CatId = coreCategory;
            c.Notes = courseNotes;
            c.Date = courseDate;
            mgr.AddCoreCourse(c);
            TempData["success"] = "A course was successfully added";
            return RedirectToAction("index");
        }

        public ActionResult AddNonExpirationItem(string NEName, DateTime NEDate, string NENotes)
        {
            var mgr = new UserPortalRepository();
            NonExpirationItem n = new NonExpirationItem();
            n.UserId = int.Parse(User.Identity.Name);
            n.Name = NEName;
            if (NENotes != null)
            {
                n.Notes = NENotes;
            }
            n.Date = NEDate;
            mgr.AddNEItem(n);
            TempData["success"] = "A non expiration item was successfully added";
            return RedirectToAction("index");
        }

        public ActionResult GetNEItem(int itemid)
        {
            var mgr = new UserPortalRepository();
            return Json(mgr.GetNEExpirationItem(itemid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteCourse(int courseid)
        {
            var mgr = new UserPortalRepository();
            mgr.DeleteCourse(courseid);
        }
        [HttpPost]
        public void DeleteCoreCourse(int courseid)
        {
            var mgr = new UserPortalRepository();
            mgr.DeleteCoreCourse(courseid);
        }

        [HttpPost]
        public ActionResult UpdateCourse(int itemid, string notes, DateTime date, int credits, string name)
        {
            var mgr = new UserPortalRepository();
            mgr.UpdateCourse(itemid, notes, date, credits, name);
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult UpdateCoreCourse(int itemid, string notes, DateTime date, int category, string name)
        {
            var mgr = new UserPortalRepository();
            mgr.UpdateCoreCourse(itemid, notes, date, category, name);
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult AddReminder(int addReminder, int expirationItemId)
        {
            var mgr = new UserPortalRepository();
            mgr.AddReminder(addReminder, expirationItemId);
            TempData["success"] = "A reminder was added successfully";
            return RedirectToAction("index");
        }
        public ActionResult EditImageOne(int itemid)
        {
            var mgr = new UserPortalRepository();
            Image i = mgr.GetImageOne(itemid);
            return Json(i.Path, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditImageTwo(int itemid)
        {
            var mgr = new UserPortalRepository();
            Image i = mgr.GetImageTwo(itemid);
            return Json(i.Path, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckReminder(int expirationItemId, int addReminder)
        {
            var mgr = new UserPortalRepository();
            var result = mgr.CheckReminder(expirationItemId, addReminder);
            return Json(result);
        }
        public ActionResult GetCompletedItems(int orgid, int userid)
        {
            var mgr = new UserPortalRepository();
            var requiredItems = mgr.GetRequiredItems(orgid, userid);
            var items = mgr.GetAllExpirationItems(userid);
            List<OrgRequiredItem> completedItems = new List<OrgRequiredItem>();
            foreach (OrgRequiredItem o in requiredItems)
            {
                foreach (ExpirationItem e in items)
                {
                    if (e.CategoryId == o.CatId && e.ExpirationDate >= DateTime.Now)
                    {
                        completedItems.Add(o);
                        break;
                    }
                }
            }
            var result = new
            {
                RequiredItems = completedItems.Select(e => new
                {
                    Id = e.Id,
                    CatId = e.CatId,
                    CategoryName = e.Category.Name
                })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRequiredItems(int orgid, int userid)
        {
            var mgr = new UserPortalRepository();
           
            var requiredItems = mgr.GetRequiredItems(orgid, userid);
            var items = mgr.GetAllExpirationItems(userid);
            List<OrgRequiredItem> incompletedItems = new List<OrgRequiredItem>();
            foreach (OrgRequiredItem o in requiredItems)
            {
                bool exist = false;
                foreach (ExpirationItem e in items)
                {
                    
                    if (e.CategoryId == o.CatId && e.ExpirationDate >= DateTime.Now)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist != true)
                {
                    incompletedItems.Add(o);
                }

            }
            var result = new
            {
                RequiredItems = incompletedItems.Select(e => new
                {
                    Id = e.Id,
                    CatId = e.CatId,
                    CategoryName = e.Category.Name
                })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExtraItems(int orgid, int userid)
        {
            var mgr = new UserPortalRepository();
            bool ItemsInShared = mgr.SeeIfSharedItems(orgid, userid);
            if (ItemsInShared == true)
            {
                var itemsShared = mgr.GetSharedItems(orgid, userid);

                var requiredItems = mgr.GetRequiredItems(orgid, userid);
                var items = mgr.GetAllExpirationItems(userid);
                List<ExpirationItem> extraItems = new List<ExpirationItem>();
               
                foreach(ExpirationItem i in items)
                {
                    bool exist = true;
                    foreach(ItemShareWithCompany j in itemsShared)
                    {
                        if(i.Id == j.ExpirationItemId)
                        {
                            exist = false;
                           
                        }
                        //If i.Id does not exist in j.ItemId then add it to
                    }
                    if(exist == true)
                    {
                        bool exists = true;
                        foreach (OrgRequiredItem o in requiredItems) 
                        {
                            if (i.CategoryId == o.CatId)
                            {
                                exists = false;
                            }
                        }
                        if (exists == true)
                        {
                            mgr.AddSharedItems(userid, orgid, i.Id);
                        }
                    }
                }
                var updateditemsShared = mgr.GetSharedItems(orgid, userid);

                var results = new
                {
                    Items = updateditemsShared.Select(e => new
                    {
                        ItemId = e.ExpirationItem.Id,
                        Name = e.ExpirationItem.Name,
                        CategoryName = e.ExpirationItem.Category.Name,
                        Shared = e.Share
                    })
                };
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var requiredItems = mgr.GetRequiredItems(orgid, userid);
                var items = mgr.GetAllExpirationItems(userid);
                List<ExpirationItem> extraItems = new List<ExpirationItem>();

                foreach (ExpirationItem e in items)
                {
                    bool exist = true;
                    foreach (OrgRequiredItem o in requiredItems)
                    {
                        if (e.CategoryId == o.CatId)
                        {
                            exist = false;
                        }
                    }
                    if (exist == true)
                    {
                        extraItems.Add(e);
                    }
                }
                foreach (ExpirationItem u in extraItems)
                {
                    mgr.AddSharedItems(userid, orgid, u.Id);
                }
                var result = new
                {
                    Items = extraItems.Select(e => new
                    {
                        ItemId = e.Id,
                        Name = e.Name,
                        CategoryName = e.Category.Name,
                        Shared = false
                    })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateItemsToShare(IEnumerable<int> item, int userId, int orgId)
        {
            var mgr = new UserPortalRepository();
            mgr.UpdateSharedItem(item, userId, orgId);
            TempData["success"] = "List of shared items was successfully updated";
            return RedirectToAction("Index");
        }
        public ActionResult CheckIfImage(int itemid)
        {
            var mgr = new UserPortalRepository();
            return Json(mgr.CheckIfImage(itemid), JsonRequestBehavior.AllowGet);
        }
        public void CreateCookieForModal()
        {
            var cookie = new HttpCookie("tour", User.Identity.Name);
            Response.Cookies.Add(cookie);
        }
    }
}