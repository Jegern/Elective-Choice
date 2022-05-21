using System.Collections.ObjectModel;
using Elective_Choice.Commands.Base;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class PastSemestersViewModel : ViewModel
{
    public ObservableCollection<Semester>? Semesters { get; }

    public PastSemestersViewModel()
    {
    }

    public PastSemestersViewModel(ViewModelStore store) : base(store)
    {
        Semesters = new ObservableCollection<Semester>(DatabaseAccess.GetSemesters());

        OpenSemesterCommand = new Command(
            OpenSemesterCommand_OnExecuted,
            OpenSemesterCommand_CanExecute);
    }

    #region OpenElectiveCommand

    public Command? OpenSemesterCommand { get; }

    private bool OpenSemesterCommand_CanExecute(object? parameter) => parameter is Semester;

    private void OpenSemesterCommand_OnExecuted(object? parameter)
    {
    }

    #endregion
}