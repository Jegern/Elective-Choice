using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class ElectiveStatisticsViewModel : ViewModel
{
    public ElectiveStatisticsViewModel()
    {
    }

    public ElectiveStatisticsViewModel(ViewModelStore store) : base(store)
    {
        store.ElectiveStatisticsLoaded += ElectiveStatisticsLoaded_OnChanged;
    }

    private void ElectiveStatisticsLoaded_OnChanged(string name)
    {
        Store?.SqlConnection.Open();
        
        // TODO: Сделать запрос к БД и получить статистику об элективе
        
        Store?.SqlConnection.Close();
    }
}