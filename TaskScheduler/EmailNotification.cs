using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public class EmailNotification
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ItemName { get; set; }
        public string ExpirationDate { get; set; }
        public string FrontImage { get; set; }
        public string Note { get; set; }
    }
}
