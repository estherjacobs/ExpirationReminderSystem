using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class MemberProfileViewModel
    {
        public Organization Organization { get; set; }
        public IEnumerable<UserOrganization> Relationships { get; set; }
    }
}