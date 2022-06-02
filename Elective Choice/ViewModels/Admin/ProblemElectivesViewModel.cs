using System.Collections.ObjectModel;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels.Admin;

public class ProblemElectivesViewModel : ViewModel
{
    #region Fields

    private readonly ObservableCollection<Elective>? _incomplete;
    private readonly ObservableCollection<Elective>? _overflowed;

    public ObservableCollection<Elective>? Incomplete
    {
        get => _incomplete;
        private init => Set(ref _incomplete, value);
    }

    public ObservableCollection<Elective>? Overflowed
    {
        get => _overflowed;
        private init => Set(ref _overflowed, value);
    }

    public ObservableCollection<Elective>? Resolved { get; } = new();

    #endregion

    #region Constructor

    public ProblemElectivesViewModel()
    {
    }

    public ProblemElectivesViewModel(EventSource source) : base(source)
    {
        Incomplete = new ObservableCollection<Elective>(DatabaseAccess.GetIncompleteElectives());
        Overflowed = new ObservableCollection<Elective>(DatabaseAccess.GetOverflowedElectives());

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
    }

    #endregion

    #region Commands

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is Elective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsLoading(this, new StatisticsEventArgs(((Elective)parameter!).Name));
    }

    #endregion

    #endregion
}