using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Student;

namespace Elective_Choice.Views;

public partial class DayElectives
{
    public DayElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new DayElectivesViewModel(source);
    }
}