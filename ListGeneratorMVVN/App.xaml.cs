using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ListGenerator.Core;
using ListGenerator.Database;

namespace ListGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) //ovveride do wprowadzenia zmian przy uruchomieniu aplikacji - punkt wejśćia

        {
            base.OnStartup(e);

            var database = new EmployeeDbContext();

            database.Database.EnsureCreated();

            DatabaseLocator.Database = database;


        }
    }
}
