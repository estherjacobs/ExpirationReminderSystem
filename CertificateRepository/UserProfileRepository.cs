using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class UserProfileRepository
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
        public User GetUser(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
               
                loadOptions.LoadWith<User>(p => p.Courses);
                loadOptions.LoadWith<User>(p => p.ExpirationItems);
                loadOptions.LoadWith<User>(p => p.Contacts);
                db.LoadOptions = loadOptions;
                return db.Users.FirstOrDefault(i => i.Id == userid);
            }
        }
        public IEnumerable<UserOrganization> GetOrganization(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<UserOrganization>(p => p.Organization);
                loadOptions.LoadWith<UserOrganization>(p => p.User);
                db.LoadOptions = loadOptions;
                var list = db.UserOrganizations.Where(i => i.UserId == userid).ToList();
                return list;
            }
        }
        public Contact GetContact(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Contacts.FirstOrDefault(i => i.UserId == userid);
            }
        }
        public void UpdateAccount(int userid, string name, string email, string phone)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                u.FullName = name;
                u.Email = email;
                u.PhoneNumber = phone;
                db.SubmitChanges();
                AddAction(userid, "Update Account Info", DateTime.Now);
            }
        }
        public void UpdatePassword(int userid, string newPasssword)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                u.HashedPassword = PasswordHelper.HashPassword(newPasssword, u.Salt);
                AddAction(userid, "Update Password", DateTime.Now);
                db.SubmitChanges();
            }
        }

        public void UpdateNotification(int userid, string radio, string name, string phone, string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                if (radio == "1")
                {
                    u.ViaEmail = true;
                    u.ViaText = false;
                }
                if (radio == "2")
                {
                    u.ViaText = true;
                    u.ViaEmail = false;
                }
                if (radio == "3")
                {
                    u.ViaText = true;
                    u.ViaEmail = true;
                }
                Contact f = db.Contacts.FirstOrDefault(i => i.UserId == userid);
                if (f == null)
                {
                    Contact c = new Contact();
                    c.Name = name;
                    c.UserId = userid;
                    c.Email = email;
                    c.Phone = phone;
                    db.Contacts.InsertOnSubmit(c);
                }
                else
                {
                    f.Name = name;
                    f.Email = email;
                    f.Phone = phone;
                }
                AddAction(userid, "Update Notification settings", DateTime.Now);
                db.SubmitChanges();
            }
        }
        public bool CheckPassword(int userid, string password)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = db.Users.FirstOrDefault(i => i.Id == userid);
                bool correctPassword = PasswordHelper.PasswordMatch(password, u.HashedPassword, u.Salt);
                return correctPassword ? true : false;
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
