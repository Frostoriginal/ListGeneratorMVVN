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
        
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeDepartment { get; set; }

        

    }
}
