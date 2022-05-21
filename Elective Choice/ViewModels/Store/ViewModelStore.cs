using System;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public event Action<string, bool>? LoginSucceed;
    public event Action<string>? LoginCompleted;
    public event Action<string, int?, bool?>? ElectiveStatisticsLoading;
    public event Action<string, int?, bool?>? ElectiveStatisticsLoaded;
    public event Action? ElectiveStatisticsClosed; 

    public void TriggerLoginSucceed(string email, bool rights) =>
        LoginSucceed?.Invoke(email, rights);

    public void TriggerLoginCompleted(string email) =>
        LoginCompleted?.Invoke(email);

    public void TriggerElectiveStatisticsLoading(string name, int? year = null, bool? season = null) =>
        ElectiveStatisticsLoading?.Invoke(name, year, season);

    public void TriggerElectiveStatisticsLoaded(string name, int? year, bool? season) =>
        ElectiveStatisticsLoaded?.Invoke(name, year, season);

    public void TriggerElectiveStatisticsClosed() => ElectiveStatisticsClosed?.Invoke();
}