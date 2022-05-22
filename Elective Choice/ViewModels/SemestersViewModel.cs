using System.Collections.Generic;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class SemestersViewModel : ViewModel
{
    public List<Semester>? Semesters { get; }

    public SemestersViewModel()
    {
    }

    public SemestersViewModel(EventSource source) : base(source)
    {
        Semesters = DatabaseAccess.GetSemesters();

        OpenSemesterCommand = new Command(
            OpenSemesterCommand_OnExecuted,
            OpenSemesterCommand_CanExecute);
    }

    #region OpenElectiveCommand

    public Command? OpenSemesterCommand { get; }

    private bool OpenSemesterCommand_CanExecute(object? parameter) => parameter is Semester;

    private void OpenSemesterCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseSemesterLoading(((Semester) parameter!).Year, ((Semester) parameter).Spring == "Весна");
    }

    #endregion
}