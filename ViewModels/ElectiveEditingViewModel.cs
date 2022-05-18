using System.Collections.ObjectModel;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Laboratory_work_1.Commands.Base;
using Npgsql;

namespace Elective_Choice.ViewModels;

public class ElectiveEditingViewModel : ViewModel
{
    public ObservableCollection<Elective>? Electives { get; }
    private Elective? _selectedElective;
    
    public Elective? SelectedElective
    {
        get => _selectedElective;
        set => Set(ref _selectedElective, value);
    }
    
    public ElectiveEditingViewModel()
    {
    }

    public ElectiveEditingViewModel(ViewModelStore? store) : base(store)
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

    private bool OpenElectiveCommand_CanExecute(object? parameter) => SelectedElective is not null;

    private void OpenElectiveCommand_OnExecuted(object? parameter)
    {
        
    }

    #endregion

    #endregion
}