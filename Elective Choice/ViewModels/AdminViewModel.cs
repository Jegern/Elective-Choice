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

    private Page? _frameContent;
    private string _fullName = string.Empty;
    private string Email { get; set; } = string.Empty;
    private Page? ElectivePage { get; set; }
    private Page? SemesterPage { get; set; }

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

        EditCommand = new Command(
            EditCommand_OnExecute,
            EditCommand_CanExecute);
        SemestersCommand = new Command(
            SemestersCommand_OnExecute,
            SemestersCommand_CanExecute);
        AlgorithmCommand = new Command(
            AlgorithmCommand_OnExecute,
            AlgorithmCommand_CanExecute);
    }

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        FullName = DatabaseAccess.GetPersonNameBy(e.Email.Substring(4, 10));
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

    #region EditCommand

    public Command? EditCommand { get; }

    private bool EditCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void EditCommand_OnExecute(object? parameter)
    {
        FrameContent = new ProblemElectives(Source!);
    }

    #endregion

    #region SemestersCommand

    public Command? SemestersCommand { get; }

    private bool SemestersCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void SemestersCommand_OnExecute(object? parameter)
    {
        FrameContent = new Semesters(Source!);
    }

    #endregion

    #region AlgorithmCommand

    public Command? AlgorithmCommand { get; }

    private bool AlgorithmCommand_CanExecute(object? parameter) => Email != string.Empty;

    private void AlgorithmCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #endregion
}