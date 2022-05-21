using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Views;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class MainViewModel : ViewModel
{
    #region Fields

    private new static ViewModelStore Store { get; } = new();
    private Page _frameContent = new Login(Store);
    private ResizeMode _resizeMode = ResizeMode.CanMinimize;

    public Page FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    public ResizeMode ResizeMode
    {
        get => _resizeMode;
        set => Set(ref _resizeMode, value);
    }

    #endregion

    public MainViewModel()
    {
        Store.LoginSucceed += Login_OnSucceed;
    }

    private void Login_OnSucceed(string email, bool rights)
    {
        FrameContent = rights ? new Admin(Store) : new Student(Store);
        ResizeMode = ResizeMode.CanResize;
        Store.TriggerLoginCompleted(email);
    }
}