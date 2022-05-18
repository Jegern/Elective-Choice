using System.Collections.ObjectModel;
using System.Windows.Controls;
using Elective_Choice.Models;
using Elective_Choice.Commands.Base;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Npgsql;

namespace Elective_Choice.ViewModels;

public class CurrentElectivesViewModel : ViewModel
{
    #region Fields

    public ObservableCollection<Elective>? Electives { get; }
    private Page? _frameContent;

    public Page? FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    #endregion
    
    public CurrentElectivesViewModel()
    {
    }

    public CurrentElectivesViewModel(ViewModelStore store) : base(store)
    {
        Electives = GetCurrentElectives();

        OpenElectiveCommand = new Command(
            OpenElectiveCommand_OnExecuted,
            OpenElectiveCommand_CanExecute);
    }

    private ObservableCollection<Elective> GetCurrentElectives()
    {
        var electives = new ObservableCollection<Elective>();

        Store?.SqlConnection.Open();
        var reader = new NpgsqlCommand(
            "SELECT electiveName, capacity " +
            "FROM electives " +
            "ORDER BY electiveName", Store?.SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(reader.GetString(0), reader.GetInt32(1)));
        Store?.SqlConnection.Close();

        return electives;
    }

    #region Commands

    #region OpenElectiveCommand

    public Command? OpenElectiveCommand { get; }

    private bool OpenElectiveCommand_CanExecute(object? parameter) => parameter is Elective;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        Store?.TriggerElectiveStatisticsLoading(((Elective) parameter!).Name);
    }

    #endregion

    #endregion
}