using System.Collections.ObjectModel;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class ProblemElectivesViewModel : ViewModel
{
    #region Fields

    private readonly ObservableCollection<ProblemElective>? _incomplete;
    private readonly ObservableCollection<ProblemElective>? _overflowed;

    public ObservableCollection<ProblemElective>? Incomplete
    {
        get => _incomplete;
        private init => Set(ref _incomplete, value);
    }

    public ObservableCollection<ProblemElective>? Overflowed
    {
        get => _overflowed;
        private init => Set(ref _overflowed, value);
    }

    public ObservableCollection<ProblemElective>? Resolved { get; } = new();

    #endregion

    public ProblemElectivesViewModel()
    {
    }

    public ProblemElectivesViewModel(EventSource source) : base(source)
    {
        Incomplete = new ObservableCollection<ProblemElective>(DatabaseAccess.GetIncompleteElectives());
        Overflowed = new ObservableCollection<ProblemElective>(DatabaseAccess.GetOverflowedElectives());

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
    }

    #region Commands

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is ProblemElective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsLoading(this, new StatisticsEventArgs(((ProblemElective)parameter!).Name));
    }

    #endregion

    #endregion
}