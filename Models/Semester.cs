namespace Elective_Choice.Models;

public class Semester
{
    public int Year { get; set; }
    
    public string Season { get; set; }

    public Semester(int year, string season)
    {
        Year = year;
        Season = season;
    }
}