using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListGenerator.Utilities;
using System.Windows.Input;

namespace ListGenerator.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand Generate_ViewCommand { get; set; }
        public ICommand Employees_ViewCommand { get; set; }
        public ICommand Departments_ViewCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        

        private void Generate_View(object obj) => CurrentView = new Generate_ViewModel();
        private void Employees_View(object obj) => CurrentView = new Employees_ViewModel();
        private void Departments_View(object obj) => CurrentView = new Departments_ViewModel();    
        private void Setting(object obj) => CurrentView = new SettingVM();      
        

        public NavigationVM()
        {
            Generate_ViewCommand = new RelayCommand(Generate_View);
            Employees_ViewCommand = new RelayCommand(Employees_View);
            Departments_ViewCommand = new RelayCommand(Departments_View);
            SettingsCommand = new RelayCommand(Setting);            

            // Startup Page
            CurrentView = new Generate_ViewModel();
        }
    }
}
