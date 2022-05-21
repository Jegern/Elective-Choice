namespace Elective_Choice.Models;

public class Semester
{
    public int Year { get; set; }
    
    public bool Spring { get; set; }

    public Semester(int year, bool spring)
    {
        Year = year;
        Spring = spring;
    }
}