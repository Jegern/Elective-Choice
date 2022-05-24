namespace Elective_Choice.Models;

public class ProblemElective
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int Counts { get; set; }
    public string Problem { get; set; }

    public ProblemElective(string name, int capacity, int counts, string problem)
    {
        Name = name;
        Capacity = capacity;
        Counts = counts;
        Problem = problem;
    }
}