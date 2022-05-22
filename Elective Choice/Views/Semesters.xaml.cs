using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class Semesters
{
    public Semesters(EventSource store)
    {
        InitializeComponent();
        DataContext = new SemestersViewModel(store);
    }
}