using System.Windows;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class Student
{
    public Student(EventSource store)
    {
        InitializeComponent();
        DataContext = new StudentViewModel(store);
    }

    private void StudentPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.MainWindow;
        if (window is null) return;
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.Top = (SystemParameters.WorkArea.Height - window.Height) / 2;
        window.Left = (SystemParameters.WorkArea.Width - window.Width) / 2;
    }
}