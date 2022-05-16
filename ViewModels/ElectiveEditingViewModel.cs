using System.Collections.ObjectModel;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Npgsql;

namespace Elective_Choice.ViewModels;

public class ElectiveEditingViewModel : ViewModel
{
    public ObservableCollection<Elective>? Electives { get; } = new()
    {
        new Elective("kek", 20)
    };

    public ElectiveEditingViewModel(ViewModelStore? store) : base(store)
    {
        Electives = GetCurrentElectives();
    }

    public ElectiveEditingViewModel()
    {
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
}