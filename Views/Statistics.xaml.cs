using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class Statistics
{
    public Statistics(ViewModelStore? store)
    {
        InitializeComponent();
        DataContext = new StatisticsViewModel(store);
    }
}