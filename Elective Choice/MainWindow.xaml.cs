using System.Windows.Controls;
using System.Windows.Navigation;

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