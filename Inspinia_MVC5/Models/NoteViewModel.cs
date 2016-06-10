using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class NoteViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Note> Notes { get; set; }
    }
}