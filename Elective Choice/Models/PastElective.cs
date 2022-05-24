namespace Elective_Choice.Models;

public class PastElective
{
    public string Name { get; set; }

    public int Capacity { get; set; }

    public PastElective(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }
}