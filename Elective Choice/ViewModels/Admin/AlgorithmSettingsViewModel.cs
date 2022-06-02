using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels.Admin;

public class AlgorithmSettingsViewModel : ViewModel
{
    #region Fields

    #endregion
    
    #region Constructor

    public AlgorithmSettingsViewModel()
    {
        
    }
    
    public AlgorithmSettingsViewModel(EventSource source) : base(source)
    {
    }

    #endregion
}