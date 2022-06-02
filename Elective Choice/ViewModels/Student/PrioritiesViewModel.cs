using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views.Styles.Priorities;

namespace Elective_Choice.ViewModels.Student;

public class PrioritiesViewModel : ViewModel
{
    #region Fields

    private string Email { get; } = string.Empty;

    private Dictionary<int, string[]> DaysOfWeek { get; } = new()
    {
        {1, new[] {"Пн", "Понедельник"}},
        {2, new[] {"Вт", "Вторник"}},
        {3, new[] {"Ср", "Среда"}},
        {4, new[] {"Чт", "Четверг"}},
        {5, new[] {"Пт", "Пятница"}},
        {6, new[] {"Сб", "Суббота"}},
        {7, new[] {"Вс", "Воскресенье"}}
    };

    private List<Elective>? Electives { get; }

    public UniformGrid UpperCardGrid { get; } = new() {Columns = 1};

    public UniformGrid LowerCardGrid { get; } = new() {Rows = 1};

    #endregion

    #region Constructor

    public PrioritiesViewModel()
    {
    }

    public PrioritiesViewModel(EventSource source, string email) : base(source)
    {
        source.PrioritiesClosing += Priorities_OnClosing;

        Email = email;
        Electives = DatabaseAccess.GetStudentElectives(email.Substring(4, 10));

        InitializeCardGrids();
        FillInCardsWithElectives();
    }

    private void InitializeCardGrids()
    {
        for (var i = 0; i < 5; i++)
        {
            UpperCardGrid.Children.Add(new UpperCard());
            LowerCardGrid.Children.Add(new LowerCard());
        }
    }

    private void FillInCardsWithElectives()
    {
        for (int i = 0, j = 0; i < 5; i++)
        {
            if (Electives!.Count <= i) continue;
            if (Electives[i].Priority == 0)
            {
                var card = (LowerCard) LowerCardGrid.Children[j];
                card.RealName = card.DisplayName = Electives[i].Name;
                card.RealDay = card.DisplayDay = DaysOfWeek[Electives[i].Day][0];
                card.IsEnabled = true;
                j++;
            }
            else
            {
                var card = (UpperCard) UpperCardGrid.Children[Electives[i].Priority - 1];
                card.RealName = card.DisplayName = Electives[i].Name;
                card.RealDay = card.DisplayDay = DaysOfWeek[Electives[i].Day][1];
                card.IsEnabled = true;
            }
        }
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && Source is not null)
            {
                Source.PrioritiesClosing -= Priorities_OnClosing;
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion

    #region Event Subscription

    private void Priorities_OnClosing(object? sender, EventArgs e)
    {
        for (var i = 0; i < 5; i++)
        {
            var upperCard = (UpperCard) UpperCardGrid.Children[i];
            if (upperCard.RealName != string.Empty)
                DatabaseAccess.UpdateStudentElectivePriority(Email.Substring(4, 10), upperCard.RealName, i + 1);
            var lowerCard = (LowerCard) LowerCardGrid.Children[i];
            if (lowerCard.RealName != string.Empty)
                DatabaseAccess.UpdateStudentElectivePriority(Email.Substring(4, 10), lowerCard.RealName, 0);
        }
    }

    #endregion
}