namespace Elective_Choice.Models;

public class Elective
{
    public Elective(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }

    public string Name { get; set; }
    public int Capacity { get; set; }
}