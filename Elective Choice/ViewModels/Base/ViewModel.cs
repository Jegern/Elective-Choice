﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels.Base;

public abstract class ViewModel : INotifyPropertyChanged
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
    
    protected ViewModelStore? Store { get; }

    protected ViewModel()
    {
        
    }

    protected ViewModel(ViewModelStore? store)
    {
        Store = store;
    }
}