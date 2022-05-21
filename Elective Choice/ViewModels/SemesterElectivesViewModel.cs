using System.Collections.Generic;
using Elective_Choice.Models;
using Elective_Choice.Commands.Base;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class SemesterElectivesViewModel : ViewModel
{
    #region Fields

    private int _year;
    private string _spring = string.Empty;
    private List<Elective>? _electives;

    public int Year
    {
        get => _year;
        set => Set(ref _year, value);
    }

    public string Spring
    {
        get => _spring;
        set => Set(ref _spring, value);
    }

    public List<Elective>? Electives
    {
        get => _electives;
        set => Set(ref _electives, value);
    }

    #endregion

    public SemesterElectivesViewModel()
    {
    }

    public SemesterElectivesViewModel(ViewModelStore store) : base(store)
    {
        store.SemesterLoaded += Semester_OnLoaded;

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
    }

    #region Event Subscription

    private void Semester_OnLoaded(int year, bool spring)
    {
        // TODO: Исправить множественную подписку на событие
        Year = year;
        Spring = spring ? "Весна" : "Осень";
        Electives = DatabaseAccess.GetSemesterElectives(year, spring);
    }

    #endregion

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is Elective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Store?.TriggerElectiveStatisticsLoading(((Elective) parameter!).Name, Year, Spring == "Весна");
    }

    #endregion
}