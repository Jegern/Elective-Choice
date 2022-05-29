using Elective_Choice.Models;

namespace Elective_Choice.Infrastructure.EventArgs;

public class DayEventArgs : System.EventArgs
{
    public string Day { get; }
    public Elective? Elective { get; }

    public DayEventArgs(string day, Elective? elective = null)
    {
        Day = day;
        Elective = elective;
    }
}