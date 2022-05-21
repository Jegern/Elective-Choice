using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class PastSemesters
{
    public PastSemesters(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new PastSemestersViewModel(store);
    }
}