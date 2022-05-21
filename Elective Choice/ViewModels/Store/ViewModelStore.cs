using System;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public event Action<string, bool>? LoginSucceed;
    public event Action<string>? LoginCompleted;
    public event Action<string, int?, bool?>? ElectiveStatisticsLoading;
    public event Action<string, int?, bool?>? ElectiveStatisticsLoaded;
    public event Action? ElectiveStatisticsClosed;
    public event Action<int, bool>? SemesterLoading;
    public event Action<int, bool>? SemesterLoaded;

    public void TriggerLoginSucceed(string email, bool rights) =>
        LoginSucceed?.Invoke(email, rights);

    public void TriggerLoginCompleted(string email) =>
        LoginCompleted?.Invoke(email);

    public void TriggerElectiveStatisticsLoading(string name, int? year = null, bool? spring = null) =>
        ElectiveStatisticsLoading?.Invoke(name, year, spring);

    public void TriggerElectiveStatisticsLoaded(string name, int? year, bool? spring) =>
        ElectiveStatisticsLoaded?.Invoke(name, year, spring);

    public void TriggerElectiveStatisticsClosed() => ElectiveStatisticsClosed?.Invoke();

    public void TriggerSemesterLoading(int year, bool spring) =>
        SemesterLoading?.Invoke(year, spring);

    public void TriggerSemesterLoaded(int year, bool spring) =>
        SemesterLoaded?.Invoke(year, spring);
}