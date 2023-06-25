using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListGenerator.Database;

namespace ListGenerator.Core
{
    public class DatabaseLocator
    {
        public static EmployeeDbContext Database { get; set; }

    }
}
