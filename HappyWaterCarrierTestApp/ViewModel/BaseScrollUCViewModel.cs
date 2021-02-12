using HappyWaterCarrierTestApp.Utils;
using HappyWaterCarrierTestApp.Utils.Pagination;
using HappyWaterCarrierTestApp.ViewModel.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.ViewModel
{
    class BaseScrollUCViewModel<T> : INotifyPropertyChanged
        where T : class
    {
        private Command addCommand;
        private Command deleteCommand;
        private Command updateCommand;
        public virtual Command AddCommand
        {
            get
            {
                return addCommand ??
                    (
                        addCommand = new Command(obj =>
                        {
                            NHibernateHelper.GetInstance()
                                .SaveAsync(
                                    NewItem,
                                    (o) => App.Current.Dispatcher.BeginInvoke((Action)delegate () {
                                        Pagination.Put(o);
                                    }));
                        })
                    );
            }
        }
        public Command DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (
                        deleteCommand = new Command(obj =>
                        {
                            if (obj == null || obj.GetType() != typeof(T))
                                return;
                            NHibernateHelper.GetInstance()
                                .DeleteAsync(
                                    (T)obj,
                                    (o) => App.Current.Dispatcher.BeginInvoke((Action)delegate () {
                                        Pagination.Remove(o);
                                    }));

                        })
                    );
            }
        }
        public Command UpdateCommand
        {
            get
            {
                return updateCommand ??
                    (
                        updateCommand = new Command(obj =>
                        {
                            NHibernateHelper.GetInstance().Update((T)obj);
                        })
                    );
            }
        }

        public T NewItem { get; set; }
        public PaginationHelper<T> Pagination { get; set; }
        public BaseScrollUCViewModel(T initialNewItem)
        {
            var items = new ObservableCollection<T>(NHibernateHelper.GetInstance().Get<T>(0, Constants.PAGE_SIZE));
            Pagination = new ScrollPaginationHelper<T>(ref items);
            NewItem = initialNewItem;
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
