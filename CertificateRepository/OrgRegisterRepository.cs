using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class OrgRegisterRepository
    {
        public IEnumerable<Category> GetAllCategories()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Categories.ToList();
            }
        }
    }
}
