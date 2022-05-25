using System;
using System.Collections.Generic;
using System.Linq;
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

    public static List<ISeries> Series { get; set; } = new()
    {
        new StackedColumnSeries<int>
        {
            Values = new int[5],
            Name = "Диапазон [3.0; 4.0)",
            Fill = new SolidColorPaint(new SKColor(255, 70, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new int[5],
            Name = "Диапазон [4.0; 4.75)",
            Fill = new SolidColorPaint(new SKColor(255, 155, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new StackedColumnSeries<int>
        {
            Values = new int[5],
            Name = "Диапазон [4.75; 5]",
            Fill = new SolidColorPaint(new SKColor(255, 230, 0)),
            TooltipLabelFormatter = TooltipLabelFormatter
        },
        new ColumnSeries<int>
        {
            Values = new int[5],
            Name = "Все диапазоны",
            Fill = new SolidColorPaint(new SKColor(200, 200, 200)),
            IgnoresBarPosition = true,
            ZIndex = -1,
            TooltipLabelFormatter = TooltipLabelFormatter
        }
    };

    private static string TooltipLabelFormatter(ChartPoint<int, RoundedRectangleGeometry, LabelGeometry> point)
    {
        var chart = (CartesianChart) point.Context.Chart;
        var totalSelected = 0d;
        for (var i = 0; i < Series.Count - 1; i++)
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

    private static void FillSeriesWith(IReadOnlyList<int[]> values)
    {
        for (var i = 0; i < Series.Count; i++)
            Series[i].Values = values[i];
        for (var i = 0; i < 5; i++)
            ((int[]) Series.ElementAt(^1).Values!)[i] = values[0][i] +
                                                        values[1][i] +
                                                        values[2][i] +
                                                        values[3][i] +
                                                        values[4][i];
    }

    #endregion

    #region BackToListCommand

    public Command? BackToListCommand { get; }

    private bool BackToListCommand_CanExecute(object? parameter) => Name != string.Empty;

    private void BackToListCommand_OnExecuted(object? parameter)
    {
        Source?.RaiseStatisticsClosing(this, new StatisticsEventArgs(Name));
    }

    #endregion
}