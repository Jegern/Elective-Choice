using System.Collections.Generic;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels.Admin;

public class SemestersViewModel : ViewModel
{
    #region Fields

    public List<Semester>? Semesters { get; }

    #endregion

    #region Constructor

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

    #endregion

    #region OpenElectiveCommand

    public Command? OpenSemesterCommand { get; }

    private bool OpenSemesterCommand_CanExecute(object? parameter) => parameter is Semester;

    private void OpenSemesterCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseSemesterLoading(this,
            new SemesterEventArgs(((Semester) parameter!).Year, ((Semester) parameter).Spring == "Весна"));
    }

    #endregion
}