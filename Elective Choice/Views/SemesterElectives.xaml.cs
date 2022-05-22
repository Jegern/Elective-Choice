using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class SemesterElectives
{
    public SemesterElectives(EventSource store)
    {
        InitializeComponent();
        DataContext = new SemesterElectivesViewModel(store);
    }
}