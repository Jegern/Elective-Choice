using System;
using Microsoft.VisualBasic;
using Npgsql;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public NpgsqlConnection SqlConnection { get; } =
        new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");

    public event Action<string, bool>? SuccessfulLogin;
    public event Action<string>? LoginCompleted;
    public event Action<string, int, string>? ElectiveStatisticsLoading;
    public event Action<string, int, string>? ElectiveStatisticsLoaded;

    public void TriggerSuccessfulLogin(string email, bool rights) =>
        SuccessfulLogin?.Invoke(email, rights);

    public void TriggerLoginCompleted(string email) =>
        LoginCompleted?.Invoke(email);

    public void TriggerElectiveStatisticsLoading(string name, int year, string season) =>
        ElectiveStatisticsLoading?.Invoke(name, year, season);

    public void TriggerElectiveStatisticsLoaded(string name, int year, string season) =>
        ElectiveStatisticsLoaded?.Invoke(name, year, season);
}