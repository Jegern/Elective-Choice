using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Views;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class MainViewModel : ViewModel
{
    #region Fields

    private new static EventSource Source { get; } = new();
    private Page _frameContent = new Login(Source);
    private ResizeMode _resizeMode = ResizeMode.CanMinimize;

    public Page FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    public ResizeMode ResizeMode
    {
        get => _resizeMode;
        private set => Set(ref _resizeMode, value);
    }

    #endregion

    public MainViewModel()
    {
        Source.LoginSucceed += Login_OnSucceed;
        Source.LogoutSucceed +=  SourceOnLogoutSucceed;
    }

    #region Event Subcription

    private void Login_OnSucceed(object? sender, LoginEventArgs e)
    {
        FrameContent = e.Rights ? new Admin(Source) : new Student(Source);
        ResizeMode = ResizeMode.CanResize;
        Source.RaiseLoginCompleted(sender, e);
    }
    
    private void SourceOnLogoutSucceed(object? sender, LoginEventArgs e)
    {
        FrameContent = new Login(Source);
        ResizeMode = ResizeMode.CanMinimize;
    }

    #endregion
}