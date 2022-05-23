using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class ProblemElectives
{
    public ProblemElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new ProblemElectivesViewModel(source);
    }
}