using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Elective_Choice.Infrastructure.EventSource;

namespace Elective_Choice.ViewModels.Base;

public abstract class ViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected EventSource? Source { get; }

    protected ViewModel()
    {
    }

    protected ViewModel(EventSource? source)
    {
        Source = source;
    }


    ~ViewModel() => Dispose(false);

    public void Dispose() => Dispose(true);

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            // Dispose managed state (managed objects)
        }
        // Free unmanaged resources (unmanaged objects) and override finalizer
        // Set large fields to null
        _disposed = true;
    }
}