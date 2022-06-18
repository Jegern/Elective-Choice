using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views.Styles.Results;


namespace Elective_Choice.ViewModels.Student;

public class ResultsViewModel: ViewModel
{
    private string Email { get; set; } = string.Empty;
    private List<List<Elective>> Electives { get; set; } = new List<List<Elective>>();
    


    private ObservableCollection<TreeViewItem> _treeItems = new ();


    public ObservableCollection<TreeViewItem> TreeItems
    {
        get => _treeItems;
        set => Set(ref _treeItems, value);
    }

    public ResultsViewModel()
    {
    }


    public ResultsViewModel(EventSource source, string email) : base(source)
    {
        Email = email;
        Electives = DatabaseAccess.GetStudentResultElectives(Email.Substring(4, 10));
        for (int i = 0; i < Electives.Count(); i++)
        {
            bool spring = Electives[i][0].Spring;
            var semester = Electives[i][0].Year.ToString();
            if (spring)
            {
                semester += " Feb-June   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";
            }
            else
            {
                semester += " September-December   ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";
            }

            var semesterName = new TreeViewItem
            {
                Header = semester,
                Margin = new Thickness(100,0,0,0),
                FontSize = 18
            };
            TreeItems.Add(semesterName);
            var electiveOne = new TreeItem
            {
                FirstElectiveName =
                {
                    Text = Electives[i][0].Name
                },
                SecondElectiveName =
                {
                    Text = Electives[i][1].Name
                },
                FirstElectivePrioirity =
                {
                    Text = Electives[i][0].Priority.ToString() + " приоритет"
                },
                SecondElectivePrioirity =
                {
                    Text = Electives[i][1].Priority.ToString() + " приоритет"
                }
            };
            semesterName.Items.Add(electiveOne);
        }
            

    }
}