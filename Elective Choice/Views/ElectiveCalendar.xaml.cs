using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class ElectiveCalendar
{
    public ElectiveCalendar(EventSource source, string email)
    {
        InitializeComponent();
        DataContext = email == string.Empty
            ? new ElectiveCalendarViewModel(source)
            : new ElectiveCalendarViewModel(source, email);
        ((ElectiveCalendarViewModel) DataContext).TuesdayElectives.CollectionChanged += TuesdayElectives_OnChanged;
        ((ElectiveCalendarViewModel) DataContext).WednesdayElectives.CollectionChanged += WednesdayElectives_OnChanged;
        ((ElectiveCalendarViewModel) DataContext).ThurdayElectives.CollectionChanged += ThurdayElectives_OnChanged;
        ((ElectiveCalendarViewModel) DataContext).FridayElectives.CollectionChanged += FridayElectives_OnChanged;
    }

    private void TuesdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>) sender!;
        if (tuesdayElectives.Count < 3)
            Tuesday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Tuesday.Plus.Visibility = Visibility.Hidden;
    }

    private void WednesdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>) sender!;
        if (tuesdayElectives.Count < 3)
            Wednesday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Wednesday.Plus.Visibility = Visibility.Hidden;
    }

    private void ThurdayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>) sender!;
        if (tuesdayElectives.Count < 3)
            Thurday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Thurday.Plus.Visibility = Visibility.Hidden;
    }

    private void FridayElectives_OnChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CheckElectiveMaximum();

        var tuesdayElectives = (ObservableCollection<Elective>) sender!;
        if (tuesdayElectives.Count < 3)
            Friday.Plus.SetValue(Grid.RowProperty, tuesdayElectives.Count * 2 + 2);
        else
            Friday.Plus.Visibility = Visibility.Hidden;
    }

    private void CheckElectiveMaximum()
    {
        var dataContext = (ElectiveCalendarViewModel) DataContext;
        var isMaximum = dataContext.ElectiveCounter >= 5;
        Tuesday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Wednesday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Thurday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
        Friday.Plus.Visibility = isMaximum ? Visibility.Hidden : Visibility.Visible;
    }
}