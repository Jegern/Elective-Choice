using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.Views;

public partial class LoginPage
{
    public LoginPage(ViewModelStore store)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(store);
    }
}