using System;
using System.Windows;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Admin;

namespace Elective_Choice.Views.Admin;

public partial class AlgorithmSettings
{
    public AlgorithmSettings(EventSource source)
    {
        InitializeComponent();
        DataContext = new AlgorithmSettingsViewModel(source);
    }

    private void StartChoices_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (StartChoices.Value is null || EndChoices.Value is null) return;
        if (DateTime.Compare((DateTime) StartChoices.Value, (DateTime) EndChoices.Value) > 0)
            StartChoices.Value = null;
        
        if (StartChoices.Value is null || StartAlgorithm.Value is null) return;
        if (DateTime.Compare((DateTime) StartChoices.Value, (DateTime) StartAlgorithm.Value) > 0)
            StartChoices.Value = null;
    }
    
    private void EndChoices_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (EndChoices.Value is null || StartChoices.Value is null) return;
        if (DateTime.Compare((DateTime) EndChoices.Value, (DateTime) StartChoices.Value) <= 0)
            EndChoices.Value = null;
        
        if (EndChoices.Value is null || StartAlgorithm.Value is null) return;
        if (DateTime.Compare((DateTime) EndChoices.Value, (DateTime) StartAlgorithm.Value) > 0)
            EndChoices.Value = null;
    }

    private void StartAlgorithm_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (StartAlgorithm.Value is null || StartChoices.Value is null) return;
        if (DateTime.Compare((DateTime) StartAlgorithm.Value, (DateTime) StartChoices.Value) <= 0)
            StartAlgorithm.Value = null;
        
        if (StartAlgorithm.Value is null || EndChoices.Value is null) return;
        if (DateTime.Compare((DateTime) StartAlgorithm.Value, (DateTime) EndChoices.Value) <= 0)
            StartAlgorithm.Value = null;
    }

}