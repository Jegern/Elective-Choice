using System;
using Elective_Choice.Infrastructure.EventArgs;

namespace Elective_Choice.Infrastructure.EventSource;

public class EventSource
{
    public event EventHandler<LoginEventArgs>? LoginSucceed;
    public event EventHandler<LoginEventArgs>? LoginCompleted;
    public event EventHandler<StatisticsEventArgs>? StatisticsLoading;
    public event EventHandler<StatisticsEventArgs>? StatisticsLoaded;
    public event EventHandler<StatisticsEventArgs>? StatisticsClosing;
    public event EventHandler<SemesterEventArgs>? SemesterLoading;
    public event EventHandler<SemesterEventArgs>? SemesterLoaded;
    public event EventHandler<SemesterEventArgs>? SemesterClosing;

    public void RaiseLoginSucceed(object? sender, LoginEventArgs e) => LoginSucceed?.Invoke(sender, e);

    public void RaiseLoginCompleted(object? sender, LoginEventArgs e) => LoginCompleted?.Invoke(sender, e);

    public void RaiseStatisticsLoading(object? sender, StatisticsEventArgs e) => StatisticsLoading?.Invoke(sender, e);

    public void RaiseStatisticsLoaded(object? sender, StatisticsEventArgs e) => StatisticsLoaded?.Invoke(sender, e);

    public void RaiseStatisticsClosing(object? sender, StatisticsEventArgs e) => StatisticsClosing?.Invoke(sender, e);

    public void RaiseSemesterLoading(object? sender, SemesterEventArgs e) => SemesterLoading?.Invoke(sender, e);

    public void RaiseSemesterLoaded(object? sender, SemesterEventArgs e) => SemesterLoaded?.Invoke(sender, e);
    
    public void RaiseSemesterClosing(object? sender, SemesterEventArgs e) => SemesterClosing?.Invoke(sender, e);
}