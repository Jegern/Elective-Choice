using System;
using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views;
using Elective_Choice.Views.Admin;

namespace Elective_Choice.ViewModels.Admin;

public class AdminViewModel : ViewModel
{
    #region Fields

    private Page? _frameContent;
    private string _fullName = string.Empty;
    private string Email { get; set; } = string.Empty;
    private Page? ElectivesPage { get; set; }
    private Page? SemestersPage { get; set; }

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

        FrameContent = new ProblemElectives(source);

        LogoutCommand = new Command(
            LogoutCommand_OnExecute,
            LogoutCommand_CanExecute);
        ProblemElectivesCommand = new Command(
            ProblemElectivesCommand_OnExecute,
            ProblemElectivesCommand_CanExecute);
        SemestersCommand = new Command(
            SemestersCommand_OnExecute,
            SemestersCommand_CanExecute);
        AlgorithmSettingsCommand = new Command(
            AlgorithmSettingsCommand_OnExecute,
            AlgorithmSettingsCommand_CanExecute);
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && Source is not null)
            {
                Source.LoginCompleted -= Login_OnCompleted;
                Source.StatisticsLoading -= Statistics_OnLoading;
                Source.StatisticsClosing -= Statistics_OnClosing;
                Source.SemesterLoading -= Semester_OnLoading;
                Source.SemesterClosing -= Semester_OnClosing;
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

    private void Statistics_OnLoading(object? sender, StatisticsEventArgs e)
    {
        ElectivesPage = FrameContent;
        FrameContent = new Statistics(Source!);
        Source?.RaiseStatisticsLoaded(sender, e);
    }

    private void Statistics_OnClosing(object? sender, StatisticsEventArgs e)
    {
        FrameContent = ElectivesPage;
    }

    private void Semester_OnLoading(object? sender, SemesterEventArgs e)
    {
        SemestersPage = FrameContent;
        FrameContent = new PastElectives(Source!);
        Source?.RaiseSemesterLoaded(this, e);
    }

    private void Semester_OnClosing(object? sender, SemesterEventArgs e)
    {
        FrameContent = SemestersPage;
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
            case AlgorithmSettings:
                Source?.RaiseAlgorithmSettingClosing(this, EventArgs.Empty);
                break;
        }
        
        Source?.RaiseLogoutSucceed(this, new LoginEventArgs(Email, false));
        Dispose();
    }

    #endregion

    #region ProblemElectivesCommand

    public Command? ProblemElectivesCommand { get; }

    private bool ProblemElectivesCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void ProblemElectivesCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case AlgorithmSettings:
                Source?.RaiseAlgorithmSettingClosing(this, EventArgs.Empty);
                break;
        }

        FrameContent = new ProblemElectives(Source!);
    }

    #endregion

    #region SemestersCommand

    public Command? SemestersCommand { get; }

    private bool SemestersCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void SemestersCommand_OnExecute(object? parameter)
    {
        switch (FrameContent)
        {
            case AlgorithmSettings:
                Source?.RaiseAlgorithmSettingClosing(this, EventArgs.Empty);
                break;
        }

        FrameContent = new Semesters(Source!);
    }

    #endregion

    #region AlgorithmSettingsCommand

    public Command? AlgorithmSettingsCommand { get; }

    private bool AlgorithmSettingsCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void AlgorithmSettingsCommand_OnExecute(object? parameter)
    {
        FrameContent = new AlgorithmSettings(Source!);
    }

    #endregion

    #endregion
}