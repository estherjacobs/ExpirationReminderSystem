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
                AddAction(u.Id, "Register", DateTime.Today);
                return u;
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
