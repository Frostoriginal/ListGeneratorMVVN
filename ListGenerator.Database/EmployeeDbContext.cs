using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListGenerator.Database.Entities;
using Microsoft.EntityFrameworkCore;


namespace ListGenerator.Database
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //My Documents
            //optionsBuilder.UseSqlite($"Filename={Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),"EmploDB.sqlite")}");
            //Program folder
            optionsBuilder.UseSqlite("Filename=EmployeesDB.sqlite");

        }
    }
}
