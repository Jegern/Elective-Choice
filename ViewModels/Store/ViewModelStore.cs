using System;

namespace Elective_Choice.ViewModels.Store;

public class ViewModelStore
{
    public event Action<string, bool>? SuccessfulLogin;

    public void TriggerSuccessfullyLoginEvent(string username, bool rights) => 
        SuccessfulLogin?.Invoke(username, rights);
}