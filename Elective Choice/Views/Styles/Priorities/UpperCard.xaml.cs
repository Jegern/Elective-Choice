using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Elective_Choice.Views.Styles.Priorities;

public partial class UpperCard
{
    #region Dependency Properties

    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(default(string)));

    public new string Background
    {
        get => (string)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        nameof(BorderBrush),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(default(string)));

    public new string BorderBrush
    {
        get => (string)GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public new static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        nameof(IsEnabled),
        typeof(bool),
        typeof(UpperCard),
        new PropertyMetadata(false));

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(
        nameof(ContentVisibility),
        typeof(Visibility),
        typeof(UpperCard),
        new PropertyMetadata(Visibility.Visible));

    public Visibility ContentVisibility
    {
        get => (Visibility)GetValue(ContentVisibilityProperty);
        set => SetValue(ContentVisibilityProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ElectiveNameProperty = DependencyProperty.Register(
        nameof(ElectiveName),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string ElectiveName
    {
        get => (string)GetValue(ElectiveNameProperty);
        set => SetValue(ElectiveNameProperty, value);
    }

    #endregion

    public UpperCard()
    {
        InitializeComponent();
    }

    private void Image_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        Root.IsEnabled = false;
        Root.IsHitTestVisible = false;
        var data = new DataObject(DataFormats.Serializable, Root);
        if (DragDrop.DoDragDrop(this, data, DragDropEffects.Move) == DragDropEffects.None)
            Root.IsEnabled = true;
        Root.IsHitTestVisible = true;
    }

    private void Border_OnDragEnter(object sender, DragEventArgs e)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        var target = Root;
        switch (data)
        {
            case LowerCard source:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.Text = source.ElectiveName;
                    target.IsEnabled = true;
                }
                else
                {
                    var grid = (UniformGrid)target.Parent;
                    var nameBuffer = source.ElectiveName;
                    for (var i = grid.Children.IndexOf(target); i < grid.Children.Count; i++)
                    {
                        var card = (UpperCard)grid.Children[i];
                        card.Text = nameBuffer;
                        nameBuffer = card.ElectiveName;
                        card.IsEnabled = true;
                        if (nameBuffer == string.Empty) break;
                    }
                }

                break;
            }
            case UpperCard source:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.Text = source.ElectiveName;
                    target.IsEnabled = true;
                }
                else
                {
                    target.Text = source.ElectiveName;
                    source.Text = target.ElectiveName;
                    source.IsEnabled = true;
                }

                break;
            }
        }
    }

    private void Border_OnDragLeave(object sender, DragEventArgs e)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        var target = Root;
        switch (data)
        {
            case LowerCard:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.Text = target.ElectiveName;
                    target.IsEnabled = false;
                }
                else
                {
                    var grid = (UniformGrid)target.Parent;
                    for (var i = grid.Children.IndexOf(target); i < grid.Children.Count; i++)
                    {
                        var card = (UpperCard)grid.Children[i];
                        card.Text = card.ElectiveName;
                        if (card.ElectiveName != string.Empty) continue;
                        card.IsEnabled = false;
                        break;
                    }
                }

                break;
            }
            case UpperCard source:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.Text = target.ElectiveName;
                    target.IsEnabled = false;
                }
                else
                {
                    target.Text = target.ElectiveName;
                    source.IsEnabled = false;
                }

                break;
            }
        }
    }

    private void Border_OnDrop(object sender, DragEventArgs e)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        var target = Root;
        switch (data)
        {
            case LowerCard source:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.ElectiveName = source.ElectiveName;
                    source.Text = string.Empty;
                    source.ElectiveName = string.Empty;
                }
                else
                {
                    var grid = (UniformGrid)target.Parent;
                    for (var i = grid.Children.IndexOf(target); i < grid.Children.Count; i++)
                    {
                        var card = (UpperCard)grid.Children[i];
                        card.ElectiveName = card.Text;
                        if (!card.IsEnabled) break;
                    }
                }

                break;
            }
            case UpperCard source:
            {
                if (target.ElectiveName == string.Empty)
                {
                    target.ElectiveName = source.ElectiveName;
                    source.Text = string.Empty;
                    source.ElectiveName = string.Empty;
                }
                else
                {
                    (source.ElectiveName, target.ElectiveName) = (target.ElectiveName, source.ElectiveName);
                    source.Text = source.ElectiveName;
                }

                break;
            }
        }
    }
}