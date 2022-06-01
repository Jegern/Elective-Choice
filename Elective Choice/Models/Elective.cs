namespace Elective_Choice.Models;

public class Elective
{
    public string Name { get; set; }

    public int Capacity { get; set; }

    public string Annotation { get; set; }
    
    public string Link { get; set; }

    public string Problem { get; set; }
    
    public int Day { get; set; }
    
    public int Priority { get; set; }

    public Elective(string name,
        int capacity,
        string annotation = "",
        string link = "",
        string problem = "",
        int day = 0,
        int priority = 0)
    {
        Name = name;
        Capacity = capacity;
        Annotation = annotation;
        Link = link;
        Problem = problem;
        Day = day;
        Priority = priority;
    }
}