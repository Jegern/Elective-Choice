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
        Semesters = new ObservableCollection<Semester>(DatabaseAccess.GetSemesters());
    }
}