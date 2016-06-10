using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class MemberViewModel
    {
        public AddMemberToken Token { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<CoreCourse> CoreCourses { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<ExpirationItem> Items { get; set; }
        public IEnumerable<OrgRequiredItem> Required { get; set; }
        public User UserDetails { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<UserOrganization> UserOrganizations { get; set; }
    }
}