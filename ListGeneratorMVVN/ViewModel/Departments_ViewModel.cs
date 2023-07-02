using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ListGenerator.Core.ViewModels;
using ListGenerator.Database;
using ListGenerator.Database.Entities;
using ListGenerator.Model;
using ListGenerator.Core;
using ListGenerator.Core.ViewModels;
using ListGenerator.Core.ViewModels.Controls;

namespace ListGenerator.ViewModel
{
    class Departments_ViewModel : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public int CustomerID
        {
            get { return _pageModel.CustomerCount; }
            set { _pageModel.CustomerCount = value; OnPropertyChanged(); }
        }

        public List<Department> departments
        {
            get { return _pageModel.DepartmentList; }
            set { _pageModel.DepartmentList = value; OnPropertyChanged(); }
                
        }

        public ObservableCollection<DepartmentViewModel> DepartmentList { get; set; } = new ObservableCollection<DepartmentViewModel>();

        public ICommand AddNewDepartmentToListCommand { get; set; }

        public ICommand DeleteSelectedDepartmentCommand { get; set; }
        public string NewDepartmentName { get; set; } = "";
        public string ErrorMessage { get; set; } = "";


        public Departments_ViewModel()
        {
            _pageModel = new PageModel();
            AddNewDepartmentToListCommand = new RelayCommand(AddNewEmployee);
            DeleteSelectedDepartmentCommand = new RelayCommand(DeleteSelectedEmployee);

            foreach (var Department in DatabaseLocator.Database.Departments.ToList())
            {
                DepartmentList.Add(new DepartmentViewModel
                {
                    Id = Department.Id,
                    DepartmentName = Department.DepartmentName,                    

                });
            }


        }

        public void AddNewEmployee()
        {

            ErrorMessage = "";
            bool validInput = true;

            if (string.IsNullOrEmpty(NewDepartmentName))
            {
                validInput = false;
                ErrorMessage = "Pole Imię jest puste";
            }
                        

            if (validInput)
            {
                //normalize input
                

                var newDepartment = new DepartmentViewModel
                {
                    DepartmentName = NewDepartmentName,                    
                };

                DepartmentList.Add(newDepartment);

                DatabaseLocator.Database.Departments.Add(new Department
                {
                    Id = newDepartment.Id,
                    DepartmentName = newDepartment.DepartmentName,
                    
                });

                DatabaseLocator.Database.SaveChanges();

                NewDepartmentName = string.Empty;                

            }

        }

        public void DeleteSelectedEmployee()
        {
            var selectedDepartments = DepartmentList.Where(x => x.IsSelected).ToList();
            if (selectedDepartments.Count != 0)
            {
                foreach (var Department in selectedDepartments)
                {
                    DepartmentList.Remove(Department);
                    var foundEntity = DatabaseLocator.Database.Departments.FirstOrDefault(x => x.Id == Department.Id);

                    if (foundEntity != null)
                    {
                        DatabaseLocator.Database.Departments.Remove(foundEntity);
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
    }
}
