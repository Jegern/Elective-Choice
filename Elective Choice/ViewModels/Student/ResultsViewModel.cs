using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.Models;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views.Student;


namespace Elective_Choice.ViewModels.Student;

public class ResultsViewModel: ViewModel
{
    public string Email { get; private set; } = string.Empty;
    public List<List<Elective>> Electives { get; private set; } = new List<List<Elective>>();

    public string _firstElectiveName = string.Empty;
    public string _secondElectiveName = string.Empty;
    public string _firstElectivePrioirity = string.Empty;
    public string _secondElectivePrioirity = string.Empty;

    public string FirstElectiveName
    {
        get => _firstElectiveName;
        set => Set(ref _firstElectiveName, value);
    }
    public string SecondElectiveName
    {
        get => _secondElectiveName;
        set => Set(ref _secondElectiveName, value);
    }
    public string FirstElectivePrioirity
    {
        get => _firstElectivePrioirity;
        set => Set(ref _firstElectivePrioirity, value);
    }
    public string SecondElectivePrioirity
    {
        get => _secondElectivePrioirity;
        set => Set(ref _secondElectivePrioirity, value);
    }
    public ResultsViewModel()
    {
    }


    public ResultsViewModel(EventSource source, string email) : base(source)
    {
        source.ResultsLoaded += Results_OnLoaded;
        Email = email;
        Electives = DatabaseAccess.GetStudentResultElectives(Email.Substring(4, 10));
        var bib = 0;
    }

    private void Results_OnLoaded(object? sender, ResultsEventArgs e)
    {
        Email = e.Email;
    }

    private void Login_OnCompleted(object? sender, LoginEventArgs e)
    {
        Email = e.Email;
    }
}