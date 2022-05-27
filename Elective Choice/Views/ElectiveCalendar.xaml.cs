using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class ElectiveCalendar
{
    public ElectiveCalendar(EventSource source)
    {
        InitializeComponent();
        DataContext = new ElectiveCalendarViewModel(source);
    }
}