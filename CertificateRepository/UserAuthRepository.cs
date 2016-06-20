using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class UserAuthRepository
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
        public User AddUser(string name, string password, string phone, string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = new User();
                u.FullName = name;
                u.Email = email;
                u.DateCreated = DateTime.Now.Date;
                u.PhoneNumber = phone;
                u.IsActive = true;
                u.ViaEmail = true;
                u.Salt = PasswordHelper.GenerateSalt();
                u.HashedPassword = PasswordHelper.HashPassword(password, u.Salt);
                db.Users.InsertOnSubmit(u);
                db.SubmitChanges();
                AddAction(u.Id, " Register", DateTime.Today);
                return u;
            }
        }

        public Organization AddOrg(int userid, string name, string address, string email, string city, string state, string zip, string phone, int year)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Organization o = new Organization();
                o.Name = name;
                o.Address = address;
                o.Email = email;
                o.City = city;
                o.City = state;
                o.City = zip;
                o.PhoneNumber = phone;
                o.YearFounded = year;
                o.Date = DateTime.Now;
                db.Organizations.InsertOnSubmit(o);
                db.SubmitChanges();
                AddOrgAction(o.Id, userid, o.Name + " registered", DateTime.Today);
                return o;
            }
        }

        public void CreateInitialUserOrdRel(int orgid, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                UserOrganization l = new UserOrganization();
                l.OrgId = orgid;
                l.UserId = userid;
                l.Permission = 3;
                l.Date = DateTime.Now;
                db.UserOrganizations.InsertOnSubmit(l);
                db.SubmitChanges();
            }
        }
        public void CreateOrgReqItems(int orgid, int catid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                OrgRequiredItem l = new OrgRequiredItem();
                l.OrgId = orgid;
                l.CatId = catid;
                db.OrgRequiredItems.InsertOnSubmit(l);
                db.SubmitChanges();
            }
        }

        public bool CheckIfEmailExist(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Email == email);
                if (i == null)
                {
                    return false;
                }
                return true;
            }
        }

        public User GetUserResetPassword(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Email == email);
                if (i == null)
                {
                    return null;
                }
                return i;
            }
        }
        public User GetUser(string email, string password)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Email == email);
                if (i == null)
                {
                    return null;
                }
                bool correctPassword = PasswordHelper.PasswordMatch(password, i.HashedPassword, i.Salt);
                return correctPassword ? i : null;
            }
        }
        public bool checkIfEmailExist(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(y => y.Email == email);
                if (u != null)
                {
                    return false;
                }
                return true;
            }
        }
        public void UpdatePassword(string password, int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Id == userid);
                i.HashedPassword = PasswordHelper.HashPassword(password, i.Salt);
                db.SubmitChanges();
            }
        }

        public static class PasswordHelper
        {
            public static string GenerateSalt()
            {
                using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
                {
                    byte[] bytes = new byte[15];
                    provider.GetBytes(bytes);
                    return Convert.ToBase64String(bytes);
                }
            }
            public static string HashPassword(string password, string salt)
            {
                SHA256Managed managed = new SHA256Managed();
                byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
                byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                byte[] combined = passwordBytes.Concat(saltBytes).ToArray();
                return Convert.ToBase64String(managed.ComputeHash(combined));
            }

            public static bool PasswordMatch(string input, string passwordHash, string salt)
            {
                string inputHash = HashPassword(input, salt);
                return inputHash == passwordHash;
            }
        }
    }
}
