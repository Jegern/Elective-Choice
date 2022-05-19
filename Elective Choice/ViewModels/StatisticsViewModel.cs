using System.Collections.ObjectModel;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Npgsql;

namespace Elective_Choice.ViewModels;

public class StatisticsViewModel : ViewModel
{
    public ObservableCollection<Semester>? Semesters { get; }

    public StatisticsViewModel()
    {
    }
    
    public StatisticsViewModel(ViewModelStore store) : base(store)
    {
        Semesters = GetSemesters();
    }

    private ObservableCollection<Semester> GetSemesters()
    {
        var semesters = new ObservableCollection<Semester>();

        Store?.SqlConnection.Open();
        var reader = new NpgsqlCommand(
            "SELECT yearofpassage, semester " + 
            "FROM selected_electives " + 
            "GROUP BY yearofpassage, semester", Store?.SqlConnection).ExecuteReader();
        while (reader.Read())
        {
            // TODO: Переделать базу данных (?), возвращать bool вместо строки для определения семестра
            semesters.Add(new Semester(reader.GetInt32(0), reader.GetString(1)));
        }
        Store?.SqlConnection.Close();
        
        return semesters;
    }
}