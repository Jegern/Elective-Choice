using Elective_Choice.Infrastructure.EventArgs;
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
        source.LoginCompleted += Login_OnCompleted;
    }

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
    }
}