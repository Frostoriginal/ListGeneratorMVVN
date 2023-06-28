using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ListGenerator.Database;

namespace ListGenerator.Core.ViewModels
{
    public class EmployeesPageViewModel:BaseViewModel
    {
        public ObservableCollection<EmployeeViewModel> EmployeeList { get; set; } = new ObservableCollection<EmployeeViewModel>();

        public string NewEmployeeName { get; set; }
        public string NewEmployeeSurname { get;set; }
        public string NewEmployeeDepartment { get; set; }

        public DateTime timeSelectedReference { get; set; }
        public string timeSelectedString { get; set; }
        public ICommand AddNewTaskToListCommand { get; set; }

        public ICommand DeleteSelectedTasksCommand  { get; set; }

        public EmployeesPageViewModel()
        {
            AddNewTaskToListCommand = new RelayCommand(AddNewTask);
            DeleteSelectedTasksCommand = new RelayCommand(DeleteSelectedTask);

            foreach (var task in DatabaseLocator.Database.Employees.ToList())
            {
                EmployeeList.Add(new EmployeeViewModel
                {   Id = task.Id,
                    EmployeeName = task.EmployeeName,
                    EmployeeSurname = task.EmployeeSurname,
                    EmployeeDepartment = task.EmployeeDepartment,
                    

                });
            }
        }

        
        private void AddNewTask() 
        {
            var newTask = new EmployeeViewModel
            { EmployeeName = NewEmployeeName,          
              EmployeeSurname = NewEmployeeSurname,
              EmployeeDepartment = NewEmployeeDepartment,
              
                    };

            EmployeeList.Add(newTask);

            DatabaseLocator.Database.Employees.Add(new Employee
            {   Id = newTask.Id,
                EmployeeName = newTask.EmployeeName,
                EmployeeSurname = newTask.EmployeeSurname,
                EmployeeDepartment = newTask.EmployeeDepartment,
                
            });

            DatabaseLocator.Database.SaveChanges();

            NewEmployeeName = string.Empty;
            NewEmployeeSurname = string.Empty;

           // OnPropertyChanged(nameof(NewWorkTaskTitle));
            //OnPropertyChanged(nameof(NewWorkTaskDescription));
        }

        private void DeleteSelectedTask()
        {
            var selectedTasks = EmployeeList.Where(x => x.IsSelected).ToList(); 
            
            foreach (var task in selectedTasks)
            {
                EmployeeList.Remove(task);
                var foundEntity = DatabaseLocator.Database.Employees.FirstOrDefault(x => x.Id == task.Id);   
                
                if(foundEntity != null)
                {
                    DatabaseLocator.Database.Employees.Remove(foundEntity);
                }
                
            }

            DatabaseLocator.Database.SaveChanges();
        }
    }
}
