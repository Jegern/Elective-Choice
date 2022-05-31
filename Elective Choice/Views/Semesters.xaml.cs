using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views;

public partial class Semesters
{
    public Semesters(EventSource store)
    {
        InitializeComponent();
        DataContext = new SemestersViewModel(store);
    }
}