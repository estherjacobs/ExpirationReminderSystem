using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class ResetPasswordRepository
    {
        public PasswordToken CreateToken(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Email == email);
                Guid g = Guid.NewGuid();
                string gg = g.ToString();
                PasswordToken p = new PasswordToken
                {
                    userid = i.Id,
                    guid = gg,
                    date = DateTime.Now
                };
                db.PasswordTokens.InsertOnSubmit(p);
                db.SubmitChanges();
                return p;
            }
        }
        public User GetUser(string email)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                User i = db.Users.FirstOrDefault(u => u.Email == email);
                return i;
            }
        }
        public bool CheckTokenValidity(string token)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                PasswordToken p = db.PasswordTokens.FirstOrDefault(i => i.guid == token);
                if (p == null || p.date < DateTime.Now.AddDays(-3))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public User GetUserFromToken(string token)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                PasswordToken p = db.PasswordTokens.FirstOrDefault(i => i.guid == token);
                return db.Users.FirstOrDefault(i => i.Id == p.userid);
            }
        }
        public void DeleteToken(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                PasswordToken p = db.PasswordTokens.FirstOrDefault(i => i.userid == userid);
                db.PasswordTokens.DeleteOnSubmit(p);
                db.SubmitChanges();
            }
        }
    }
}
