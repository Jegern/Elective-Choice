using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Elective_Choice.Views.Styles.Priorities;

namespace Elective_Choice
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Frame_OnNavigated(object sender, NavigationEventArgs e)
        {
            (sender as Frame)?.NavigationService.RemoveBackEntry();
        }
    }
}