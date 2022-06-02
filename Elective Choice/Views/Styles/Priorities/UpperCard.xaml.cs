using System.Collections.Generic;
using System.Linq;
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

    public new static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        nameof(IsEnabled),
        typeof(bool),
        typeof(UpperCard),
        new PropertyMetadata(false));

    public new bool IsEnabled
    {
        get => (bool) GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
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

    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        nameof(DisplayName),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string DisplayName
    {
        get => (string) GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public static readonly DependencyProperty RealNameProperty = DependencyProperty.Register(
        nameof(RealName),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string RealName
    {
        get => (string) GetValue(RealNameProperty);
        set => SetValue(RealNameProperty, value);
    }

    public static readonly DependencyProperty DisplayDayProperty = DependencyProperty.Register(
        nameof(DisplayDay),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string DisplayDay
    {
        get => (string) GetValue(DisplayDayProperty);
        set => SetValue(DisplayDayProperty, value);
    }

    public static readonly DependencyProperty RealDayProperty = DependencyProperty.Register(
        nameof(RealDay),
        typeof(string),
        typeof(UpperCard),
        new PropertyMetadata(string.Empty));

    public string RealDay
    {
        get => (string) GetValue(RealDayProperty);
        set => SetValue(RealDayProperty, value);
    }

    #endregion

    public UpperCard()
    {
        InitializeComponent();
    }

    private static Dictionary<string, string> DaysOfWeek { get; } = new()
    {
        {"Пн", "Понедельник"},
        {"Вт", "Вторник"},
        {"Ср", "Среда"},
        {"Чт", "Четверг"},
        {"Пт", "Пятница"},
        {"Сб", "Суббота"},
        {"Вс", "Воскресенье"}
    };

    #region Events

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
                FromLowerCard_OnDragEnter(source, target);
                break;
            case UpperCard source:
                FromUpperCard_OnDragEnter(source, target);
                break;
        }
    }

    private static void FromLowerCard_OnDragEnter(LowerCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.DisplayName = source.RealName;
            target.DisplayDay = DaysOfWeek[source.RealDay];
            target.IsEnabled = true;
        }
        else
        {
            var grid = (UniformGrid) target.Parent;
            var nameBuffer = source.RealName;
            var dayBuffer = DaysOfWeek[source.RealDay];
            for (var i = grid.Children.IndexOf(target); i < grid.Children.Count; i++)
            {
                var card = (UpperCard) grid.Children[i];
                card.DisplayName = nameBuffer;
                card.DisplayDay = dayBuffer;
                nameBuffer = card.RealName;
                dayBuffer = card.RealDay;
                card.IsEnabled = true;
                if (nameBuffer == string.Empty) break;
            }

            if (nameBuffer == string.Empty) return;
            source.DisplayName = nameBuffer;
            source.DisplayDay = DaysOfWeek.First(x=> x.Value == dayBuffer).Key;
            source.IsEnabled = true;
        }
    }

    private static void FromUpperCard_OnDragEnter(UpperCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.DisplayName = source.RealName;
            target.DisplayDay = source.RealName;
            target.IsEnabled = true;
        }
        else
        {
            target.DisplayName = source.RealName;
            source.DisplayName = target.RealName;
            target.DisplayDay = source.RealDay;
            source.DisplayDay = target.RealDay;
            source.IsEnabled = true;
        }
    }

    private void Border_OnDragLeave(object sender, DragEventArgs e)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        var target = Root;
        switch (data)
        {
            case LowerCard source:
                FromLowerCard_OnDragLeave(source, target);
                break;
            case UpperCard source:
                FromUpperCard_OnDragLeave(source, target);
                break;
        }
    }

    private static void FromLowerCard_OnDragLeave(LowerCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.DisplayName = target.RealName;
            target.DisplayDay = target.RealDay;
            target.IsEnabled = false;
        }
        else
        {
            var grid = (UniformGrid) target.Parent;
            for (var i = grid.Children.IndexOf(target); i < grid.Children.Count; i++)
            {
                var card = (UpperCard) grid.Children[i];
                card.DisplayName = card.RealName;
                card.DisplayDay = card.RealDay;
                if (card.RealName != string.Empty) continue;
                card.IsEnabled = false;
                break;
            }

            if (source.DisplayName == source.RealName) return;
            source.DisplayName = source.RealName;
            source.DisplayDay = source.RealDay;
            source.IsEnabled = false;
        }
    }

    private static void FromUpperCard_OnDragLeave(UpperCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.DisplayName = target.RealName;
            target.DisplayDay = target.RealDay;
            target.IsEnabled = false;
        }
        else
        {
            target.DisplayName = target.RealName;
            target.DisplayDay = target.RealDay;
            source.DisplayName = source.RealName;
            source.DisplayDay = source.RealDay;
            source.IsEnabled = false;
        }
    }

    private void Border_OnDrop(object sender, DragEventArgs e)
    {
        var data = e.Data.GetData(DataFormats.Serializable);
        var target = Root;
        switch (data)
        {
            case LowerCard source:
                FromLowerCard_OnDrop(source, target);
                break;
            case UpperCard source:
                FromUpperCard_OnDrop(source, target);
                break;
        }
    }

    private static void FromLowerCard_OnDrop(LowerCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.RealName = source.RealName;
            target.RealDay = DaysOfWeek[source.RealDay];
            source.RealName = source.DisplayName = string.Empty;
            source.RealDay = source.DisplayDay = string.Empty;
        }
        else
        {
            var grid = (UniformGrid) target.Parent;
            var index = grid.Children.IndexOf(target);
            if (index < 4)
            {
                for (var i = index; i < grid.Children.Count; i++)
                {
                    var card = (UpperCard) grid.Children[i];
                    card.RealName = card.DisplayName;
                    card.RealDay = card.DisplayDay;
                    if (!card.IsEnabled) break;
                }
                
                if (source.DisplayName == source.RealName) return;
                source.RealName = source.DisplayName;
                source.RealDay = source.DisplayDay;
            }
            else
            {
                (source.RealName, target.RealName) = (target.RealName, source.RealName);
                (source.RealDay, target.RealDay) =
                    (DaysOfWeek.First(x => x.Value == target.RealDay).Key, DaysOfWeek[source.RealDay]);
                source.DisplayName = source.RealName;
                source.DisplayDay = source.RealDay;
                source.IsEnabled = true;
            }
        }
    }

    private static void FromUpperCard_OnDrop(UpperCard source, UpperCard target)
    {
        if (target.RealName == string.Empty)
        {
            target.RealName = source.RealName;
            target.RealDay = source.RealDay;
            source.RealName = source.DisplayName = string.Empty;
            source.RealDay = source.DisplayDay = string.Empty;
        }
        else
        {
            (source.RealName, target.RealName) = (target.RealName, source.RealName);
            (source.RealDay, target.RealDay) = (target.RealDay, source.RealDay);
            source.DisplayName = source.RealName;
            source.DisplayDay = source.RealDay;
        }
    }

    #endregion
}