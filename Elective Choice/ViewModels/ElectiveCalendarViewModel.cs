using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class ElectiveCalendarViewModel : ViewModel
{
    public ElectiveCalendarViewModel()
    {
    }
    
    public ElectiveCalendarViewModel(EventSource source) : base(source)
    {
    }
}