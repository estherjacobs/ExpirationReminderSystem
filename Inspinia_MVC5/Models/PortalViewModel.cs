using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificateRepository;

namespace Inspinia_MVC5.Models
{
    public class PortalViewModel
    {
        public IEnumerable<ExpirationItem> ExpirationItems { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<NonExpirationItem> NonExpirationItems { get; set; }
        public IEnumerable<Reminder> Reminders { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<CoreCourse> CoreCourses { get; set; }
        public IEnumerable<UserOrganization> UserOrganizations { get; set; }
    }
}