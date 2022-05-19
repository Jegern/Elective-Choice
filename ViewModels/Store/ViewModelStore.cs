using System;
using Npgsql;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public NpgsqlConnection SqlConnection { get; } =
        new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");

    public event Action<string, bool>? SuccessfulLogin;
    public event Action<string>? LoginCompleted;
    public event Action<string>? ElectiveStatisticsLoading;
    public event Action<string>? ElectiveStatisticsLoaded;

    public void TriggerSuccessfulLogin(string email, bool rights) =>
        SuccessfulLogin?.Invoke(email, rights);

    public void TriggerLoginCompleted(string email) =>
        LoginCompleted?.Invoke(email);

    public void TriggerElectiveStatisticsLoading(string name) =>
        ElectiveStatisticsLoading?.Invoke(name);
    
    public void TriggerElectiveStatisticsLoaded(string name) =>
        ElectiveStatisticsLoaded?.Invoke(name);
}