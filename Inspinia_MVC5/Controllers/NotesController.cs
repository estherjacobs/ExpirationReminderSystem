using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;
using CertificateRepository;

namespace Inspinia_MVC5.Controllers
{
    public class NotesController : Controller
    {
        public ActionResult Index()
        {
            NoteViewModel vm = new NoteViewModel();
            var mgr = new NoteRepository();
            vm.Categories = mgr.GetAllCategories();
            var notes = mgr.GetAllNotes(int.Parse(User.Identity.Name));
            if (notes != null)
            {
                vm.Notes = notes;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddNote(int category, string notes, string subject)
        {
            var mgr = new NoteRepository();
            mgr.AddNote(int.Parse(User.Identity.Name), category, subject, notes);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteNote(int itemid)
        {
            var mgr = new NoteRepository();
            mgr.DeleteNote(itemid);
            return RedirectToAction("Index");
        }
    }
}