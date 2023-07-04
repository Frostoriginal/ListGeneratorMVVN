using ListGenerator.Core.ViewModels;
using ListGenerator.Database.Entities;
using ListGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListGenerator.View
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Employees_View : UserControl
    {
        public Employees_View()
        {
            InitializeComponent();            

            var dt = new Employees_ViewModel();
            DataContext = dt;

            DepartmentsListBox.ItemsSource =  DatabaseLocator.Database.Departments.ToList();
                                   

            dt.NewEmployeeDepartment = newDepartment.DepartmentName;
            viewModelRelay = dt;
            ErrorMessageLocal = viewModelRelay.ErrorMessage;


        }
        
        public Employees_ViewModel viewModelRelay = new Employees_ViewModel();

        public string ErrorMessageLocal = "";

        #region Department related
       
       // public List<Department> departments = new List<Department>();
        
        Department newDepartment = new Department() { DepartmentName = "" };
        
        #endregion

        private void DepartmentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newDepartment.DepartmentName = DepartmentsListBox.SelectedValue.ToString();
        }
              

        private void ButtonUpdateAdd_Click(object sender, RoutedEventArgs e)
        {
            viewModelRelay.AddNewEmployee();

            ErrorMessageLocal = viewModelRelay.ErrorMessage;
            if (ErrorMessageLocal != "") DisplayErrorMessage();

        }

        private void ButtonUpdateDelete_Click(object sender, RoutedEventArgs e)
        {
            viewModelRelay.DeleteSelectedEmployee();

            ErrorMessageLocal = viewModelRelay.ErrorMessage;
            if (ErrorMessageLocal != "") DisplayErrorMessage();

        }

        private void DisplayErrorMessage()
        {
            if (ErrorMessageLocal != "")
            {
                MessageBox.Show(ErrorMessageLocal, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ErrorMessageLocal = "";

        }
    }
}
