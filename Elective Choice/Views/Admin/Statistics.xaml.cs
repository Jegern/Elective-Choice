using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views.Admin;

public partial class Statistics
{
    public Statistics(EventSource store)
    {
        InitializeComponent();
        DataContext = new StatisticsViewModel(store);
    }
}