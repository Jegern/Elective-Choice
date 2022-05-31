using System;
using System.Windows.Controls;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views;

public partial class ProblemElectives
{
    private Elective? EditedElective { get; set; }

    public ProblemElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new ProblemElectivesViewModel(source);
    }

    private void DataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        EditedElective = e.Row.Item as Elective;
    }

    private void ProblemDataGrid_OnCurrentCellChanged(object? sender, EventArgs e)
    {
        if (EditedElective is null) return;

        switch (EditedElective.Problem)
        {
            case "Incomplete":
                ((ProblemElectivesViewModel)DataContext).Incomplete?.Remove(EditedElective);
                break;
            case "Overflowed":
                ((ProblemElectivesViewModel)DataContext).Overflowed?.Remove(EditedElective);
                break;
        }

        ((ProblemElectivesViewModel)DataContext).Resolved?.Add(EditedElective);
        // DatabaseAccess.UpdateElectiveCapacity(EditedElective);
        EditedElective = null;
    }

    private void ResolvedDataGrid_OnCurrentCellChanged(object? sender, EventArgs e)
    {
        // TODO: Исправить проблему, при которой событие не запускается, если электив находится последним в списке 
        if (EditedElective is null) return;
        // DatabaseAccess.UpdateElectiveCapacity(EditedElective);
        EditedElective = null;
    }
}