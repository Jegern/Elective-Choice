using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class ElectiveCalendarViewModel : ViewModel
{
    #region Fields

    private ObservableCollection<Elective> _tuesdayElectives = new();
    private ObservableCollection<Elective> _wednesdayElectives = new();
    private ObservableCollection<Elective> _thurdayElectives = new();
    private ObservableCollection<Elective> _fridayElectives = new();

    private string Email { get; set; } = string.Empty;
    public int ElectiveCounter { get; private set; }

    public ObservableCollection<Elective> TuesdayElectives
    {
        get => _tuesdayElectives;
        set => Set(ref _tuesdayElectives, value);
    }

    public ObservableCollection<Elective> WednesdayElectives
    {
        get => _wednesdayElectives;
        set => Set(ref _wednesdayElectives, value);
    }

    public ObservableCollection<Elective> ThurdayElectives
    {
        get => _thurdayElectives;
        set => Set(ref _thurdayElectives, value);
    }

    public ObservableCollection<Elective> FridayElectives
    {
        get => _fridayElectives;
        set => Set(ref _fridayElectives, value);
    }

    #endregion

    public ElectiveCalendarViewModel()
    {
    }

    public ElectiveCalendarViewModel(EventSource source) : base(source)
    {
        source.LoginCompleted += Login_OnCompleted;
        source.DayElectiveChosen += DayElective_OnChosen;
        TuesdayElectives.CollectionChanged += DayElectives_OnChanged;
        WednesdayElectives.CollectionChanged += DayElectives_OnChanged;
        ThurdayElectives.CollectionChanged += DayElectives_OnChanged;
        FridayElectives.CollectionChanged += DayElectives_OnChanged;

        PlusCommand = new Command(
            PlusCommand_OnExecuted,
            PlusCommand_CanExecute);
        CrossCommand = new Command(
            CrossCommand_OnExecuted,
            CrossCommand_CanExecute);
    }

    public ElectiveCalendarViewModel(EventSource source, string email) : base(source)
    {
        source.DayElectiveChosen += DayElective_OnChosen;
        TuesdayElectives.CollectionChanged += DayElectives_OnChanged;
        WednesdayElectives.CollectionChanged += DayElectives_OnChanged;
        ThurdayElectives.CollectionChanged += DayElectives_OnChanged;
        FridayElectives.CollectionChanged += DayElectives_OnChanged;

        Login_OnCompleted(null, new LoginEventArgs(email, false));

        PlusCommand = new Command(
            PlusCommand_OnExecuted,
            PlusCommand_CanExecute);
        CrossCommand = new Command(
            CrossCommand_OnExecuted,
            CrossCommand_CanExecute);
    }

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        var id = e.Email.Substring(4, 10);

        var dayElectives = DatabaseAccess.GetStudentElectivesForDay(2, id);
        foreach (var elective in dayElectives)
            TuesdayElectives.Add(elective);

        dayElectives = DatabaseAccess.GetStudentElectivesForDay(3, id);
        foreach (var elective in dayElectives)
            WednesdayElectives.Add(elective);

        dayElectives = DatabaseAccess.GetStudentElectivesForDay(4, id);
        foreach (var elective in dayElectives)
            ThurdayElectives.Add(elective);

        dayElectives = DatabaseAccess.GetStudentElectivesForDay(5, id);
        foreach (var elective in dayElectives)
            FridayElectives.Add(elective);
    }

    private void DayElective_OnChosen(object? sender, DayEventArgs e)
    {
        switch (e.Day)
        {
            case "Вторник":
                TuesdayElectives.Add(e.Electives[^1]);
                break;
            case "Среда":
                WednesdayElectives.Add(e.Electives[^1]);
                break;
            case "Четверг":
                ThurdayElectives.Add(e.Electives[^1]);
                break;
            case "Пятница":
                FridayElectives.Add(e.Electives[^1]);
                break;
        }

        DatabaseAccess.AddStudentElective(Email.Substring(4, 10), e.Electives[^1].Name, ElectiveCounter);
    }

    private void DayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
            ElectiveCounter++;
        else
            ElectiveCounter--;
    }

    #endregion

    #region Commands

    #region PlusCommand

    public Command? PlusCommand { get; }

    private bool PlusCommand_CanExecute(object? parameter) =>
        parameter is string s &&
        s != string.Empty;

    private void PlusCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseDayLoading(this, new DayEventArgs((string) parameter!, (string) parameter! switch
        {
            "Вторник" => TuesdayElectives.ToList(),
            "Среда" => WednesdayElectives.ToList(),
            "Четверг" => ThurdayElectives.ToList(),
            "Пятница" => FridayElectives.ToList(),
            _ => new List<Elective>()
        }));
    }

    #endregion

    #region CrossCommand

    public Command? CrossCommand { get; }

    private bool CrossCommand_CanExecute(object? parameter) =>
        parameter is string s &&
        s != string.Empty;

    private void CrossCommand_OnExecuted(object? parameter)
    {
        var electiveName = (string) parameter!;
        RemoveElectiveFromObservableCollection(TuesdayElectives, electiveName);
        RemoveElectiveFromObservableCollection(WednesdayElectives, electiveName);
        RemoveElectiveFromObservableCollection(ThurdayElectives, electiveName);
        RemoveElectiveFromObservableCollection(FridayElectives, electiveName);

        DatabaseAccess.RemoveStudentElective(Email.Substring(4, 10), electiveName);
    }

    private static void RemoveElectiveFromObservableCollection(IList<Elective> electives, string name)
    {
        for (var i = 0; i < electives.Count; i++)
            if (electives[i].Name == name)
                electives.RemoveAt(i);
    }

    #endregion

    #endregion
}