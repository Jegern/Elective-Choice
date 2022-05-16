using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class ElectiveEditing
{
    public ElectiveEditing(ViewModelStore? store)
    {
        InitializeComponent();
        DataContext = new ElectiveEditingViewModel(store);
    }
}