using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Student;


namespace Elective_Choice.Views.Student;

public partial class Results : Page
{

    public Results(EventSource source, string email)
    {
        InitializeComponent();
        DataContext = new ResultsViewModel(source, email);
        // TreeViewItem parentItem = new TreeViewItem();  
        // parentItem.Header = "Semester 4, Feb-Jun 2021";
        // parentItem.FontSize = 20;
        // SemesterResults.Items.Add(parentItem);

    }
    
}