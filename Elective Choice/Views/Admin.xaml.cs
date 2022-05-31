using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views;

public partial class Admin
{
    public Admin(EventSource store)
    {
        InitializeComponent();
        DataContext = new AdminViewModel(store);
    }

    private void AdminPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.MainWindow;
        if (window is null) return;
        window.MinWidth = 960;
        window.MinHeight = 720;
        window.Width = 1280;
        window.Height = 720;
        window.Top = (SystemParameters.WorkArea.Height - window.Height) / 2;
        window.Left = (SystemParameters.WorkArea.Width - window.Width) / 2;
    }

    private void Frame_OnNavigated(object sender, NavigationEventArgs e)
    {
        (sender as Frame)?.NavigationService.RemoveBackEntry();
    }
}