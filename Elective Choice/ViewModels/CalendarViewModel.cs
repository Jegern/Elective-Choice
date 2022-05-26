using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class CalendarViewModel : ViewModel
{
    public CalendarViewModel(EventSource source) : base(source)
    {
    }
}