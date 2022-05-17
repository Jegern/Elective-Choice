namespace Elective_Choice.Models;

public class Semester
{
    public int Year { get; set; }
    
    public bool IsFirst { get; set; }

    public Semester(int year, bool isFirst)
    {
        Year = year;
        IsFirst = isFirst;
    }
}