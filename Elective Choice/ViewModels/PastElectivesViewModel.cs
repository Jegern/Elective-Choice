using System.Collections.Generic;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class PastElectivesViewModel : ViewModel
{
    #region Fields

    private string _name = string.Empty;
    private List<Elective>? _electives;


    private int Year { get; set; }
    private bool Spring { get; set; }

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public List<Elective>? Electives
    {
        get => _electives;
        set => Set(ref _electives, value);
    }

    #endregion

    public PastElectivesViewModel()
    {
    }

    public PastElectivesViewModel(EventSource source) : base(source)
    {
        source.SemesterLoaded += Semester_OnLoaded;

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
        BackToListCommand = new Command(
            BackToListCommand_OnExecuted,
            BackToListCommand_CanExecute);
    }

    #region Event Subscription

    private void Semester_OnLoaded(object? sender, SemesterEventArgs e)
    {
        // TODO: Исправить множественную подписку на событие
        Name = $"{(e.Spring ? "Осень" : "Весна")}, {e.Year}-й";
        Year = e.Year;
        Spring = e.Spring;
        Electives = DatabaseAccess.GetSemesterElectives(e.Year, e.Spring);
    }

    #endregion

    #region Commands

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is Elective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsLoading(this,
            new StatisticsEventArgs(((Elective) parameter!).Name, Year, Spring));
    }

    #endregion

    #region BackToListCommand

    public Command? BackToListCommand { get; }

    private bool BackToListCommand_CanExecute(object? parameter) => Name != string.Empty;

    private void BackToListCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseSemesterClosing(this, new SemesterEventArgs(Year, Spring));
    }

    #endregion

    #endregion
}