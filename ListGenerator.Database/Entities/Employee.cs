using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Database
{
    public class Employee
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
