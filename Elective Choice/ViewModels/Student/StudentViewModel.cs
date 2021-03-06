using System;
using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views;
using Elective_Choice.Views.Student;

namespace Elective_Choice.ViewModels.Student;

public class StudentViewModel : ViewModel
{
    #region Fields

    private Page? _frameContent;
    private string _fullName = string.Empty;
    private string Email { get; set; } = string.Empty;
    private Page? CalendarPage { get; set; }

    public Page? FrameContent
    {
        get => _frameContent;
        private set => Set(ref _frameContent, value);
    }

    public string FullName
    {
        get => _fullName;
        set => Set(ref _fullName, value);
    }

    #endregion

    #region Constructor

    public StudentViewModel()
    {
    }

    public StudentViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += Login_OnCompleted;
        source.DayLoading += Day_OnLoading;
        source.DayClosing += Day_OnClosing;

        FrameContent = new ElectiveCalendar(source, Email);

        LogoutCommand = new Command(
            LogoutCommand_OnExecute,
            LogoutCommand_CanExecute);
        CalendarCommand = new Command(
            CalendarCommand_OnExecute,
            CalendarCommand_CanExecute);
        PrioritizeCommand = new Command(
            PrioritizeCommand_OnExecute,
            PrioritizeCommand_CanExecute);
        ResultCommand = new Command(
            ResultCommand_OnExecute,
            ResultCommand_CanExecute);
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && Source is not null)
            {
                Source.LoginCompleted -= Login_OnCompleted;
                Source.DayLoading -= Day_OnLoading;
                Source.DayClosing -= Day_OnClosing;
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        FullName = DatabaseAccess.GetPersonNameBy(e.Email.Substring(4, 10));
    }

    private void Day_OnLoading(object? sender, DayEventArgs e)
    {
        CalendarPage = FrameContent;
        FrameContent = new DayElectives(Source!);
        Source?.RaiseDayLoaded(this, e);
    }

    private void Day_OnClosing(object? sender, DayEventArgs e)
    {
        FrameContent = CalendarPage;
    }

    #endregion

    #region Commands

    #region LogoutCommand

    public Command? LogoutCommand { get; }

    private bool LogoutCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void LogoutCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case ElectiveCalendar:
                Source?.RaiseCalendarClosing(this, EventArgs.Empty);
                break;
            case Priorities:
                Source?.RaisePrioritiesClosing(this, EventArgs.Empty);
                break;
        }

        Source?.RaiseLogoutSucceed(this, new LoginEventArgs(Email, false));
        Dispose();
    }

    #endregion

    #region CalendarCommand

    public Command? CalendarCommand { get; }

    private bool CalendarCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void CalendarCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case Priorities:
                Source?.RaisePrioritiesClosing(this, EventArgs.Empty);
                break;
            case Results:
                Source?.RaiseResultsClosing(this, EventArgs.Empty);
                break;
        }

        FrameContent = new ElectiveCalendar(Source!, Email);
    }

    #endregion

    #region PrioritizeCommand

    public Command? PrioritizeCommand { get; }

    private bool PrioritizeCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void PrioritizeCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case ElectiveCalendar:
                Source?.RaiseCalendarClosing(this, EventArgs.Empty);
                break;
            case Results:
                Source?.RaiseResultsClosing(this, EventArgs.Empty);
                break;
        }

        FrameContent = new Priorities(Source!, Email);
    }

    #endregion

    #region ResultCommand

    public Command? ResultCommand { get; }

    private bool ResultCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void ResultCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case ElectiveCalendar:
                Source?.RaiseCalendarClosing(this, EventArgs.Empty);
                break;
            case Priorities:
                Source?.RaisePrioritiesClosing(this, EventArgs.Empty);
                break;
        }
        
        FrameContent = new Results(Source!, Email);
    }

    #endregion

    #endregion
}