using System.Collections.Generic;
using Elective_Choice.Models;

namespace Elective_Choice.Infrastructure.EventArgs;

public class ResultsEventArgs : System.EventArgs
{
    public string Email { get; }

    public ResultsEventArgs(string email)
    {
        Email = email;
    }
}