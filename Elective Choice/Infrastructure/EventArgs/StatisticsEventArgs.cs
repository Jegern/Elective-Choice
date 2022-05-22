namespace Elective_Choice.Infrastructure.EventArgs;

public class StatisticsEventArgs : System.EventArgs 
{
    public string Name { get; }
    public int? Year { get; }
    public bool? Spring { get; }

    public StatisticsEventArgs(string name, int? year = null, bool? spring = null)
    {
        Name = name;
        Year = year;
        Spring = spring;
    }
}