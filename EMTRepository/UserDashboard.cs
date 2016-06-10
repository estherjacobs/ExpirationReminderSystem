using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMTRepository
{
    public class UserDashboard
    {

        public IEnumerable<ExpirationItem> GetAllExpirationItems(int UserId)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.ExpirationItems;
            }
        }

        public void AddUser(string name, string password, string phone, string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User u = new User();
                u.FullName = name;
                u.Email = email;
                u.PhoneNumber = phone;
                u.Salt = PasswordHelper.GenerateSalt();
                u.HashedPassword = PasswordHelper.HashPassword(password, u.Salt);
                db.Users.InsertOnSubmit(u);
                db.SubmitChanges();
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
