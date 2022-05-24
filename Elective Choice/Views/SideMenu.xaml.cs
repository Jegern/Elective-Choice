using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;

namespace Elective_Choice.Views;

public partial class SideMenu
{
    #region Dependency Properties

    public static readonly DependencyProperty FirstTextProperty = DependencyProperty.Register(
        nameof(FirstText),
        typeof(string),
        typeof(SideMenu),
        new PropertyMetadata(string.Empty));

    public string FirstText
    {
        get => (string) GetValue(FirstTextProperty);
        set => SetValue(FirstTextProperty, value);
    }

    public static readonly DependencyProperty SecondTextProperty = DependencyProperty.Register(
        nameof(SecondText),
        typeof(string),
        typeof(SideMenu),
        new PropertyMetadata(string.Empty));

    public string SecondText
    {
        get => (string) GetValue(SecondTextProperty);
        set => SetValue(SecondTextProperty, value);
    }

    public static readonly DependencyProperty ThirdTextProperty = DependencyProperty.Register(
        nameof(ThirdText),
        typeof(string),
        typeof(SideMenu),
        new PropertyMetadata(string.Empty));

    public string ThirdText
    {
        get => (string) GetValue(ThirdTextProperty);
        set => SetValue(ThirdTextProperty, value);
    }

    public static readonly DependencyProperty FirstSourceProperty = DependencyProperty.Register(
        nameof(FirstSource),
        typeof(ImageSource),
        typeof(SideMenu),
        new PropertyMetadata(default(ImageSource)));

    public ImageSource FirstSource
    {
        get => (ImageSource) GetValue(FirstSourceProperty);
        set => SetValue(FirstSourceProperty, value);
    }

    public static readonly DependencyProperty SecondSourceProperty = DependencyProperty.Register(
        nameof(SecondSource),
        typeof(ImageSource),
        typeof(SideMenu),
        new PropertyMetadata(default(ImageSource)));

    public ImageSource SecondSource
    {
        get => (ImageSource) GetValue(SecondSourceProperty);
        set => SetValue(SecondSourceProperty, value);
    }

    public static readonly DependencyProperty ThirdSourceProperty = DependencyProperty.Register(
        nameof(ThirdSource),
        typeof(ImageSource),
        typeof(SideMenu),
        new PropertyMetadata(default(ImageSource)));

    public ImageSource ThirdSource
    {
        get => (ImageSource) GetValue(ThirdSourceProperty);
        set => SetValue(ThirdSourceProperty, value);
    }

    #endregion

    #region Routed Events

    public static readonly RoutedEvent FirstClickEvent = EventManager.RegisterRoutedEvent(
        nameof(FirstClick),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(SideMenu));

    public event RoutedEventHandler FirstClick
    {
        add => AddHandler(FirstClickEvent, value);
        remove => RemoveHandler(FirstClickEvent, value);
    }

    private void First_OnClick(object? sender, RoutedEventArgs e)
    {
        First.IsEnabled = false;
        Second.IsEnabled = true;
        Third.IsEnabled = true;
        RaiseEvent(new RoutedEventArgs(FirstClickEvent));
    }

    public static readonly RoutedEvent SecondClickEvent = EventManager.RegisterRoutedEvent(
        nameof(SecondClick),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(SideMenu));

    public event RoutedEventHandler SecondClick
    {
        add => AddHandler(SecondClickEvent, value);
        remove => RemoveHandler(SecondClickEvent, value);
    }

    private void Second_OnClick(object? sender, RoutedEventArgs e)
    {
        First.IsEnabled = true;
        Second.IsEnabled = false;
        Third.IsEnabled = true;
        RaiseEvent(new RoutedEventArgs(SecondClickEvent));
    }

    public static readonly RoutedEvent ThirdClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ThirdClick),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(SideMenu));

    public event RoutedEventHandler ThirdClick
    {
        add => AddHandler(ThirdClickEvent, value);
        remove => RemoveHandler(ThirdClickEvent, value);
    }

    private void Third_OnClick(object? sender, RoutedEventArgs e)
    {
        First.IsEnabled = true;
        Second.IsEnabled = true;
        Third.IsEnabled = false;
        RaiseEvent(new RoutedEventArgs(ThirdClickEvent));
    }

    #endregion

    private EventSource? Source { get; }
    private string Email { get; set; } = string.Empty;

    public SideMenu()
    {
        InitializeComponent();
    }

    public SideMenu(EventSource source)
    {
        InitializeComponent();
        Source = source;
        source.LoginCompleted += Login_OnCompleted;
    }

    #region Event Subscription

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
        FullName.Text = DatabaseAccess.GetPersonNameBy(e.Email.Substring(4, 10));
        CurrentDate.Text = DateTime.Today.ToString("D", CultureInfo.GetCultureInfo("ru-RU"));
    }

    #endregion
}