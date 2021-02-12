using HappyWaterCarrierTestApp.ViewModel.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Command loadEmployeeUCCommand;
        private Command loadUnitUCCommand;
        private Command loadItemUCCommand;
        private Command loadOrderUCCommand;
        public Command LoadEmployeeUCCommand
        {
            get
            {
                return loadEmployeeUCCommand ??
                    (
                        loadEmployeeUCCommand = new Command(obj =>
                        {
                            CodeBehind.LoadView(ViewType.Employee);
                        })
                    );
            }
        }
        public Command LoadUnitUCCommand
        {
            get
            {
                return loadUnitUCCommand ??
                    (
                        loadUnitUCCommand = new Command(obj =>
                        {
                            CodeBehind.LoadView(ViewType.Unit);
                        })
                    );
            }
        }
        public Command LoadItemUCCommand
        {
            get
            {
                return loadItemUCCommand ??
                    (
                        loadItemUCCommand = new Command(obj =>
                        {
                            CodeBehind.LoadView(ViewType.Item);
                        })
                    );
            }
        }
        public Command LoadOrderUCCommand
        {
            get
            {
                return loadOrderUCCommand ??
                    (
                        loadOrderUCCommand = new Command(obj =>
                        {
                            CodeBehind.LoadView(ViewType.Order);
                        })
                    );
            }
        }
        public MainWindowViewModel()
        {
            
        }
        public IMainWindowsCodeBehind CodeBehind { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
