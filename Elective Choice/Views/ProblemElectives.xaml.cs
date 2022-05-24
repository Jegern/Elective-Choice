using System;
using System.Windows.Controls;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class ProblemElectives
{
    private ProblemElective? EditedProblemElective { get; set; }

    public ProblemElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new ProblemElectivesViewModel(source);
    }

    private void DataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        EditedProblemElective = e.Row.Item as ProblemElective;
    }

    private void DataGrid_OnCurrentCellChanged(object? sender, EventArgs e)
    {
        if (EditedProblemElective is null) return;
        switch (EditedProblemElective.Problem)
        {
            case "Incomplete":
                ((ProblemElectivesViewModel)DataContext).Incomplete?.Remove(EditedProblemElective);
                break;
            case "Overflowed":
                ((ProblemElectivesViewModel)DataContext).Overflowed?.Remove(EditedProblemElective);
                break;
        }
        
        ((ProblemElectivesViewModel)DataContext).Resolved?.Add(EditedProblemElective);
        EditedProblemElective = null;
    }
}