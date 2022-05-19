using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class Login
{
    public Login(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(store);
    }
}