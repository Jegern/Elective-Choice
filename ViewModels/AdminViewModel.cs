using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class AdminViewModel : ViewModel
{
    private string Username { get; set; } = string.Empty;
    
    public AdminViewModel()
    {
        
    }

    public AdminViewModel(ViewModelStore store) : base(store)
    {
        store.SuccessfulLogin += (username, _) => Username = username;
    }
}