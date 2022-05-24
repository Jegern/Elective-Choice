using System;
using System.Globalization;
using System.Windows.Controls;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views;

namespace Elective_Choice.ViewModels;

public class AdminViewModel : ViewModel
{
    #region Fields

    private string Email { get; set; } = string.Empty;
    private Page? ElectivePage { get; set; }
    private Page? SemesterPage { get; set; }
    private Page? _frameContent;
    private string _name = string.Empty;
    private bool _editEnabled;
    private bool _statisticsEnabled = true;
    private bool _algorithmEnabled = true;
    private string _currentDate = string.Empty;
    private string _countdown = string.Empty;

    public Page? FrameContent
    {
        get => _frameContent;
        private set => Set(ref _frameContent, value);
    }

    public string Name
    {
        get => _name;
        private set => Set(ref _name, value);
    }

    public bool EditEnabled
    {
        get => _editEnabled;
        set
        {
            if (!Set(ref _editEnabled, value)) return;
            if (!EditEnabled)
                StatisticsEnabled = AlgorithmEnabled = true;
        }
    }

    public bool StatisticsEnabled
    {
        get => _statisticsEnabled;
        set
        {
            if (!Set(ref _statisticsEnabled, value)) return;
            if (!StatisticsEnabled)
                EditEnabled = AlgorithmEnabled = true;
        }
    }

    public bool AlgorithmEnabled
    {
        get => _algorithmEnabled;
        set
        {
            if (!Set(ref _algorithmEnabled, value)) return;
            if (!AlgorithmEnabled)
                EditEnabled = StatisticsEnabled = true;
        }
    }

    public string CurrentDate
    {
        get => _currentDate;
        private set => Set(ref _currentDate, value);
    }

    public string Countdown
    {
        get => _countdown;
        set => Set(ref _countdown, value);
    }

    #endregion

    public AdminViewModel()
    {
    }

    public AdminViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += Login_OnCompleted;
        source.StatisticsLoading += Statistics_OnLoading;
        source.StatisticsClosing += Statistics_OnClosing;
        source.SemesterLoading += Semester_OnLoading;
        source.SemesterClosing += Semester_OnClosing;

        FrameContent = new ProblemElectives(Source!);

        MenuCommand = new Command(
            MenuCommand_OnExecute,
            MenuCommand_CanExecute);
        LogoutCommand = new Command(
            LogoutCommand_OnExecute,
            LogoutCommand_CanExecute);
        EditCommand = new Command(
            EditCommand_OnExecute,
            EditCommand_CanExecute);
        StatisticsCommand = new Command(
            StatisticsCommand_OnExecute,
            StatisticsCommand_CanExecute);
        AlgorithmCommand = new Command(
            AlgorithmCommand_OnExecute,
            AlgorithmCommand_CanExecute);
    }

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        Name = DatabaseAccess.GetPersonNameBy(e.Email.Substring(4, 10));
        CurrentDate = DateTime.Today.ToString("D", CultureInfo.GetCultureInfo("ru-RU"));
    }

    private void Statistics_OnLoading(object? sender, StatisticsEventArgs e)
    {
        ElectivePage = FrameContent;
        FrameContent = new Statistics(Source!);
        Source?.RaiseStatisticsLoaded(sender, e);
    }

    private void Statistics_OnClosing(object? sender, StatisticsEventArgs e)
    {
        FrameContent = ElectivePage;
    }

    private void Semester_OnLoading(object? sender, SemesterEventArgs e)
    {
        SemesterPage = FrameContent;
        FrameContent = new PastElectives(Source!);
        Source?.RaiseSemesterLoaded(this, e);
    }

    private void Semester_OnClosing(object? sender, SemesterEventArgs e)
    {
        FrameContent = SemesterPage;
    }

    #endregion

    #region Commands

    #region MenuCommand

    public Command? MenuCommand { get; }

    private bool MenuCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void MenuCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #region LogoutCommand

    public Command? LogoutCommand { get; }

    private bool LogoutCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void LogoutCommand_OnExecute(object? parameter)
    {
        Source?.RaiseLogoutSucceed(this, new LoginEventArgs(Email, true));
    }

    #endregion

    #region EditCommand

    public Command? EditCommand { get; }

    private bool EditCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void EditCommand_OnExecute(object? parameter)
    {
        EditEnabled = false;
        FrameContent = new ProblemElectives(Source!);
    }

    #endregion

    #region StatisticsCommand

    public Command? StatisticsCommand { get; }

    private bool StatisticsCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void StatisticsCommand_OnExecute(object? parameter)
    {
        StatisticsEnabled = false;
        FrameContent = new Semesters(Source!);
    }

    #endregion

    #region AlgorithmCommand

    public Command? AlgorithmCommand { get; }

    private bool AlgorithmCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void AlgorithmCommand_OnExecute(object? parameter)
    {
        AlgorithmEnabled = false;
    }

    #endregion

    #endregion
}