using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository
{
    public class NoteRepository
    {
        public IEnumerable<Category> GetAllCategories()
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                return db.Categories.ToList();
            }
        }
        public void AddNote(int userid, int category, string subject, string notes)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Note n = new Note();
                n.userId = userid;
                n.categoryId = category;
                n.Subject = subject;
                n.notes = notes;
                db.Notes.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
        public IEnumerable<Note> GetAllNotes(int userid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Note>(p => p.User);
                loadOptions.LoadWith<Note>(p => p.Category);
                db.LoadOptions = loadOptions;
                return db.Notes.Where(j => j.userId == userid).OrderBy(i => i.Id).ToList();
            }
        }
        public void DeleteNote(int itemid)
        {
            using (DataLayerDataContext db = new DataLayerDataContext())
            {
                Note c = db.Notes.FirstOrDefault(i => i.Id == itemid);
                db.Notes.DeleteOnSubmit(c);
                db.SubmitChanges();
            }
        }
    }
}
