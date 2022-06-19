using System.Windows.Controls;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views.Admin;

public partial class ProblemElectives
{
    private bool _isManualEditCommit;

    public ProblemElectives(EventSource source)
    {
        InitializeComponent();
        DataContext = new ProblemElectivesViewModel(source);
    }

    private void ProblemDataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isManualEditCommit) return;
        _isManualEditCommit = true;
        ((DataGrid)sender!).CommitEdit(DataGridEditingUnit.Row, true);
        _isManualEditCommit = false;

        SendProblemElectiveToResolved(sender, e);
    }

    private void SendProblemElectiveToResolved(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Row.Item is not Elective elective || sender is not DataGrid dataGrid) return;
        if (Validation.GetHasError(dataGrid.ItemContainerGenerator.ContainerFromItem(elective))) return;

        switch (elective.Problem)
        {
            case "Incomplete":
                ((ProblemElectivesViewModel)DataContext).Incomplete?.Remove(elective);
                break;
            case "Overflowed":
                ((ProblemElectivesViewModel)DataContext).Overflowed?.Remove(elective);
                break;
        }

        ((ProblemElectivesViewModel)DataContext).Resolved?.Add(elective);       
        DatabaseAccess.UpdateElectiveCapacity(elective);
    }

    private void ResolvedDataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (_isManualEditCommit) return;
        _isManualEditCommit = true;
        ((DataGrid)sender!).CommitEdit(DataGridEditingUnit.Row, true);
        _isManualEditCommit = false;

        ChangeResolvedElectiveCapacity(sender, e);
    }

    private static void ChangeResolvedElectiveCapacity(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Row.Item is not Elective elective || sender is not DataGrid dataGrid) return;
        if (Validation.GetHasError(dataGrid.ItemContainerGenerator.ContainerFromItem(elective))) return;
        DatabaseAccess.UpdateElectiveCapacity(elective);
    }
}