using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class CurrentElectives
{
    public CurrentElectives(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new CurrentElectivesViewModel(store);
    }
}