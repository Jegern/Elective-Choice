using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Student;

namespace Elective_Choice.Views.Student;

public partial class ElectiveCalendar
{
    public ElectiveCalendar(EventSource source, string email)
    {
        InitializeComponent();
        DataContext = email == string.Empty
            ? new ElectiveCalendarViewModel(source)
            : new ElectiveCalendarViewModel(source, email);
        
        var dataContext = (ElectiveCalendarViewModel)DataContext;
        dataContext.TuesdayElectives.CollectionChanged += TuesdayElectives_OnChanged;
        dataContext.WednesdayElectives.CollectionChanged += WednesdayElectives_OnChanged;
        dataContext.ThurdayElectives.CollectionChanged += ThurdayElectives_OnChanged;
        dataContext.FridayElectives.CollectionChanged += FridayElectives_OnChanged;
    }

    private void ElectiveCalendar_OnLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (ElectiveCalendarViewModel)DataContext;
        var id = dataContext.Email.Substring(4, 10);
        if (dataContext.ElectiveCounter > 0) return;

        var electivesByDay = new[]
        {
            dataContext.TuesdayElectives,
            dataContext.WednesdayElectives,
            dataContext.ThurdayElectives,
            dataContext.FridayElectives
        };
        
        var electives = DatabaseAccess.GetStudentElectives(id);
        foreach (var elective in electives)
            electivesByDay[elective.Day - 2].Add(elective);
    }

    private void TuesdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>)sender!;
        if (tuesdayElectives.Count < 3)
            Tuesday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Tuesday.Plus.Visibility = Visibility.Hidden;
    }

    private void WednesdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>)sender!;
        if (tuesdayElectives.Count < 3)
            Wednesday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Wednesday.Plus.Visibility = Visibility.Hidden;
    }

    private void ThurdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>)sender!;
        if (tuesdayElectives.Count < 3)
            Thurday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Thurday.Plus.Visibility = Visibility.Hidden;
    }

    private void FridayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>)sender!;
        if (tuesdayElectives.Count < 3)
            Friday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Friday.Plus.Visibility = Visibility.Hidden;
    }

    private void CheckElectiveMaximum()
    {
        var dataContext = (ElectiveCalendarViewModel)DataContext;
        var isMaximum = dataContext.ElectiveCounter >= 5;
        Tuesday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Wednesday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Thurday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Friday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
    }
}