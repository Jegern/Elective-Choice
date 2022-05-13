using System;
using Npgsql;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public NpgsqlConnection SqlConnection { get; } =
        new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");
    
    public event Action<string, bool>? SuccessfulLogin;
    public event Action<string>? LoginComplete;

    public void TriggerSuccessfulLoginEvent(string email, bool rights) => 
        SuccessfulLogin?.Invoke(email, rights);
    public void TriggerLoginCompleteEvent(string email) => 
        LoginComplete?.Invoke(email);
}