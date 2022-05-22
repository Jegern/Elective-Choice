using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using SkiaSharp;

namespace Elective_Choice.ViewModels;

public class StatisticsViewModel : ViewModel
{
    #region Fields

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }
    
    public List<ISeries> Series { get; set; } = new()
    {
        new StackedColumnSeries<int>
        {
            Values = new List<int>(),
            Name = "Диапазон [3.0; 3.5)",
            Fill = new SolidColorPaint(new SKColor(255, 51, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int>(),
            Name = "Диапазон [3.5; 4.0)",
            Fill = new SolidColorPaint(new SKColor(255, 102, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int>(),
            Name = "Диапазон [4.0; 4.5)",
            Fill = new SolidColorPaint(new SKColor(255, 153, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int>(),
            Name = "Диапазон [4.5; 5)",
            Fill = new SolidColorPaint(new SKColor(255, 204, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int>(),
            Name = "Диапазон [5.0; 5.0]",
            Fill = new SolidColorPaint(new SKColor(255, 255, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        }
    };

    private static string TooltipLabelFormatter(ChartPoint<int, RoundedRectangleGeometry, LabelGeometry> point)
    {
        var chart = (CartesianChart) point.Context.Chart;
        var totalSelected = 0d;
        for (var i = 0; i < 5; i++)
            totalSelected += ((int[]) chart.Series.ElementAt(i).Values!).Sum();
        return $"{Math.Round(100 * point.PrimaryValue / totalSelected, 2)}%";
    }

    public List<Axis> XAxes { get; set; } = new()
    {
        new Axis
        {
            Labels = new[] {"1 приоритет", "2 приоритет", "3 приоритет", "4 приоритет", "5 приоритет"},
            LabelsRotation = 15
        }
    };

    public List<Axis> YAxes { get; set; } = new()
    {
        new Axis
        {
            TextSize = 0
        }
    };

    #endregion

    public StatisticsViewModel()
    {
    }

    public StatisticsViewModel(EventSource source) : base(source)
    {
        source.StatisticsLoaded += Statistics_OnLoaded;
        // WeakEventManager<EventSource, StatisticsEventArgs>.AddHandler(
        //     source, 
        //     nameof(EventSource.StatisticsLoaded), 
        //     Statistics_OnLoaded);

        BackToListCommand = new Command(
            BackToListCommand_OnExecuted,
            BackToListCommand_CanExecute);
    }
    
    #region Event Subscription

    private void Statistics_OnLoaded(object? sender, StatisticsEventArgs e)
    {
        // TODO: Исправить множественную подписку на событие
        Name = e.Name;

        if (e.Year is null || e.Spring is null)
            FillSeriesWith(DatabaseAccess.GetElectiveStatistics(e.Name));
        else
            FillSeriesWith(DatabaseAccess.GetElectiveStatistics(e.Name, (int) e.Year, (bool) e.Spring));
    }

    private void FillSeriesWith(IReadOnlyList<int[]> values)
    {
        for (var i = 0; i < Series.Count; i++)
            Series[i].Values = values[i];
    }

    #endregion

    #region BackToListCommand

    public Command? BackToListCommand { get; }

    private bool BackToListCommand_CanExecute(object? parameter) => Name != string.Empty;

    private void BackToListCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsClosed();
    }

    #endregion
}