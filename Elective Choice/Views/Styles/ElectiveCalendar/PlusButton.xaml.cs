using System.Windows;
using System.Windows.Input;

namespace Elective_Choice.Views.Styles.ElectiveCalendar;

public partial class PlusButton
{
    public static readonly DependencyProperty PlusCommandProperty = DependencyProperty.Register(
        nameof(PlusCommand),
        typeof(ICommand),
        typeof(PlusButton),
        new PropertyMetadata(null));

    public ICommand? PlusCommand
    {
        get => (ICommand) GetValue(PlusCommandProperty);
        set => SetValue(PlusCommandProperty, value);
    }

    public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
        nameof(Day),
        typeof(string),
        typeof(PlusButton),
        new PropertyMetadata(default(string)));

    public string Day
    {
        get => (string) GetValue(DayProperty);
        set => SetValue(DayProperty, value);
    }
    
    public PlusButton()
    {
        InitializeComponent();
    }

    private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        PlusCommand?.Execute(Day);
    }
}