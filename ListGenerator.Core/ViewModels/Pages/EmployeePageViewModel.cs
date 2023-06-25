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

        public string NewWorkTaskTitle { get; set; }
        public string NewWorkTaskDescription { get;set; }

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
                    Title = task.Title,
                    Description = task.Description,
                    CreatedDate = task.CreatedDate,

                });
            }
        }
        private void AddNewTask() 
        {
            var newTask = new EmployeeViewModel
            { Title = NewWorkTaskTitle,          
              Description = NewWorkTaskDescription,
              CreatedDate = DateTime.Now
                    };

            EmployeeList.Add(newTask);

            DatabaseLocator.Database.Employees.Add(new Employee
            {   Id = newTask.Id,
                Title = newTask.Title,
                Description = newTask.Description,
                CreatedDate = newTask.CreatedDate,
            });

            DatabaseLocator.Database.SaveChanges();

            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;

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
