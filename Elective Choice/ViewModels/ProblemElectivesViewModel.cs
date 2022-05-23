using System.Collections.Generic;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class ProblemElectivesViewModel : ViewModel
{
    #region Fields

    private string _name = string.Empty;
    private List<Elective>? _incomplete;
    private List<Elective>? _overflowed;
    private List<Elective>? _resolved;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public List<Elective>? Incomplete
    {
        get => _incomplete;
        set => Set(ref _incomplete, value);
    }

    public List<Elective>? Overflowed
    {
        get => _overflowed;
        set => Set(ref _overflowed, value);
    }

    public List<Elective>? Resolved
    {
        get => _resolved;
        set => Set(ref _resolved, value);
    }

    #endregion

    public ProblemElectivesViewModel()
    {
    }

    public ProblemElectivesViewModel(EventSource source) : base(source)
    {
        Incomplete = DatabaseAccess.GetIncompleteElectives();
        Overflowed = DatabaseAccess.GetOverflowedElectives();

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
    }

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is Elective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsLoading(this, new StatisticsEventArgs(((Elective) parameter!).Name));
    }

    #endregion
}