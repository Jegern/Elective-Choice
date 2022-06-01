using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Elective_Choice.Views.Styles.Priorities;

public partial class LowerCard
{
    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(default(string)));

    public new string Background
    {
        get => (string) GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        nameof(BorderBrush),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(default(string)));

    public new string BorderBrush
    {
        get => (string) GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public new static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        nameof(IsEnabled),
        typeof(bool),
        typeof(LowerCard),
        new PropertyMetadata(true));

    public new bool IsEnabled
    {
        get => (bool) GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(
        nameof(ContentVisibility),
        typeof(Visibility),
        typeof(LowerCard),
        new PropertyMetadata(Visibility.Visible));

    public Visibility ContentVisibility
    {
        get => (Visibility) GetValue(ContentVisibilityProperty);
        set => SetValue(ContentVisibilityProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(LowerCard),
        new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public LowerCard()
    {
        InitializeComponent();
    }

    private void Image_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        DragDrop.DoDragDrop((Image) sender, (Image) sender, DragDropEffects.Move);
        IsEnabled = false;
    }

    private void Border_OnDrop(object sender, DragEventArgs e)
    {
    }

    private void Border_OnDragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.Serializable) is not Image) return;
        
    }
}