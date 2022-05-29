namespace Elective_Choice.Models;

public class Elective
{
    public string Name { get; set; }

    public int Capacity { get; set; }

    public string Annotation { get; set; }
    
    public string Link { get; set; }
    
    public int Counts { get; set; }
    
    public string Problem { get; set; }

    public Elective(string name,
        int capacity,
        string annotation = "",
        string link = "",
        int counts = 0,
        string problem = "")
    {
        Name = name;
        Capacity = capacity;
        Annotation = annotation;
        Link = link;
        Counts = counts;
        Problem = problem;
    }
}