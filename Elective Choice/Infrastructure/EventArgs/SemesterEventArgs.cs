namespace Elective_Choice.Infrastructure.EventArgs;

public class SemesterEventArgs : System.EventArgs
{
    public int Year { get; }
    public bool Spring { get; }

    public SemesterEventArgs(int year, bool spring)
    {
        Year = year;
        Spring = spring;
    }
}