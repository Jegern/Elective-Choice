using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class PastElectives
{
    public PastElectives(EventSource store)
    {
        InitializeComponent();
        DataContext = new PastElectivesViewModel(store);
    }
}