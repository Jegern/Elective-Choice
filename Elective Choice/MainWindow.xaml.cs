using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Elective_Choice.ViewModels;
using Elective_Choice.Views.Styles.Priorities;

namespace Elective_Choice
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += ((MainViewModel) DataContext).MainWindow_OnClosing;
        }

        private void Frame_OnNavigated(object sender, NavigationEventArgs e)
        {
            (sender as Frame)?.NavigationService.RemoveBackEntry();
        }
    }
}