using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Student;

namespace Elective_Choice.Views;

public partial class Priorities
{
    public Priorities(EventSource source, string email)
    {
        InitializeComponent();
        DataContext = new PrioritiesViewModel(source, email);
    }
}