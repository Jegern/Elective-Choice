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
    private Page _frameContent = new LoginPage(Store);
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
        if (CheckUserData())
            Store.SuccessfulLogin += SuccessfulLogin_OnChanged;
    }

    private bool CheckUserData()
    {
        // TODO: Реализовать обращение к БД и проверку электронной почты и пароля
        return true;
    }

    private void SuccessfulLogin_OnChanged(string email, bool rights)
    {
        FrameContent = rights ? new AdminPage(Store) : new StudentPage(Store);
        ResizeMode = ResizeMode.CanResize;
        Store.TriggerLoginCompleteEvent(email);
    }
}