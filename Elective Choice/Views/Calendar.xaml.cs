using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class Calendar
{
    public Calendar(EventSource source)
    {
        InitializeComponent();
        DataContext = new CalendarViewModel(source);
    }
}