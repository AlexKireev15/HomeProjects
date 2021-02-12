using HappyWaterCarrierTestApp.Utils;
using HappyWaterCarrierTestApp.Utils.Pagination;
using HappyWaterCarrierTestApp.ViewModel.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.ViewModel
{
    abstract class BaseScrollUCViewModel<T>// : INotifyPropertyChanged
    {/*
        abstract public Command AddCommand { get; set; }
        abstract public Command DeleteCommand { get; set; }
        abstract public Command UpdateCommand { get; set; }

        abstract public T NewItem { get; set; }
        private ObservableCollection<T> items;
        public ObservableCollection<T> Items { get { return employees; } set { employees = value; } }
        private PaginationHelper<Employee> Pagination;
        public BaseScrollUCViewModel()
        {
            Employees = new ObservableCollection<Employee>(NHibernateHelper.GetInstance().Get<Employee>(0, Constants.PAGE_SIZE));
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
        }*/
    }
}
