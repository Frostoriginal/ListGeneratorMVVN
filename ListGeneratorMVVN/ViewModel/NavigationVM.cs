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

        public ICommand HomeCommand { get; set; }
        public ICommand Departments_ViewCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand TransactionsCommand { get; set; }
        public ICommand ShipmentsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand Employees_ViewCommand { get; set; }


        private void Home(object obj) => CurrentView = new HomeVM();
        private void Departments_View(object obj) => CurrentView = new Departments_ViewModel();
        private void Product(object obj) => CurrentView = new ProductVM();
        private void Order(object obj) => CurrentView = new OrderVM();
        private void Transaction(object obj) => CurrentView = new TransactionVM();
        private void Shipment(object obj) => CurrentView = new ShipmentVM();
        private void Setting(object obj) => CurrentView = new SettingVM();

        private void Employees_View(object obj) => CurrentView = new Employees_ViewModel();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            Departments_ViewCommand = new RelayCommand(Departments_View);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);
            TransactionsCommand = new RelayCommand(Transaction);
            ShipmentsCommand = new RelayCommand(Shipment);
            SettingsCommand = new RelayCommand(Setting);
            Employees_ViewCommand = new RelayCommand(Employees_View);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
