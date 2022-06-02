using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Student;

namespace Elective_Choice.Views.Student;

public partial class DayElectives
{
    public DayElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new DayElectivesViewModel(source);
    }
}