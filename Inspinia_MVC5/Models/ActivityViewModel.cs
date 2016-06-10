using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class ActivityViewModel
    {
        public IEnumerable<CertificateRepository.Action> Actions { get; set; }
    }
}