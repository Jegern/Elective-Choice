using System.Collections.ObjectModel;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels.Student;

public class PrioritiesViewModel : ViewModel
{
    #region Fields

    private ObservableCollection<Elective> _placedElectives = new();
    private ObservableCollection<Elective>? _unplacedElectives;

    private string Email { get; } = string.Empty;

    public ObservableCollection<Elective> PlacedElectives
    {
        get => _placedElectives;
        set => Set(ref _placedElectives, value);
    }

    public ObservableCollection<Elective>? UnplacedElectives
    {
        get => _unplacedElectives;
        set => Set(ref _unplacedElectives, value);
    }

    #endregion

    public PrioritiesViewModel()
    {
    }

    public PrioritiesViewModel(EventSource source, string email) : base(source)
    {
        Email = email;
        // var electives = DatabaseAccess.GetStudentElectives(email.Substring(4, 10));
    }
}