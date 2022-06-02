using System.Windows;
using System.Windows.Input;

namespace Elective_Choice.Views.Styles.Priorities;

public partial class LowerCard
{
    #region Dependency Properties

    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(default(string)));

    public new string Background
    {
        get => (string)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        nameof(BorderBrush),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(default(string)));

    public new string BorderBrush
    {
        get => (string)GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public new static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        nameof(IsEnabled),
        typeof(bool),
        typeof(LowerCard),
        new PropertyMetadata(false));

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(
        nameof(ContentVisibility),
        typeof(Visibility),
        typeof(LowerCard),
        new PropertyMetadata(Visibility.Visible));

    public Visibility ContentVisibility
    {
        get => (Visibility)GetValue(ContentVisibilityProperty);
        set => SetValue(ContentVisibilityProperty, value);
    }

    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        nameof(DisplayName),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(string.Empty));

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public static readonly DependencyProperty RealNameProperty = DependencyProperty.Register(
        nameof(RealName),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(string.Empty));

    public string RealName
    {
        get => (string)GetValue(RealNameProperty);
        set => SetValue(RealNameProperty, value);
    }

    public static readonly DependencyProperty DisplayDayProperty = DependencyProperty.Register(
        nameof(DisplayDay),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(string.Empty));

    public string DisplayDay
    {
        get => (string) GetValue(DisplayDayProperty);
        set => SetValue(DisplayDayProperty, value);
    }

    public static readonly DependencyProperty RealDayProperty = DependencyProperty.Register(
        nameof(RealDay),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(string.Empty));

    public string RealDay
    {
        get => (string) GetValue(RealDayProperty);
        set => SetValue(RealDayProperty, value);
    }

    #endregion

    public LowerCard()
    {
        InitializeComponent();
    }

    #region Events

    private void Image_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        Root.IsEnabled = false;
        var data = new DataObject(DataFormats.Serializable, Root);
        if (DragDrop.DoDragDrop(this, data, DragDropEffects.Move) == DragDropEffects.None)
            Root.IsEnabled = true;
    }

    #endregion
}