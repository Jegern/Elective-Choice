using System.Collections.Generic;
using System.Linq;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.Comparers;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class DayElectivesViewModel : ViewModel
{
    #region Fields

    private string _headerText = string.Empty;
    private List<Elective>? _dayElectives;

    private string Day { get; set; } = string.Empty;
    private List<Elective>? Electives { get; set; }

    public string HeaderText
    {
        get => _headerText;
        set => Set(ref _headerText, value);
    }

    public List<Elective>? DayElectives
    {
        get => _dayElectives;
        set => Set(ref _dayElectives, value);
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
        Electives = e.Electives;
        HeaderText = $"Элективы на {e.Day switch {"Среда" => "среду", "Пятница" => "пятницу", _ => e.Day.ToLower()}}";
        DayElectives = DatabaseAccess.GetElectivesForDay(e.Day switch
        {
            "Вторник" => 2,
            "Среда" => 3,
            "Четверг" => 4,
            "Пятница" => 5,
            _ => 0
        }).Except(Electives, new ElectiveComparer()).ToList();
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
        Electives!.Add((Elective) parameter!);
        Source?.RaiseDayElectiveChosen(this, new DayEventArgs(Day, Electives!));
        Source?.RaiseDayClosing(this, new DayEventArgs(Day, Electives!));
    }

    #endregion

    #region GoBackCommand

    public Command? GoBackCommand { get; }

    private bool GoBackCommand_CanExecute(object? parameter) => true;

    private void GoBackCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseDayClosing(this, new DayEventArgs(Day, Electives!));
    }

    #endregion

    #endregion
}