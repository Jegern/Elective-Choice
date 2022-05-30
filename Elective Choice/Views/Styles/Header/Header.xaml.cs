using System.Windows;
using System.Windows.Input;

namespace Elective_Choice.Views.Styles.Header;

public partial class Header
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(Header),
        new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(
        nameof(IconVisibility),
        typeof(Visibility),
        typeof(Header),
        new PropertyMetadata(default(Visibility)));

    public Visibility IconVisibility
    {
        get => (Visibility) GetValue(IconVisibilityProperty);
        set => SetValue(IconVisibilityProperty, value);
    }

    public static readonly DependencyProperty GoBackCommandProperty = DependencyProperty.Register(
        nameof(GoBackCommand),
        typeof(ICommand),
        typeof(Header),
        new PropertyMetadata(null));

    public ICommand GoBackCommand
    {
        get => (ICommand) GetValue(GoBackCommandProperty);
        set => SetValue(GoBackCommandProperty, value);
    }
    
    public Header()
    {
        InitializeComponent();
    }
}