using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class StudentViewModel : ViewModel
{
    private string Email { get; set; } = string.Empty;

    public StudentViewModel()
    {
        
    }

    public StudentViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += email => Email = email;
    }
}