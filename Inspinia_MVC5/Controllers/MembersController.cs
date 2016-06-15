using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertificateRepository;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    public class MembersController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            MemberViewModel vm = new MemberViewModel();
            var mgr = new AdminMembersRepository();
            vm.Organization = mgr.GetOrganizationByUser(int.Parse(User.Identity.Name));
            vm.Members = mgr.GetAllMembers(vm.Organization.Id);
            vm.UserOrganizations = mgr.UserOrgItems(vm.Organization.Id);
            vm.Required = mgr.GetRequiredItems(int.Parse(User.Identity.Name));
            //vm.Organization = mgr.GetOrganizationByUser(int.Parse(User.Identity.Name));
            return View(vm);
        }
        public ActionResult Activity()
        {
            var mgr = new AdminMembersRepository();

            var manager = new UserPortalRepository();
            IEnumerable<UserOrganization> list = manager.GetRelationships(int.Parse(User.Identity.Name));
            int orgid = list.FirstOrDefault().OrgId;
            //Just put in company Id
            var lists = mgr.GetOrgActivity(orgid);

            return View(lists);
        }
        [HttpPost]
        public void DisableUser(int userid, int orgid)
        {
            var mgr = new AdminMembersRepository();

            mgr.DisableUser(userid, orgid, int.Parse(User.Identity.Name));
        }
        public ActionResult GetDateJoined(int userid, int orgid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.GetDateJoined(userid, orgid).ToShortDateString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserDetails(int userid, int orgid)
        {
            var mgr = new AdminMembersRepository();
            var list = mgr.GetUserDetails(userid);
            var permission = mgr.GetPermission(userid, orgid);
            var requiredItems = mgr.GetRequiredItems(userid, orgid);
            var extraItems = mgr.GetExtraItems(userid, orgid);
            var result = new
            {
                Id = list.Id,
                FullName = list.FullName,
                Email = list.Email,
                PhoneNumber = list.PhoneNumber,
                Permission = permission,
                RequiredItems = requiredItems.OrderBy(i => i.ExpirationDate).Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    ExpirationDate = e.ExpirationDate.ToShortDateString(),
                    CategoryName = e.Category.Name
                }),
                ExtraItems = extraItems.OrderBy(i => i.ExpirationDate).Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    ExpirationDate = e.ExpirationDate.ToShortDateString(),
                    CategoryName = e.Category.Name
                }),
                Courses = list.Courses.Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date.ToShortDateString(),
                    Credits = e.Credits
                }),
                CoreCourses = list.CoreCourses.Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date.ToShortDateString(),
                    CategoryName = e.CourseCategory.Name
                })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCoursePerUser(int userid)
        {
            var mgr = new AdminMembersRepository();
            var list = mgr.GetCoursePerUser(userid);
            var result = new
            {
                Courses = list.Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    Credits = e.Credits
                })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult InvalidToken()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getMemberDetails(int memberUserId)
        {
            var mgr = new AdminMembersRepository();
            var company = mgr.GetOrganizationByUser(int.Parse(User.Identity.Name));
            var list = mgr.GetMemberDetails(memberUserId, company.Id);
            return Json(list);
        }
        [HttpPost]
        public ActionResult AddMember(string email, int permission, int companyid)
        {
            var mgr = new AdminMembersRepository();

            Guid g = Guid.NewGuid();
            string gg = g.ToString();
            Organization o = mgr.GetOrg(companyid);
            User u = mgr.CheckIfUserExist(email);
            if (u == null)
            {
                mgr.AddMember(email, permission, gg, null, o.Id);
            }
            else
            {
                mgr.AddAction(int.Parse(User.Identity.Name), "Received invite to join organization " + o.Name, DateTime.Now);
                mgr.AddMember(email, permission, gg, u.Id, o.Id);
            }
            var manager = new EmailManager();
            mgr.AddOrgAction(companyid, int.Parse(User.Identity.Name), "Sent invite to " + email + " to join organization", DateTime.Now);
            manager.SendInvitationMemberEmail(email, gg, o);
            return RedirectToAction("Index");
        }

        public ActionResult Link(string token)
        {
            if (token == null)
            {
                return RedirectToAction("/pages/login");
            }
            var mgr = new AdminMembersRepository();
            bool isValid = mgr.CheckTokenValidity(token);
            AddMemberToken fulltoken = mgr.GetToken(token);
            if (!isValid)
            {
                return RedirectToAction("invalidToken");
            }
            if (fulltoken.UserId != null)
            {
                mgr.SetupMemberRel(token, null, int.Parse(User.Identity.Name));
                return RedirectToAction("success");
            }
            else
            {
                //MemberViewModel vm = new MemberViewModel();
                //vm.Token = fulltoken;
                return View(fulltoken);
            }
        }
        [HttpPost]
        public ActionResult SignUp(string name, string email, string password, string phone, string token, int permission)
        {
            var mgr = new UserAuthRepository();
            var mgr2 = new AdminMembersRepository();
            User u = mgr.AddUser(name, password, phone, email);

            mgr2.SetupMemberRel(token, u.Id, int.Parse(User.Identity.Name));
            return RedirectToAction("Login", "Pages");
        }

        public ActionResult CheckIfEmailExist(string email)
        {
            var mgr = new AdminMembersRepository();
            Organization o = mgr.GetOrganizationByUser(int.Parse(User.Identity.Name));
            bool Exist = mgr.CheckIfEmailExist(email, o.Id);
            //True - Email does exist already
            return Json(Exist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPermission(int userid, int orgid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.GetPermission(userid, orgid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMyPermission(int orgid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.GetPermission(int.Parse(User.Identity.Name), orgid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewItemImage1(int itemid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.GetItemImage1(itemid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewItemImage2(int itemid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.GetItemImage2(itemid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckIfImage(int itemid)
        {
            var mgr = new AdminMembersRepository();
            return Json(mgr.CheckIfImage(itemid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Profile()
        {
            MemberProfileViewModel vm = new MemberProfileViewModel();
            var mgr = new AdminMembersRepository();
            vm.Organization = mgr.GetOrgForProfile(int.Parse(User.Identity.Name));
            vm.Relationships = mgr.GetRelationships(int.Parse(User.Identity.Name));
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateProfileOrg(string name, string email, string phone, string address, string city, string state, string zip, int year, int orgid)
        {
            var mgr = new AdminMembersRepository();
            mgr.UpdateOrganization(int.Parse(User.Identity.Name), orgid, name, email, phone, address, city, state, zip, year);
            return RedirectToAction("Profile");
        }
    }
}