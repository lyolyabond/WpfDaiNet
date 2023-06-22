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
using System.Windows.Shapes;
using WpfDaiNet.Model;
using WpfDaiNet.ViewModel;

namespace WpfDaiNet
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window
    {
        private readonly Window _mainWindow;
        private readonly ResultViewModel _resultViewModel;

        public Result(Window mainWindow)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            _mainWindow = mainWindow;
            _resultViewModel = new ResultViewModel();
            DataContext = _resultViewModel;
            IsVisibleChanged += WindowIsVisibleChanged;
        }
        private void WindowIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                _resultViewModel.Start();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            _mainWindow.Show();
        }
    }
}
