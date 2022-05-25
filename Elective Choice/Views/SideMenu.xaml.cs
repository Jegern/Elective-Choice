using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Elective_Choice.Views;

public partial class SideMenu
{
    #region Dependency Properties

    public static readonly DependencyProperty FullNameProperty = DependencyProperty.Register(
        nameof(FullName), typeof(string), typeof(SideMenu), new PropertyMetadata(string.Empty));

    public string FullName
    {
        get => (string) GetValue(FullNameProperty);
        set => SetValue(FullNameProperty, value);
    }

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

    #region RoutedCommands

    public static readonly DependencyProperty FirstCommandProperty = DependencyProperty.Register(
        nameof(FirstCommand), 
        typeof(ICommand),
        typeof(SideMenu), 
        new PropertyMetadata(null));

    public ICommand? FirstCommand
    {
        get => (ICommand) GetValue(FirstCommandProperty);
        set => SetValue(FirstCommandProperty, value);
    }

    public static readonly DependencyProperty SecondCommandProperty = DependencyProperty.Register(
        nameof(SecondCommand), 
        typeof(ICommand),
        typeof(SideMenu), 
        new PropertyMetadata(null));

    public ICommand? SecondCommand
    {
        get => (ICommand) GetValue(SecondCommandProperty);
        set => SetValue(SecondCommandProperty, value);
    }

    public static readonly DependencyProperty ThirdCommandProperty = DependencyProperty.Register(
        nameof(ThirdCommand), 
        typeof(ICommand),
        typeof(SideMenu), 
        new PropertyMetadata(null));

    public ICommand? ThirdCommand
    {
        get => (ICommand) GetValue(ThirdCommandProperty);
        set => SetValue(ThirdCommandProperty, value);
    }

    #endregion

    public SideMenu()
    {
        InitializeComponent();
    }

    private void SideMenu_OnLoaded(object sender, RoutedEventArgs e)
    {
        CurrentDate.Text = DateTime.Today.ToString("D", CultureInfo.GetCultureInfo("ru-RU"));
    }

    private void First_OnClick(object sender, RoutedEventArgs e)
    {
        if (FirstCommand is null) return;
        FirstCommand.Execute(null);
        First.IsEnabled = false;
        Second.IsEnabled = true;
        Third.IsEnabled = true;
    }

    private void Second_OnClick(object sender, RoutedEventArgs e)
    {
        if (SecondCommand is null) return;
        SecondCommand.Execute(null);
        First.IsEnabled = true;
        Second.IsEnabled = false;
        Third.IsEnabled = true;
    }

    private void Third_OnClick(object sender, RoutedEventArgs e)
    {
        
        if (ThirdCommand is null) return;
        ThirdCommand.Execute(null);
        First.IsEnabled = true;
        Second.IsEnabled = true;
        Third.IsEnabled = false;
    }
}