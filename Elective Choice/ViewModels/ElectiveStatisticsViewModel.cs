using System;
using System.Collections.Generic;
using System.Linq;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using Npgsql;
using SkiaSharp;

namespace Elective_Choice.ViewModels;

public class ElectiveStatisticsViewModel : ViewModel
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
            totalSelected += ((List<int>) chart.Series.ElementAt(i).Values!).Sum();
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

    public ElectiveStatisticsViewModel()
    {
    }

    public ElectiveStatisticsViewModel(ViewModelStore store) : base(store)
    {
        store.ElectiveStatisticsLoaded += ElectiveStatisticsLoaded_OnChanged;
    }

    private void ElectiveStatisticsLoaded_OnChanged(string name, int year, string season)
    {
        Name = name;

        Store?.SqlConnection.Open();

        for (double i = 0d, perf = 3d; perf <= 5d; i++, perf += 0.5)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT selected_electives.priority, Count(*) AS number_of_elections
                          FROM selected_electives
                              JOIN electives ON selected_electives.electiveID = electives.id
                              JOIN old_students ON selected_electives.studentID = old_students.id
                          WHERE year = {year} AND semester = '{season}' AND electives.name = '{name}'
                              AND old_students.performance >= {perf} AND old_students.performance < {perf + 0.5}
                          GROUP BY selected_electives.priority", Store?.SqlConnection).ExecuteReader();
            for (var priority = 1; priority <= 5; priority++)
            {
                if (reader.Read() && reader.GetInt32(0) == priority)
                    ((List<int>?) Series[(int) i].Values)?.Add(reader.GetInt32(1));
                else
                    ((List<int>?) Series[(int) i].Values)?.Add(0);
            }

            reader.Close();
        }

        Store?.SqlConnection.Close();
    }
}