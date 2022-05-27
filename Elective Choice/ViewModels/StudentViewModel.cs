using System.Windows.Controls;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views;

namespace Elective_Choice.ViewModels;

public class StudentViewModel : ViewModel
{
    #region Fields

    private Page? _frameContent;
    private string _fullName = string.Empty;
    private string Email { get; set; } = string.Empty;
    private Page? ElectivePage { get; set; }

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

    public StudentViewModel()
    {
    }

    public StudentViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += Login_OnCompleted;

        FrameContent = new ElectiveCalendar(source);
        
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

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        FullName = DatabaseAccess.GetPersonNameBy(e.Email.Substring(4, 10));
    }

    #endregion
    
    #region Commands

    #region CalendarCommand

    public Command? CalendarCommand { get; }

    private bool CalendarCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void CalendarCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #region PrioritizeCommand

    public Command? PrioritizeCommand { get; }

    private bool PrioritizeCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void PrioritizeCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #region ResultCommand

    public Command? ResultCommand { get; }

    private bool ResultCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void ResultCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #endregion
}