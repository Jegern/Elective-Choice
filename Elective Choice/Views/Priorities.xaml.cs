using System.Linq;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Student;
using Elective_Choice.Views.Styles.Priorities;

namespace Elective_Choice.Views;

public partial class Priorities
{
    public Priorities(EventSource source, string email)
    {
        InitializeComponent();
        var dataContext = new PrioritiesViewModel(source, email);
        DataContext = dataContext;
        var electives = DatabaseAccess.GetStudentElectives(dataContext.Email.Substring(4, 10));
        for (var i = 0; i < 5; i++)
        {
            var card = (LowerCard)LowerGrid.Children[i];
            if (electives.Count > i)
                card.ElectiveName = card.Text = electives[i].Name;
            else
                card.IsEnabled = false;
        }

        foreach (var card in UpperGrid.Children.Cast<UpperCard>())
            card.IsEnabled = false;
    }
}