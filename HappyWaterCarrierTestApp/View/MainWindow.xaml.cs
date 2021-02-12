using HappyWaterCarrierTestApp.Model;
using HappyWaterCarrierTestApp.View;
using HappyWaterCarrierTestApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HappyWaterCarrierTestApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowsCodeBehind
    {
        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public void LoadView(ViewType viewType)
        {
            UserControl view = null;
            switch (viewType)
            {
                case ViewType.Employee:
                    view = new EmployeeUC();
                    view.DataContext = new BaseScrollUCViewModel<Employee>(new Employee { Birthday = DateTime.Now });
                    break;
                case ViewType.Unit:
                    view = new UnitUC();
                    break;
                case ViewType.Item:
                    view = new ItemUC();
                    break;
                case ViewType.Order:
                    view = new OrderUC();
                    break;
            }
            OutputView.Content = view;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = viewModel;
            vm.CodeBehind = this;
            DataContext = vm;
            LoadView(ViewType.Employee);
        }
    }

    public enum ViewType
    {
        Employee,
        Unit,
        Item,
        Order
    }
    public interface IMainWindowsCodeBehind
    {
        void LoadView(ViewType viewType);
    }
}
