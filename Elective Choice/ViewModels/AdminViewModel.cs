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
    private Page? CurrentElectives { get; set; }
    private Page? _frameContent;
    private string _name = string.Empty;
    private bool _editEnabled;
    private bool _statisticsEnabled = true;
    private bool _algorithmEnabled = true;
    private string _currentDate = string.Empty;
    private string _beforeDistribution = string.Empty;

    public Page? FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
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
        set => Set(ref _currentDate, value);
    }

    public string BeforeDistribution
    {
        get => _beforeDistribution;
        set => Set(ref _beforeDistribution, value);
    }

    #endregion

    public AdminViewModel()
    {
    }

    public AdminViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += Login_OnCompleted;
        source.StatisticsLoading += StatisticsOnLoading;
        source.StatisticsClosed += StatisticsOnClosed;
        source.SemesterLoading += Semester_OnLoading;

        // FrameContent = new SemesterElectives(Store!);

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

    private void Login_OnCompleted(string email)
    {
        Email = email;
        Name = DatabaseAccess.GetPersonNameBy(email.Substring(4, 10));
    }

    private void StatisticsOnLoading(string name, int? year, bool? spring)
    {
        CurrentElectives = FrameContent;
        FrameContent = new Statistics(Source!);
        Source?.RaiseStatisticsLoaded(this, new StatisticsEventArgs(name, year, spring));
    }

    private void StatisticsOnClosed()
    {
        FrameContent = CurrentElectives;
    }

    private void Semester_OnLoading(int year, bool spring)
    {
        FrameContent = new SemesterElectives(Source!);
        Source?.RaiseSemesterLoaded(year, spring);
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
    }

    #endregion

    #region EditCommand

    public Command? EditCommand { get; }

    private bool EditCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void EditCommand_OnExecute(object? parameter)
    {
        EditEnabled = false;
        // FrameContent = new SemesterElectives(Store!);
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