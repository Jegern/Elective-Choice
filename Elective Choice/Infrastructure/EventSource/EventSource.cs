using System;
using Elective_Choice.Infrastructure.EventArgs;

namespace Elective_Choice.Infrastructure.EventSource;

public class EventSource
{
    public int kek = 1;
    
    public event Action<string, bool>? LoginSucceed;
    public event Action<string>? LoginCompleted;
    public event Action<string, int?, bool?>? StatisticsLoading;
    public event EventHandler<StatisticsEventArgs>? StatisticsLoaded;
    public event Action? StatisticsClosed;
    public event Action<int, bool>? SemesterLoading;
    public event Action<int, bool>? SemesterLoaded;

    public void RaiseLoginSucceed(string email, bool rights) =>
        LoginSucceed?.Invoke(email, rights);

    public void RaiseLoginCompleted(string email) =>
        LoginCompleted?.Invoke(email);

    public void RaiseStatisticsLoading(string name, int? year = null, bool? spring = null) =>
        StatisticsLoading?.Invoke(name, year, spring);

    public void RaiseStatisticsLoaded(object? sender, StatisticsEventArgs e) =>
        StatisticsLoaded?.Invoke(sender, e);

    public void RaiseStatisticsClosed() => StatisticsClosed?.Invoke();

    public void RaiseSemesterLoading(int year, bool spring) =>
        SemesterLoading?.Invoke(year, spring);

    public void RaiseSemesterLoaded(int year, bool spring) =>
        SemesterLoaded?.Invoke(year, spring);
}