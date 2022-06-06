using System;
using Elective_Choice.Infrastructure.EventArgs;

namespace Elective_Choice.Infrastructure.EventSource;

public class EventSource
{
    public event EventHandler<LoginEventArgs>? LoginSucceed;
    public event EventHandler<LoginEventArgs>? LoginCompleted;
    public event EventHandler<LoginEventArgs>? LogoutSucceed;
    public event EventHandler<StatisticsEventArgs>? StatisticsLoading;
    public event EventHandler<StatisticsEventArgs>? StatisticsLoaded;
    public event EventHandler<StatisticsEventArgs>? StatisticsClosing;
    public event EventHandler<DayEventArgs>? DayLoading;
    public event EventHandler<DayEventArgs>? DayLoaded;
    public event EventHandler<DayEventArgs>? DayClosing;
    public event EventHandler<DayEventArgs>? DayElectiveChosen;
    public event EventHandler<SemesterEventArgs>? SemesterLoading;
    public event EventHandler<SemesterEventArgs>? SemesterLoaded;
    public event EventHandler<SemesterEventArgs>? SemesterClosing;
    public event EventHandler<System.EventArgs>? CalendarClosing;
    public event EventHandler<System.EventArgs>? PrioritiesClosing;
    public event EventHandler<System.EventArgs>? AlgorithmSettingClosing;
    public event EventHandler<System.EventArgs>? ResultsClosing;
    public event EventHandler<ResultsEventArgs>? ResultsLoaded;



    public void RaiseLoginSucceed(object? sender, LoginEventArgs e) => LoginSucceed?.Invoke(sender, e);

    public void RaiseLoginCompleted(object? sender, LoginEventArgs e) => LoginCompleted?.Invoke(sender, e);

    public void RaiseLogoutSucceed(object? sender, LoginEventArgs e) => LogoutSucceed?.Invoke(sender, e);

    public void RaiseStatisticsLoading(object? sender, StatisticsEventArgs e) => StatisticsLoading?.Invoke(sender, e);

    public void RaiseStatisticsLoaded(object? sender, StatisticsEventArgs e) => StatisticsLoaded?.Invoke(sender, e);

    public void RaiseStatisticsClosing(object? sender, StatisticsEventArgs e) => StatisticsClosing?.Invoke(sender, e);

    public void RaiseDayLoading(object? sender, DayEventArgs e) => DayLoading?.Invoke(sender, e);

    public void RaiseDayLoaded(object? sender, DayEventArgs e) => DayLoaded?.Invoke(sender, e);

    public void RaiseDayClosing(object? sender, DayEventArgs e) => DayClosing?.Invoke(sender, e);

    public void RaiseDayElectiveChosen(object? sender, DayEventArgs e) => DayElectiveChosen?.Invoke(sender, e);

    public void RaiseSemesterLoading(object? sender, SemesterEventArgs e) => SemesterLoading?.Invoke(sender, e);

    public void RaiseSemesterLoaded(object? sender, SemesterEventArgs e) => SemesterLoaded?.Invoke(sender, e);

    public void RaiseSemesterClosing(object? sender, SemesterEventArgs e) => SemesterClosing?.Invoke(sender, e);

    public void RaiseCalendarClosing(object? sender, System.EventArgs e) => CalendarClosing?.Invoke(sender, e);
    
    public void RaisePrioritiesClosing(object? sender, System.EventArgs e) => PrioritiesClosing?.Invoke(sender, e);

    public void RaiseAlgorithmSettingClosing(object? sender, System.EventArgs e) => AlgorithmSettingClosing?.Invoke(sender, e);
    public void RaiseResultsClosing(object? sender, System.EventArgs e) => ResultsClosing?.Invoke(sender, e);
    public void RaiseResultsLoaded(object? sender, ResultsEventArgs e) => ResultsLoaded?.Invoke(sender, e);
}