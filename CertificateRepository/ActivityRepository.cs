using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository;

namespace CertificateRepository
{
    public class ActivityRepository
    {
        public IEnumerable<Action> GetAllActivity(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Actions.Where(i => i.UserId == userid).ToList();
            }
        }
    }
}
