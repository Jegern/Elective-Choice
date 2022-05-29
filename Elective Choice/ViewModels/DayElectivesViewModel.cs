using System.Collections.Generic;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class DayElectivesViewModel : ViewModel
{
    #region Fields

    private string _headerText = string.Empty;
    private List<Elective>? _electives;

    private string Day { get; set; } = string.Empty;

    public string HeaderText
    {
        get => _headerText;
        set => Set(ref _headerText, value);
    }

    public List<Elective>? Electives
    {
        get => _electives;
        set => Set(ref _electives, value);
    }

    #endregion

    public DayElectivesViewModel()
    {
    }

    public DayElectivesViewModel(EventSource source) : base(source)
    {
        source.DayLoaded += DayOnLoaded;

        ChooseElectiveCommand = new Command(
            ChooseElectiveCommand_OnExecuted,
            ChooseElectiveCommand_CanExecute);
        GoBackCommand = new Command(
            GoBackCommand_OnExecuted,
            GoBackCommand_CanExecute);
    }

    #region Event Subscription

    private void DayOnLoaded(object? sender, DayEventArgs e)
    {
        Day = e.Day;
        HeaderText = $"Элективы на {e.Day switch {"Среда" => "среду", "Пятница" => "пятницу", _ => e.Day.ToLower()}}";
        Electives = DatabaseAccess.GetElectivesForDay(e.Day switch
        {
            "Вторник" => 2,
            "Среда" => 3,
            "Четверг" => 4,
            "Пятница" => 5,
            _ => 0
        });
    }

    #endregion

    #region Commands

    #region ChooseElectiveCommand

    public Command? ChooseElectiveCommand { get; }

    private bool ChooseElectiveCommand_CanExecute(object? parameter) => 
        parameter is Elective &&
        Day != string.Empty;

    private void ChooseElectiveCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseDayElectiveChosen(this, new DayEventArgs(Day, (Elective) parameter!));
        Source?.RaiseDayClosing(this, new DayEventArgs(Day, (Elective) parameter!));
    }

    #endregion

    #region GoBackCommand

    public Command? GoBackCommand { get; }

    private bool GoBackCommand_CanExecute(object? parameter) => Day != string.Empty;

    private void GoBackCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseDayClosing(this, new DayEventArgs(Day));
    }

    #endregion

    #endregion
}