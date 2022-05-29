using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
                TuesdayElectives.Add(e.Elective!);
                break;
            case "Среда":
                WednesdayElectives.Add(e.Elective!);
                break;
            case "Четверг":
                ThurdayElectives.Add(e.Elective!);
                break;
            case "Пятница":
                FridayElectives.Add(e.Elective!);
                break;
        }

        DatabaseAccess.AddStudentElective(Email.Substring(4, 10), e.Elective!.Name, ElectiveCounter);
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
        Source?.RaiseDayLoading(this, new DayEventArgs((string) parameter!));
    }

    #endregion

    #region CrossCommand

    public Command? CrossCommand { get; }

    private bool CrossCommand_CanExecute(object? parameter) =>
        parameter is string s &&
        s != string.Empty;

    private void CrossCommand_OnExecuted(object? parameter)
    {
        for (var i = 0; i < TuesdayElectives.Count; i++)
            if (TuesdayElectives[i].Name == (string) parameter!)
                TuesdayElectives.RemoveAt(i);

        for (var i = 0; i < WednesdayElectives.Count; i++)
            if (WednesdayElectives[i].Name == (string) parameter!)
                WednesdayElectives.RemoveAt(i);

        for (var i = 0; i < ThurdayElectives.Count; i++)
            if (ThurdayElectives[i].Name == (string) parameter!)
                ThurdayElectives.RemoveAt(i);

        for (var i = 0; i < FridayElectives.Count; i++)
            if (FridayElectives[i].Name == (string) parameter!)
                FridayElectives.RemoveAt(i);
        
        DatabaseAccess.RemoveStudentElective(Email.Substring(4, 10), (string) parameter!);
    }

    #endregion

    #endregion
}