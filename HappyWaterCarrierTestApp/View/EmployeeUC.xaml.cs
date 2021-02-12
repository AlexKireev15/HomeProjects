using HappyWaterCarrierTestApp.Utils;
using HappyWaterCarrierTestApp.ViewModel;
using System.Windows.Controls;

namespace HappyWaterCarrierTestApp.View
{
    /// <summary>
    /// Логика взаимодействия для EmployeeUC.xaml
    /// </summary>
    public partial class EmployeeUC : UserControl
    {
        public EmployeeUC()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(e.VerticalChange != 0)
            {
                if (e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - Constants.DOUBLE_CHECK)
                {
                    ((EmployeeUCViewModel)DataContext).LoadNextElement();
                    ((ScrollViewer)sender).ScrollToVerticalOffset(e.VerticalOffset - 1);
                }
                if (e.VerticalOffset <= Constants.DOUBLE_CHECK)
                {
                    ((EmployeeUCViewModel)DataContext).LoadPrevElement();
                    ((ScrollViewer)sender).ScrollToVerticalOffset(e.VerticalOffset + 1);
                }
            }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ScrollViewer.VerticalOffset <= Constants.DOUBLE_CHECK)
            {
                ScrollViewer.ScrollToVerticalOffset(Constants.DOUBLE_CHECK);
            }
        }
    }
}
