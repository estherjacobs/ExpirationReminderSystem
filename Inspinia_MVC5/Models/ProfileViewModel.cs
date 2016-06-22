using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<UserOrganization> Organizations { get; set; }
    }
}