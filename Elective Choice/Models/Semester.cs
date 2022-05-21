namespace Elective_Choice.Models;

public class Semester
{
    public int Year { get; set; }
    
    public string Spring { get; set; }
    
    public int NumberOfElectives { get; set; }

    public Semester(int year, bool spring, int numberOfElectives)
    {
        Year = year;
        Spring = spring ? "Весна" : "Осень";
        NumberOfElectives = numberOfElectives;
    }
}