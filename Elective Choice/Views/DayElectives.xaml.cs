using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class DayElectives
{
    public DayElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new DayElectivesViewModel(source);
    }
}