using System;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public event Action<string, bool>? SuccessfulLogin;
    public event Action<string>? LoginComplete;

    public void TriggerSuccessfulLoginEvent(string email, bool rights) => 
        SuccessfulLogin?.Invoke(email, rights);
    public void TriggerLoginCompleteEvent(string email) => 
        LoginComplete?.Invoke(email);
}