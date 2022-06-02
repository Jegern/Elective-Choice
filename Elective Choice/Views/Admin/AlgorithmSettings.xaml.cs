using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views.Admin;

public partial class AlgorithmSettings
{
    public AlgorithmSettings(EventSource source)
    {
        InitializeComponent();
        DataContext = new AlgorithmSettingsViewModel(source);
    }
}