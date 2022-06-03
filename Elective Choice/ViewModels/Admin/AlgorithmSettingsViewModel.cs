using System;
using Elective_Choice.Infrastructure;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels.Admin;

public class AlgorithmSettingsViewModel : ViewModel
{
    #region Fields

    private DateTime? _startChoices;
    private DateTime? _endChoices;
    private DateTime? _startAlgorithm;

    public DateTime? StartChoices
    {
        get => _startChoices;
        set => Set(ref _startChoices, value);
    }

    public DateTime? EndChoices
    {
        get => _endChoices;
        set => Set(ref _endChoices, value);
    }

    public DateTime? StartAlgorithm
    {
        get => _startAlgorithm;
        set => Set(ref _startAlgorithm, value);
    }

    #endregion

    #region Constructor

    public AlgorithmSettingsViewModel()
    {
    }

    public AlgorithmSettingsViewModel(EventSource source) : base(source)
    {
        source.AlgorithmSettingClosing += AlgorithmSetting_OnClosing;
        
        var settings = DatabaseAccess.GetAlgorithmSettings();
        StartChoices = settings[0];
        EndChoices = settings[1];
        StartAlgorithm = settings[2];
        
        StartAlgorithmCommand = new Command(
            StartAlgorithmCommand_OnExecuted,
            StartAlgorithmCommand_CanExecute);
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && Source is not null)
            {
                Source.AlgorithmSettingClosing -= AlgorithmSetting_OnClosing;
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #region Event Subsription

    private void AlgorithmSetting_OnClosing(object? sender, EventArgs e)
    {
        DatabaseAccess.UpdateAlgorithmSettings(StartChoices, EndChoices, StartAlgorithm);
        Dispose();
    }

    #endregion

    #endregion

    #region StartAlgorithmCommand

    public Command? StartAlgorithmCommand { get; }

    private bool StartAlgorithmCommand_CanExecute(object? parameter) => true;

    private void StartAlgorithmCommand_OnExecuted(object? parameter)
    {
        // if (MessageBox.Show(
        //         "Вы уверены, что хотите запустить алгоритм?",
        //         "Предупреждение",
        //         MessageBoxButton.YesNo,
        //         MessageBoxImage.Warning) == MessageBoxResult.Yes) ;
        // TODO: Здесь должен быть запуск алгоритма
    }

    #endregion
}