using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class UserPortalRepository
    {
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
        public IEnumerable<ExpirationItem> GetAllExpirationItems(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                loadOptions.LoadWith<ExpirationItem>(p => p.Reminders);
                db.LoadOptions = loadOptions;
                return db.ExpirationItems.Where(j => j.UserId == userid).OrderBy(i => i.ExpirationDate).ToList();
            }
        }
        public IEnumerable<Reminder> GetAllReminders(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Reminder>(p => p.ExpirationItem);
                loadOptions.LoadWith<Reminder>(p => p.User);
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                loadOptions.LoadWith<ExpirationItem>(p => p.Images);
                db.LoadOptions = loadOptions;
                return db.Reminders.Where(i => i.UserId == userid && i.Date > DateTime.Now).OrderByDescending(i => i.Date).ToList();
            }
        }
        public IEnumerable<Course> GetAllCourses(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                db.LoadOptions = loadOptions;
                return db.Courses.Where(i => i.UserId == userid).ToList();
            }
        }
        public IEnumerable<CoreCourse> GetAllCoreCourses(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<CoreCourse>(p => p.CourseCategory);
                db.LoadOptions = loadOptions;
                return db.CoreCourses.Where(i => i.UserId == userid).ToList();
            }
        }
      
        public IEnumerable<Category> GetAllCategories()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Categories.ToList();
            }
        }
        public IEnumerable<NonExpirationItem> GetAllNonExpirationItems(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.NonExpirationItems.Where(i => i.UserId == userid).ToList();
            }
        }
        public ExpirationItem GetExpirationItem(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                db.DeferredLoadingEnabled = false;
                return db.ExpirationItems.FirstOrDefault(i => i.Id == itemid);
            }
        }

        public NonExpirationItem GetNEExpirationItem(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {

                return db.NonExpirationItems.FirstOrDefault(i => i.Id == itemid);
            }
        }

        public int AddItem(int userid, string name, int category, int reminder, DateTime? issuedate, DateTime expiredate, string notes)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                ExpirationItem i = new ExpirationItem();
                i.Name = name;
                i.Notes = notes;
                i.ExpirationDate = expiredate;
                i.CategoryId = category;
                i.UserId = userid;
                i.IssueDate = issuedate;
                db.ExpirationItems.InsertOnSubmit(i);
                db.SubmitChanges();
                AddReminder(userid, i.Id, reminder, expiredate);
                AddAction(userid, "Add Expiration Item " + i.Name + " " + i.Category.Name, DateTime.Now);
                return i.Id;

            }
        }

        public void AddReminder(int userid, int expirationItemId, int reminder, DateTime expiredate)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Reminder r = new Reminder();
                r.UserId = userid;
                r.ExpirationItemId = expirationItemId;
                r.Days = reminder;
                r.Date = expiredate.AddDays(-reminder).Date;
                db.Reminders.InsertOnSubmit(r);
                db.SubmitChanges();

            }
        }
        public IEnumerable<UserOrganization> GetRelationships(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<UserOrganization>(p => p.Organization);
                loadOptions.LoadWith<UserOrganization>(p => p.User);
                db.LoadOptions = loadOptions;
                return db.UserOrganizations.Where(u => u.UserId == userid).ToList();
            }
        }
        public void AddImage(string gg, int identity)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Image i = new Image(); ;
                i.ExpirationItemId = identity;
                i.Path = gg;
                i.IsBack = false;
                db.Images.InsertOnSubmit(i);
                db.SubmitChanges();
            }
        }
        public void AddImage2(string gg, int identity)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Image i = new Image();
                i.ExpirationItemId = identity;
                i.Path = gg;
                i.IsBack = true;
                db.Images.InsertOnSubmit(i);
                db.SubmitChanges();
            }
        }
        public int GetEMTCredits(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                int total = 0;
                IEnumerable<Course> courses = db.Courses.Where(i => i.UserId == userid);
                ExpirationItem item = db.ExpirationItems.Where(i => i.CategoryId == 2 && i.UserId == userid).OrderByDescending(j => j.ExpirationDate).FirstOrDefault();
                if (item != null)
                {
                    foreach (Course c in courses)
                    {
                        if (c.Date > item.IssueDate && c.Date < item.ExpirationDate)
                        {
                            total += c.Credits;
                        }
                    }
                }
                return total;
            }
        }
        public void AddCourse(Course c)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                db.Courses.InsertOnSubmit(c);
                db.SubmitChanges();
                AddAction(c.UserId, "Add Course " + c.Name, DateTime.Now);
            }
        }
        public void AddCoreCourse(CoreCourse c)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                db.CoreCourses.InsertOnSubmit(c);
                db.SubmitChanges();
                AddAction(c.UserId, "Add Core Course " + c.CourseCategory.Name, DateTime.Now);
            }
        }
        public void AddNEItem(NonExpirationItem n)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                db.NonExpirationItems.InsertOnSubmit(n);
                db.SubmitChanges();
                AddAction(n.UserId, "Add Non Expiration Item " + n.Name, DateTime.Now);
            }
        }
        public void DeleteCourse(int courseid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Course c = db.Courses.FirstOrDefault(i => i.Id == courseid);
                db.Courses.DeleteOnSubmit(c);
                db.SubmitChanges();
                AddAction(c.UserId, "Delete Course " + c.Name, DateTime.Now);
            }
        }
        public void DeleteCoreCourse(int courseid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                CoreCourse c = db.CoreCourses.FirstOrDefault(i => i.Id == courseid);
                db.CoreCourses.DeleteOnSubmit(c);
                db.SubmitChanges();
                AddAction(c.UserId, "Delete Core Course " + c.Name, DateTime.Now);
            }
        }
        public void DeleteEItem(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                foreach (Reminder r in db.Reminders.Where(i => i.ExpirationItemId == itemid))
                {
                    db.Reminders.DeleteOnSubmit(r);
                }
                foreach (Image i in db.Images.Where(i => i.ExpirationItemId == itemid))
                {
                    db.Images.DeleteOnSubmit(i);
                }
                
                foreach (ItemShareWithCompany g in db.ItemShareWithCompanies.Where(j => j.ExpirationItemId == itemid))
                {
                    db.ItemShareWithCompanies.DeleteOnSubmit(g);
                }
               
                ExpirationItem e = db.ExpirationItems.FirstOrDefault(i => i.Id == itemid);
                db.ExpirationItems.DeleteOnSubmit(e);
                db.SubmitChanges();
                AddAction(e.UserId, "Delete Expiration Item " + e.Name, DateTime.Now);
            }
        }

       
        public int UpdateItem(int userid, string EIname, int EIcategory, DateTime? EIissuedate, DateTime EIexpiredate, string EInotes, int EIitemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                ExpirationItem n = db.ExpirationItems.FirstOrDefault(i => i.Id == EIitemid);
                n.Name = EIname;
                n.Id = EIitemid;
                if (EInotes != null)
                {
                    n.Notes = EInotes;
                }
                n.CategoryId = EIcategory;
                if (EIissuedate != null)
                {
                    n.IssueDate = EIissuedate;
                }
                n.ExpirationDate = EIexpiredate;
                db.SubmitChanges();
                AddAction(n.UserId, "Update Expiration Item " + n.Name, DateTime.Now);
                return n.Id;
            }
        }
        public void UpdateImage1(string gg, int identity, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Image i = db.Images.FirstOrDefault(j => j.ExpirationItemId == identity && j.IsBack != true);
                i.Path = gg;
                db.SubmitChanges();
            }
        }
        public void UpdateImage2(string gg, int identity, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Image i = db.Images.FirstOrDefault(j => j.ExpirationItemId == identity && j.IsBack == true);
                i.Path = gg;
                db.SubmitChanges();
            }
        }
        public void UpdateCourse(int itemid, string notes, DateTime date, int credits, string name)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Course c = db.Courses.FirstOrDefault(i => i.Id == itemid);
                c.Name = name;
                c.Notes = notes;
                c.Date = date;
                c.Credits = credits;
                db.SubmitChanges();
                AddAction(c.UserId, "Update Course " + c.Name, DateTime.Now);
            }
        }
        public void UpdateCoreCourse(int itemid, string notes, DateTime date, int category, string name)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                CoreCourse c = db.CoreCourses.FirstOrDefault(i => i.Id == itemid);
                c.Name = name;
                c.Notes = notes;
                c.Date = date;
                c.CatId = category;
                db.SubmitChanges();
                AddAction(c.UserId, "Update Core Course " + c.Name, DateTime.Now);
            }
        }
        public void AddReminder(int addReminder, int expirationItemId)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Reminder o = db.Reminders.FirstOrDefault(i => i.ExpirationItemId == expirationItemId);
                Reminder r = new Reminder();
                r.ExpirationItemId = o.ExpirationItemId;
                r.UserId = o.UserId;
                r.Days = addReminder;
                r.Date = o.ExpirationItem.ExpirationDate.AddDays(addReminder);
                db.Reminders.InsertOnSubmit(r);
                db.SubmitChanges();
                AddAction(r.UserId, "Add Reminder for " + r.ExpirationItem.Name + " " + r.ExpirationItem.Category.Name, DateTime.Now);
            }
        }
        public void UpdateNEItem(NonExpirationItem n)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                NonExpirationItem item = db.NonExpirationItems.FirstOrDefault(i => i.Id == n.Id);
                item.Name = n.Name;
                item.Notes = n.Notes;
                item.Date = n.Date;
                db.SubmitChanges();
                AddAction(item.UserId, "Update Non Expiration Item " + item.Name, DateTime.Now);
            }
        }
        public void DeleteNEItem(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var f = db.NonExpirationItems.FirstOrDefault(i => i.Id == itemid);
                db.NonExpirationItems.DeleteOnSubmit(f);
                db.SubmitChanges();
                AddAction(f.UserId, "Delete Non Expiration Item " + f.Name, DateTime.Now);
            }
        }
        public Image GetImageOne(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid && i.IsBack != true);
            }
        }
        public Image GetImageTwo(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid && i.IsBack == true);
            }
        }
        public bool CheckReminder(int expirationItemId, int addReminder)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                ExpirationItem i = db.ExpirationItems.FirstOrDefault(j => j.Id == expirationItemId);
                if (i.ExpirationDate.AddDays(-addReminder) < DateTime.Today)
                {
                    return false;
                }
                return true;
            }
        }
        public int GetPermission(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                return u.Permission;
            }
        }
        public User GetUser(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                return u;
            }
        }
        public IEnumerable<UserOrganization> GetAllUserRelationships(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<UserOrganization>(p => p.Organization);
                db.LoadOptions = loadOptions;
                return db.UserOrganizations.Where(i => i.UserId == userid).ToList();
            }
        }
        public IEnumerable<OrgRequiredItem> GetRequiredItems(int orgid, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<OrgRequiredItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                return db.OrgRequiredItems.Where(i => i.OrgId == orgid).ToList();
            }
        }
        public void AddSharedItems(int userid, int orgid, int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                ItemShareWithCompany i = new ItemShareWithCompany();
                i.UserId = userid;
                i.OrgId = orgid;
                i.ExpirationItemId = itemid;
                i.Share = false;
                db.ItemShareWithCompanies.InsertOnSubmit(i);
                db.SubmitChanges();
            }
        }
        public IEnumerable<ItemShareWithCompany> GetSharedItems(int orgid, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ItemShareWithCompany>(p => p.ExpirationItem);
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                db.LoadOptions = loadOptions;
                return db.ItemShareWithCompanies.Where(x => x.UserId == userid && x.OrgId == orgid).ToList();
            }
        }
        public bool SeeIfSharedItems(int orgid, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var list = db.ItemShareWithCompanies.Where(x => x.UserId == userid && x.OrgId == orgid);
                if (list.Count() < 1)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
        }
        public void UpdateSharedItem(IEnumerable<int> item, int userId, int orgId)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var list = db.ItemShareWithCompanies.Where(i => i.UserId == userId && i.OrgId == orgId);
                foreach (ItemShareWithCompany t in list)
                {
                    if (item.Contains(t.ExpirationItemId))
                    {
                        t.Share = true;
                    }
                    if (!item.Contains(t.ExpirationItemId))
                    {
                        t.Share = false;
                    }
                    db.SubmitChanges();

                }
                AddAction(userId, "Updated list of items to share with organization", DateTime.Now);
            }
        }
        public bool CheckIfImage(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var item = db.Images.FirstOrDefault(i => i.ExpirationItemId == itemid);
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
    }
}
