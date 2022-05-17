namespace Elective_Choice.Models;

public class Elective
{
    public string Name { get; set; }

    public int Capacity { get; set; }

    public Elective(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }
}