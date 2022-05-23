namespace Elective_Choice.Infrastructure.EventArgs;

public class LoginEventArgs : System.EventArgs
{
    public string Email { get; }
    public bool Rights { get; }

    public LoginEventArgs(string email, bool rights)
    {
        Email = email;
        Rights = rights;
    }
}