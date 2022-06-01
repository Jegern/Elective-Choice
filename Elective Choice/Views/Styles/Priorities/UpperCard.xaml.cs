using System.Windows;

namespace Elective_Choice.Views.Styles.Priorities;

public partial class UpperCard
{
    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background), 
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(default(string)));

    public new string Background
    {
        get => (string) GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    
    public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        nameof(BorderBrush), 
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(default(string)));

    public new string BorderBrush
    {
        get => (string) GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }
    
    public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(
        nameof(ContentVisibility),
        typeof(Visibility),
        typeof(UpperCard),
        new PropertyMetadata(Visibility.Visible));

    public Visibility ContentVisibility
    {
        get => (Visibility) GetValue(ContentVisibilityProperty);
        set => SetValue(ContentVisibilityProperty, value);
    }
    
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public UpperCard()
    {
        InitializeComponent();
    }
}