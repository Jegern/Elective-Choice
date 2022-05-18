using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class ElectiveStatistics
{
    public ElectiveStatistics(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new ElectiveStatisticsViewModel(store);
    }
}