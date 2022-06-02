using System;
using System.ComponentModel;
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

    public Page FrameContent
    {
        get => _frameContent;
        private set => Set(ref _frameContent, value);
    }

    #endregion

    public MainViewModel()
    {
        Source.LoginSucceed += Login_OnSucceed;
        Source.LogoutSucceed += Logout_OnSucceed;
    }

    #region Event Subcription

    private void Login_OnSucceed(object? sender, LoginEventArgs e)
    {
        FrameContent = e.Rights ? new Views.Admin.Admin(Source) : new Views.Student.Student(Source);
        Source.RaiseLoginCompleted(sender, e);
    }

    private void Logout_OnSucceed(object? sender, LoginEventArgs e)
    {
        FrameContent = new Login(Source);
    }

    public void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        if (FrameContent is not Views.Student.Student) return;
        Source.RaiseCalendarClosing(this, EventArgs.Empty);
        Source.RaisePrioritiesClosing(this, EventArgs.Empty);
    }

    #endregion
}