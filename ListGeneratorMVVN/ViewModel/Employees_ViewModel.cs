using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ListGenerator.Database;
using ListGenerator.Core;
using ListGenerator.Core.ViewModels;
using ListGenerator.Model;

namespace ListGenerator.ViewModel
{
    public class Employees_ViewModel : BaseViewModel
    {
        public ObservableCollection<EmployeeViewModel> EmployeeList { get; set; } = new ObservableCollection<EmployeeViewModel>();
        public string NewEmployeeName { get; set; } = "";
        public string NewEmployeeSurname { get; set; } = "";
        public string NewEmployeeDepartment { get; set; } = "";               
        public string ErrorMessage { get; set; } = "";
        public ICommand AddNewEmployeeToListCommand { get; set; }
        public ICommand DeleteSelectedEmployeeCommand  { get; set; }

        private readonly PageModel _pageModel;
        
                
        public Employees_ViewModel()
        {
            _pageModel = new PageModel();
            AddNewEmployeeToListCommand = new RelayCommand(AddNewEmployee);
            DeleteSelectedEmployeeCommand = new RelayCommand(DeleteSelectedEmployee);

            foreach (var Employee in DatabaseLocator.Database.Employees.ToList())
            {
                EmployeeList.Add(new EmployeeViewModel
                {   Id = Employee.Id,
                    EmployeeName = Employee.EmployeeName,
                    EmployeeSurname = Employee.EmployeeSurname,
                    EmployeeDepartment = Employee.EmployeeDepartment,  
                    EmployeeNameAndSurname = $"{Employee.EmployeeName} {Employee.EmployeeSurname}"

                });
            }
            
        }

        
        public void AddNewEmployee() 
        {
            
            ErrorMessage = "";
            bool validInput = true;
            
            if (string.IsNullOrEmpty(NewEmployeeName))
            { 
                validInput = false;
                ErrorMessage = "Pole Imię jest puste";
            }

            if (string.IsNullOrEmpty(NewEmployeeSurname))
            {
                validInput = false;
                if (ErrorMessage != "") ErrorMessage += "\n";
                ErrorMessage += "Pole Nazwisko jest puste";
            }
            
            if (validateTheInput(NewEmployeeName, "Imię") != true) validInput = false;
            if (validateTheInput(NewEmployeeSurname, "Nazwisko") != true) validInput = false;


            

            if (validInput)
            {
                //normalize input
                NewEmployeeName = normalizeTheInput(NewEmployeeName);
                NewEmployeeSurname = normalizeTheInput(NewEmployeeSurname);
                
                var newEmployee = new EmployeeViewModel
                { EmployeeName = NewEmployeeName,          
                  EmployeeSurname = NewEmployeeSurname,
                  EmployeeDepartment = NewEmployeeDepartment,
                  EmployeeNameAndSurname = $"{NewEmployeeName} {NewEmployeeSurname}"
                };

                EmployeeList.Add(newEmployee);
                
                DatabaseLocator.Database.Employees.Add(new Employee
                {   Id = newEmployee.Id,
                    EmployeeName = newEmployee.EmployeeName,
                    EmployeeSurname = newEmployee.EmployeeSurname,
                    EmployeeDepartment = newEmployee.EmployeeDepartment,                    
                });
                
                DatabaseLocator.Database.SaveChanges();

                NewEmployeeName = string.Empty;
                NewEmployeeSurname = string.Empty;                
                
            }
            
        }

        public void DeleteSelectedEmployee()
        {
            var selectedEmployees = EmployeeList.Where(x => x.IsSelected).ToList(); 
            if( selectedEmployees.Count != 0) {
                foreach (var task in selectedEmployees)
                {
                    EmployeeList.Remove(task);
                    var foundEntity = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.Id == task.Id);

                    if (foundEntity != null)
                    {
                        DatabaseLocator.Database.Employees.Remove(foundEntity);
                    }
                }
                DatabaseLocator.Database.SaveChanges();
            }
            else
            {                
                ErrorMessage = "Zaznacz wpis do usunięcia.";
            }            
            
        }

        public bool validateTheInput(string input, string caller)
        {
            List<string> errors = new List<string>();
            
            bool isValidInput = true;
            if (input.Length > 0 && input.Length < 2) errors.Add($"{caller} jest za krótkie.");
            if (input.Length > 25) errors.Add($"{caller} jest za długie.");

            bool invalidCharacter = false;
            foreach (char item in input)
            {
                if (!char.IsLetter(item)) invalidCharacter = true;                
            }
            if (invalidCharacter)  errors.Add($"Pole {caller} zawiera niedozwolone znaki."); 


            if (errors.Count > 0)
            {
                isValidInput = false;
                if (ErrorMessage != "") ErrorMessage += "\n";
                foreach (string error in errors)
                {
                    ErrorMessage += $"{error}\n";
                }               
            }           

            return isValidInput;
        }

        public string normalizeTheInput(string input)
        {
            input = input.ToLower();
            char[] temp = input.ToCharArray();
            temp[0] = char.ToUpper(temp[0]);

            string result = new string(temp);
            return result;
        }
    }


}
