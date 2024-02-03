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
        public ICommand DeleteSelectedEmployeeCommand { get; set; }
        public ICommand MoveUpSelectedEmployeeCommand { get; set; }
        public ICommand MoveDownSelectedEmployeeCommand { get; set; }


        public Employees_ViewModel()
        {
            AddNewEmployeeToListCommand = new RelayCommand(AddNewEmployee);
            DeleteSelectedEmployeeCommand = new RelayCommand(DeleteSelectedEmployee);
          //  MoveUpSelectedEmployeeCommand = new RelayCommand(MoveUpSelectedEmployee);

            foreach (var Employee in DatabaseLocator.Database.Employees.OrderBy(s=>s.EmployeeOrder).ToList())  //orderby department then position
            {
                EmployeeList.Add(new EmployeeViewModel
                {
                    Id = Employee.Id,
                    EmployeeName = Employee.EmployeeName,
                    EmployeeSurname = Employee.EmployeeSurname,
                    EmployeeDepartment = Employee.EmployeeDepartment,
                    EmployeeNameAndSurname = $"{Employee.EmployeeName} {Employee.EmployeeSurname}",
                    EmployeeOrder = Employee.EmployeeOrder

                });
            }

            //EmployeeList.Add(new EmployeeViewModel
            //{
            //    Id = 1,
            //    EmployeeName = "ForcedInput",
            //    EmployeeSurname = " ",
            //    EmployeeDepartment = " ",
            //    EmployeeNameAndSurname = $"Forced by Code",
            //    EmployeeOrder = 1

            //}); ;



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

            if (string.IsNullOrEmpty(NewEmployeeDepartment))
            {
                validInput = false;
                if (ErrorMessage != "") ErrorMessage += "\n";
                ErrorMessage += "Brakuje działu, dodaj działy do wyboru w sekcji Działy";
            }


            if (validateTheInput(NewEmployeeName, "Imię") != true) validInput = false;
            if (validateTheInput(NewEmployeeSurname, "Nazwisko") != true) validInput = false;




            if (validInput)
            {
                //normalize input
                NewEmployeeName = normalizeTheInput(NewEmployeeName);
                NewEmployeeSurname = normalizeTheInput(NewEmployeeSurname);
                int currentMaxOrder = 1;
                if (EmployeeList.Count > 0)
                {
                   currentMaxOrder = EmployeeList.Max(x => x.EmployeeOrder) + 1;
                }
                
                int employeeOder = currentMaxOrder > 1 ? currentMaxOrder : 1;
                var newEmployee = new EmployeeViewModel
                {
                    EmployeeName = NewEmployeeName,
                    EmployeeSurname = NewEmployeeSurname,
                    EmployeeDepartment = NewEmployeeDepartment,
                    EmployeeNameAndSurname = $"{NewEmployeeName} {NewEmployeeSurname}",
                    EmployeeOrder = employeeOder,

                };


                

                EmployeeList.Add(newEmployee);

                DatabaseLocator.Database.Employees.Add(new Employee
                {
                    Id = newEmployee.Id,
                    EmployeeName = newEmployee.EmployeeName,
                    EmployeeSurname = newEmployee.EmployeeSurname,
                    EmployeeDepartment = newEmployee.EmployeeDepartment,
                    EmployeeOrder = newEmployee.EmployeeOrder,
                });

                DatabaseLocator.Database.SaveChanges();

                NewEmployeeName = string.Empty;
                NewEmployeeSurname = string.Empty;

            }

        }

        public void DeleteSelectedEmployee()
        {
            var selectedEmployees = EmployeeList.Where(x => x.IsSelected).ToList();
            if (selectedEmployees.Count != 0)
            {
                foreach (var employee in selectedEmployees)
                {
                    EmployeeList.Remove(employee);
                    var foundEntity = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.Id == employee.Id);

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

        public void MoveUpSelectedEmployee()
        {
            ErrorMessage = "";
            var selectedEmployees = EmployeeList.Where(x => x.IsSelected).ToList();
            EmployeeViewModel selected = new EmployeeViewModel();
            
            if (selectedEmployees.Count == 1)
            {
                selected = selectedEmployees[0];
                EmployeeViewModel[] EmployeeSwitchArray = EmployeeList.ToArray();
                int position = Array.IndexOf(EmployeeSwitchArray, selected);
                if (position > 0)
                {
                    EmployeeViewModel temp = EmployeeSwitchArray[position - 1];
                    EmployeeSwitchArray[position - 1] = selectedEmployees[0];
                    EmployeeSwitchArray[position] = temp;
                    EmployeeList = new ObservableCollection<EmployeeViewModel>(EmployeeSwitchArray);

                    int currentPosition = selected.EmployeeOrder;

                    var goesUp = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.EmployeeOrder == currentPosition - 1);
                    var goesDown = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.Id == selected.Id);

                    if (goesUp != null && goesDown != null)
                    {
                        goesUp.EmployeeOrder = currentPosition;
                        goesDown.EmployeeOrder = currentPosition - 1;
                        DatabaseLocator.Database.Employees.Update(goesUp);
                        DatabaseLocator.Database.Employees.Update(goesDown);

                    }

                    DatabaseLocator.Database.SaveChanges();
                }
            }
            
            else
            {
                ErrorMessage = "Zaznacz jednego pracownika do przesunięcia.";
            }

        }

        public void MoveDownSelectedEmployee()
        {
            ErrorMessage = "";
            var selectedEmployees = EmployeeList.Where(x => x.IsSelected).ToList();
            EmployeeViewModel selected = new EmployeeViewModel();

            if (selectedEmployees.Count == 1)
            {
                selected = selectedEmployees[0];
                EmployeeViewModel[] EmployeeSwitchArray = EmployeeList.ToArray();
                int position = Array.IndexOf(EmployeeSwitchArray, selected);
                if (position >= 0 && position < EmployeeSwitchArray.Length-1)
                {
                    EmployeeViewModel temp = EmployeeSwitchArray[position + 1];
                    EmployeeSwitchArray[position + 1] = selectedEmployees[0];
                    EmployeeSwitchArray[position] = temp;
                    EmployeeList = new ObservableCollection<EmployeeViewModel>(EmployeeSwitchArray);
                    
                    int currentPosition = selected.EmployeeOrder;
                                       
                        var goesUp = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.EmployeeOrder == currentPosition+1);
                        var goesDown = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.Id == selected.Id);

                    if (goesUp != null && goesDown !=null)
                        {
                        goesUp.EmployeeOrder = currentPosition;
                        goesDown.EmployeeOrder = currentPosition + 1;
                        DatabaseLocator.Database.Employees.Update(goesUp);
                        DatabaseLocator.Database.Employees.Update(goesDown);

                    }
                    
                    DatabaseLocator.Database.SaveChanges();

                }
            }

            else
            {
                ErrorMessage = "Zaznacz jednego pracownika do przesunięcia.";
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
                if (!char.IsLetter(item))
                {
                    invalidCharacter = true;
                    if (item == '-') invalidCharacter = false;
                    if (item == ' ') invalidCharacter = false;
                }
            }
            if (invalidCharacter) errors.Add($"Pole {caller} zawiera niedozwolone znaki.");


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
