using System.Collections.Generic;
using Elective_Choice.Models;

namespace Elective_Choice.Infrastructure.EventArgs;

public class DayEventArgs : System.EventArgs
{
    public string Day { get; }
    public List<Elective> Electives { get; }

    public DayEventArgs(string day, List<Elective> electives)
    {
        Day = day;
        Electives = electives;
    }
}