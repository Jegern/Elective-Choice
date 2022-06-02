using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views.Admin;

public partial class PastElectives
{
    public PastElectives(EventSource store)
    {
        InitializeComponent();
        DataContext = new PastElectivesViewModel(store);
    }
}