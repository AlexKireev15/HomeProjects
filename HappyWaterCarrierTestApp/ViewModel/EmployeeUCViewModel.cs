using HappyWaterCarrierTestApp.Model;
using HappyWaterCarrierTestApp.Utils;
using HappyWaterCarrierTestApp.Utils.Pagination;
using HappyWaterCarrierTestApp.ViewModel.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.ViewModel
{
    class EmployeeUCViewModel : INotifyPropertyChanged
    {
        private Command addEmployeeCommand;
        private Command deleteEmployeeCommand;
        private Command updateEmployeeCommand;
        public Command AddEmployeeCommand
        {
            get
            {
                return addEmployeeCommand ??
                    (
                        addEmployeeCommand = new Command(obj =>
                        {
                            NHibernateHelper.GetInstance()
                                .SaveAsync(
                                    new Employee(NewEmployee),
                                    (o) => App.Current.Dispatcher.BeginInvoke((Action)delegate () {
                                        Pagination.Put(o);
                                    }));
                        })
                    );
            }
        }
        public Command DeleteEmployeeCommand
        {
            get
            {
                return deleteEmployeeCommand ??
                    (
                        deleteEmployeeCommand = new Command(obj =>
                        {
                            if (obj == null || obj.GetType() != typeof(Employee))
                                return;
                            NHibernateHelper.GetInstance()
                                .DeleteAsync(
                                    (Employee)obj,
                                    (o) => App.Current.Dispatcher.BeginInvoke((Action)delegate () {
                                        Pagination.Remove(o);
                                    }));
                            Console.WriteLine("Deleting element " + ((Employee)obj).ID);
                            
                        })
                    );
            }
        }
        public Command UpdateEmployeeCommand
        {
            get
            {
                return updateEmployeeCommand ??
                    (
                        updateEmployeeCommand = new Command(obj =>
                        {
                            NHibernateHelper.GetInstance().Update((Employee)obj);
                        })
                    );
            }
        }

        public Employee NewEmployee { get; set; }
        //private ObservableCollection<Employee> employees;
        //public ObservableCollection<Employee> Employees { get { return employees; } set { employees = value; } }
        public PaginationHelper<Employee> Pagination { get; set; }
        public EmployeeUCViewModel()
        {
            var employees = new ObservableCollection<Employee>(NHibernateHelper.GetInstance().Get<Employee>(0, Constants.PAGE_SIZE));
            Pagination = new ScrollPaginationHelper<Employee>(ref employees);
            NewEmployee = new Employee { Birthday = DateTime.Now };
        }

        internal void LoadNextElement()
        {
            Pagination.MoveDown(1);
        }

        internal void LoadPrevElement()
        {
            Pagination.MoveUp(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
