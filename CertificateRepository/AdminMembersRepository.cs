using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class AdminMembersRepository
    {
        public IEnumerable<User> GetAllMembers(int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var organizations = db.UserOrganizations.Where(i => i.OrgId == orgid);
                var users = db.Users.ToList();
                List<User> list = new List<User>();
                foreach (User u in users)
                {
                    foreach (UserOrganization o in organizations)
                    {
                        if (o.UserId == u.Id)
                        {
                            list.Add(u);
                        }
                    }
                }
                return list;
            }
        }
        public IEnumerable<UserOrganization> UserOrgItems(int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.UserOrganizations.Where(i => i.OrgId == orgid).ToList();
            }
        }
        public int GetStatus(int companyid, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var items = db.OrgRequiredItems.Where(i => i.OrgId == companyid);
                var expirationItems = db.ExpirationItems.Where(o => o.UserId == userid);
                int number = expirationItems.Count();
                if (number <= 2)
                {
                    return 1;
                }
                List<ExpirationItem> eitems = new List<ExpirationItem>();
                foreach (OrgRequiredItem o in items)
                {
                    ExpirationItem e = expirationItems.FirstOrDefault(i => i.CategoryId == o.CatId);
                    if (e != null)
                    {
                        eitems.Add(e);
                    }
                }
                if (eitems.Count < 3)
                {
                    return 1;
                }
                if (eitems.Count >= 3)
                {
                    foreach (ExpirationItem e in eitems)
                    {
                        if (e.ExpirationDate < DateTime.Now)
                        {
                            return 4;
                        }
                        if (e.ExpirationDate <= DateTime.Now.AddDays(30) && e.ExpirationDate > DateTime.Now)
                        {
                            return 3;
                        }
                    }
                }
                return 2;
            }
        }

        public IEnumerable<ExpirationItem> GetExpirationItems()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                loadOptions.LoadWith<ExpirationItem>(p => p.User);
                db.LoadOptions = loadOptions;
                return db.ExpirationItems.ToList();
            }
        }

        public bool CheckIfEmailExist(string email, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Email == email);
                if (u == null)
                {
                    return true;
                }
                UserOrganization uo = db.UserOrganizations.FirstOrDefault(i => i.UserId == u.Id && i.OrgId == orgid);
                if (uo == null)
                {
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<Course> GetCourses()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {

                return db.Courses.ToList();
            }
        }
        public IEnumerable<CoreCourse> GetCoreCourses()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<CoreCourse>(p => p.CourseCategory);
                db.LoadOptions = loadOptions;
                return db.CoreCourses.ToList();
            }
        }
        public User GetUserDetails(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<User>(p => p.ExpirationItems);
                loadOptions.LoadWith<User>(p => p.Courses);
                loadOptions.LoadWith<User>(p => p.CoreCourses);
                loadOptions.LoadWith<CoreCourse>(p => p.CourseCategory);
                loadOptions.LoadWith<ExpirationItem>(e => e.Category);
                loadOptions.LoadWith<Category>(e => e.OrgRequiredItems);
                loadOptions.LoadWith<ExpirationItem>(e => e.ItemShareWithCompanies);
                db.LoadOptions = loadOptions;
                var list = db.Users.FirstOrDefault(i => i.Id == userid);
                return list;
            }
        }
        public void DisableUser(int userid, int orgid, int currentUserId)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.UserOrganizations.FirstOrDefault(i => i.UserId == userid && i.OrgId == orgid);
                Organization o = db.Organizations.FirstOrDefault(j => j.Id == orgid);
                User u = db.Users.FirstOrDefault(r => r.Id == userid);
                AddAction(userid, o.Name + " Disabled organization relationship", DateTime.Now);
                AddOrgAction(orgid, currentUserId, "Disabled relationship with " + u.FullName, DateTime.Now);
                db.UserOrganizations.DeleteOnSubmit(item);
                db.SubmitChanges();

            }
        }
        public IEnumerable<OrgRequiredItem> GetRequiredItems(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<OrgRequiredItem>(p => p.Organization);
                loadOptions.LoadWith<OrgRequiredItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                return db.OrgRequiredItems.ToList();
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                db.DeferredLoadingEnabled = false;
                return db.Users.ToList();
            }
        }
        public IEnumerable<ExpirationItem> GetMemberDetails(int userid, int companyid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {

                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ExpirationItem>(p => p.User);
                loadOptions.LoadWith<User>(p => p.CoreCourses);
                loadOptions.LoadWith<User>(p => p.Courses);
                loadOptions.LoadWith<CoreCourse>(p => p.CourseCategory);
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                IEnumerable<OrgRequiredItem> o = db.OrgRequiredItems.Where(i => i.OrgId == companyid);
                IEnumerable<ExpirationItem> e = db.ExpirationItems.Where(i => i.UserId == userid);
                List<ExpirationItem> list = new List<ExpirationItem>();
                foreach (ExpirationItem i in e)
                {
                    foreach (OrgRequiredItem r in o)
                    {
                        if (i.CategoryId == r.CatId)
                        {
                            list.Add(i);
                        }
                    }
                }
                return list.ToList();
            }
        }
        public Organization GetOrganizationByUser(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                UserOrganization uo = db.UserOrganizations.FirstOrDefault(i => i.UserId == userid && i.Permission != 1);
                return db.Organizations.FirstOrDefault(i => i.Id == uo.OrgId);
            }
        }
        public IEnumerable<Course> GetCoursePerUser(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Courses.Where(i => i.UserId == userid);
            }
        }
        public DateTime GetDateJoined(int userid, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                UserOrganization uo = db.UserOrganizations.FirstOrDefault(i => i.UserId == userid && i.OrgId == orgid);
                return uo.Date.Value;

            }
        }
        public void AddMember(string email, int permission, string token, int? userid, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var t = new AddMemberToken();
                t.Email = email;
                t.Permission = permission;
                t.Token = token;
                t.OrgId = orgid;
                if (userid != null)
                {
                    t.UserId = userid;
                }
                db.AddMemberTokens.InsertOnSubmit(t);
                db.SubmitChanges();
            }
        }
        public User CheckIfUserExist(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Email == email);
                if (u == null)
                {
                    return null;
                }
                return u;
            }
        }
        public Organization GetOrg(int companyid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Organizations.FirstOrDefault(i => i.Id == companyid);
            }
        }
        public bool CheckTokenValidity(string token)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                AddMemberToken a = db.AddMemberTokens.FirstOrDefault(i => i.Token == token);
                if (a == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public AddMemberToken GetToken(string token)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                AddMemberToken a = db.AddMemberTokens.FirstOrDefault(i => i.Token == token);
                return a;
            }
        }
        public void SetupMemberRel(string token, int? userid, int currentUserId)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                AddMemberToken a = db.AddMemberTokens.FirstOrDefault(i => i.Token == token);
                UserOrganization u = new UserOrganization();
                if (a.UserId == null)
                {
                    u.UserId = userid.Value;
                }
                else
                {
                    u.UserId = a.UserId.Value;
                }
                u.Date = DateTime.Now;
                u.OrgId = a.OrgId;
                u.Permission = a.Permission;
                db.UserOrganizations.InsertOnSubmit(u);
                User j = db.Users.FirstOrDefault(r => r.Id == u.UserId);
                db.SubmitChanges();
                DeleteToken(a.Id);
                AddOrgAction(a.OrgId, currentUserId, j.FullName + " setup Relationship with you", DateTime.Now);
                AddAction(u.UserId, "Setup Relationship with organization " + u.Organization.Name, DateTime.Now);
            }
        }
        public void DeleteToken(int tokenid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                AddMemberToken c = db.AddMemberTokens.FirstOrDefault(i => i.Id == tokenid);
                db.AddMemberTokens.DeleteOnSubmit(c);
                db.SubmitChanges();
            }
        }
        public void AddAction(int userid, string name, DateTime date)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Action a = new Action();
                a.UserId = userid;
                a.Name = name;
                a.Date = date;
                db.Actions.InsertOnSubmit(a);
                db.SubmitChanges();
            }
        }
        public void AddOrgAction(int orgid, int userid, string name, DateTime date)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                OrgAction a = new OrgAction();
                a.UserId = userid;
                a.OrgId = orgid;
                a.Name = name;
                a.Date = date;
                db.OrgActions.InsertOnSubmit(a);
                db.SubmitChanges();
            }
        }
        public IEnumerable<OrgAction> GetOrgActivity(int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<OrgAction>(p => p.User);
                db.LoadOptions = loadOptions;
                return db.OrgActions.Where(u => u.OrgId == orgid).ToList();
            }
        }
        public IEnumerable<ExpirationItem> GetRequiredItems(int userid, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var items = db.OrgRequiredItems.Where(i => i.OrgId == orgid);
                var expirationItems = db.ExpirationItems.Where(o => o.UserId == userid);
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                List<ExpirationItem> eitems = new List<ExpirationItem>();
                foreach (OrgRequiredItem o in items)
                {
                    ExpirationItem e = expirationItems.FirstOrDefault(i => i.CategoryId == o.CatId);
                    if (e != null)
                    {
                        eitems.Add(e);
                    }
                }

                return eitems;
            }
        }

        public IEnumerable<ExpirationItem> GetExtraItems(int userid, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var items = db.ItemShareWithCompanies.Where(i => i.OrgId == orgid && i.UserId == userid && i.Share == true);
                var expirationItems = db.ExpirationItems.Where(o => o.UserId == userid).OrderBy(r => r.ExpirationDate);
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                List<ExpirationItem> eitems = new List<ExpirationItem>();
                foreach (ExpirationItem i in expirationItems)
                {
                    foreach (ItemShareWithCompany j in items)
                    {
                        if (i.Id == j.ExpirationItemId)
                        {
                            eitems.Add(i);
                            break;
                        }
                    }
                }
                IEnumerable<ExpirationItem> updated = eitems;
                return updated.ToList();
            }
        }
        public int GetPermission(int userid, int orgid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.UserOrganizations.FirstOrDefault(i => i.OrgId == orgid && i.UserId == userid);
                return item.Permission.Value;
            }
        }
        public string GetItemImage1(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid && i.IsBack == false);
                if (item != null)
                {
                    return item.Path;
                }
                else
                {
                    return null;
                }
            }
        }
        public string GetItemImage2(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid && i.IsBack == true);
                if (item != null)
                {
                    return item.Path;
                }
                else
                {
                    return null;
                }

            }
        }

        public bool CheckIfImage(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid && i.IsBack == false);
                if (item == null)
                {
                    //There is no image with this item id 
                    return false;
                }
                else
                {
                    //There is a image with this item id
                    return true;
                }
            }
        }
        public Organization GetOrgForProfile(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.UserOrganizations.FirstOrDefault(i => i.UserId == userid && i.Permission == 3);
                return db.Organizations.FirstOrDefault(i => i.Id == item.OrgId);
            }
        }
        public void UpdateOrganization(int userid, int orgid, string name, string email, string phone, string address, string city, string state, string zip, int year)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var j = db.Organizations.FirstOrDefault(i => i.Id == orgid);
                j.Name = name;
                j.Email = email;
                j.PhoneNumber = phone;
                j.Address = address;
                j.City = city;
                j.State = state;
                j.Zip = zip;
                j.YearFounded = year;
                db.SubmitChanges();
                AddOrgAction(j.Id, userid, " Updated company profile", DateTime.Now);
            }
        }
        public IEnumerable<UserOrganization> GetRelationships(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<UserOrganization>(p => p.User);
                db.LoadOptions = loadOptions;
                var item = db.UserOrganizations.FirstOrDefault(i => i.UserId == userid && i.Permission != 1);
                var list = db.UserOrganizations.Where(i => i.OrgId == item.OrgId);
                return list.ToList();
            }

        }
        public bool CheckIfAdminAnywhere(User u)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                UserOrganization i = db.UserOrganizations.FirstOrDefault(j => j.UserId == u.Id && j.Permission != 1);
                if(i == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
