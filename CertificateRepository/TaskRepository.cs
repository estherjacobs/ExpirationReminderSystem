using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class TaskRepository
    {
        public IEnumerable<Reminder> GetAllTodaysReminders()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Reminder>(p => p.ExpirationItem);
                loadOptions.LoadWith<Reminder>(p => p.User);
                loadOptions.LoadWith<ExpirationItem>(p => p.Category);
                loadOptions.LoadWith<ExpirationItem>(p => p.Images);
                loadOptions.LoadWith<User>(p => p.Contacts);
                db.LoadOptions = loadOptions;
                return db.Reminders.Where(i => i.Date == DateTime.Now).ToList();
            }
        }
    }
}
