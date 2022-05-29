using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Elective_Choice.Models;

namespace Elective_Choice.Views.Styles.ElectiveCalendar;

public partial class DayColumn
{
    public static readonly DependencyProperty DayTextProperty = DependencyProperty.Register(
        nameof(DayText),
        typeof(string),
        typeof(DayColumn),
        new PropertyMetadata(default(string)));

    public string DayText
    {
        get => (string) GetValue(DayTextProperty);
        set => SetValue(DayTextProperty, value);
    }

    public static readonly DependencyProperty ElectivesProperty = DependencyProperty.Register(
        nameof(Electives),
        typeof(ObservableCollection<Elective>),
        typeof(DayColumn),
        new PropertyMetadata(default(ObservableCollection<Elective>)));

    public ObservableCollection<Elective> Electives
    {
        get => (ObservableCollection<Elective>) GetValue(ElectivesProperty);
        set => SetValue(ElectivesProperty, value);
    }

    public static readonly DependencyProperty PlusCommandProperty = DependencyProperty.Register(
        nameof(PlusCommand),
        typeof(ICommand),
        typeof(DayColumn),
        new PropertyMetadata(default(ICommand)));

    public ICommand? PlusCommand
    {
        get => (ICommand) GetValue(PlusCommandProperty);
        set => SetValue(PlusCommandProperty, value);
    }

    public static readonly DependencyProperty CrossCommandProperty = DependencyProperty.Register(
        nameof(CrossCommand),
        typeof(ICommand),
        typeof(DayColumn),
        new PropertyMetadata(default(ICommand)));

    public ICommand? CrossCommand
    {
        get => (ICommand) GetValue(CrossCommandProperty);
        set => SetValue(CrossCommandProperty, value);
    }

    public DayColumn()
    {
        InitializeComponent();
    }
}