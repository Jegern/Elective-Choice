using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class SemesterElectives
{
    public SemesterElectives(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new SemesterElectivesViewModel(store);
    }
}