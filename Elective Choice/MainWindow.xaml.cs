using System.Windows.Controls;
using System.Windows.Navigation;
using Elective_Choice.ViewModels;

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