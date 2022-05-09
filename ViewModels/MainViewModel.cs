using System.Windows.Controls;
using Elective_Choice.Views;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class MainViewModel : ViewModel
{
    #region Fields

    private Page? _frameContent;

    public Page? FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    #endregion

    public MainViewModel()
    {
    }

    public MainViewModel(ViewModelStore store) : base(store)
    {
        store.SuccessfulLogin += (_, rights) => FrameContent = rights ? new AdminPage() : new StudentPage();

        FrameContent = new LoginPage(store);
    }
}