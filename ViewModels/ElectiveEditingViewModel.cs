using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class ElectiveEditingViewModel : ViewModel
{
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

    public Elective[] Electives { get; set; } = 
    {
        new("kek", 20),
        new("lol", 40)
    };
}