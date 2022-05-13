using System.Windows;
using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class AdminPage
{
    public AdminPage(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new AdminViewModel(store);
    }

    private void AdminPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.MainWindow;
        if (window is null) return;
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.Top = (SystemParameters.WorkArea.Height - window.Height) / 2;
        window.Left = (SystemParameters.WorkArea.Width - window.Width) / 2;
    }
}