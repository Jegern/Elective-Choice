using System.Windows.Controls;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Elective_Choice.Views;
using Laboratory_work_1.Commands.Base;

namespace Elective_Choice.ViewModels;

public class AdminViewModel : ViewModel
{
    #region Fields

    private string Email { get; set; } = string.Empty;
    private Page? _frameContent;
    private string _fullname = string.Empty;
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

    public string Fullname
    {
        get => _fullname;
        set => Set(ref _fullname, value);
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

    public AdminViewModel(ViewModelStore store) : base(store)
    {
        store.LoginComplete += LoginComplete_OnChanged;
        
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

    private void LoginComplete_OnChanged(string email)
    {
        Email = email;
        // TODO: Реализовать обращение к БД и получение некоторых полей
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
    }

    #endregion

    #region StatisticsCommand

    public Command? StatisticsCommand { get; }

    private bool StatisticsCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void StatisticsCommand_OnExecute(object? parameter)
    {
        StatisticsEnabled = false;

        FrameContent = new ElectiveEditing();
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