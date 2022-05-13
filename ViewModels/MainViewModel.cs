﻿using System.Windows;
using System.Windows.Controls;
using Elective_Choice.Views;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;
using Npgsql;

namespace Elective_Choice.ViewModels;

public class MainViewModel : ViewModel
{
    #region Fields

    private new static ViewModelStore Store { get; } = new();
    private Page _frameContent = new Login(Store);
    private ResizeMode _resizeMode = ResizeMode.CanMinimize;

    public Page FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }

    public ResizeMode ResizeMode
    {
        get => _resizeMode;
        set => Set(ref _resizeMode, value);
    }

    #endregion

    public MainViewModel()
    {
        Store.SuccessfulLogin += SuccessfulLogin_OnChanged;
    }

    private void SuccessfulLogin_OnChanged(string email, bool rights)
    {
        FrameContent = rights ? new Admin(Store) : new Student(Store);
        ResizeMode = ResizeMode.CanResize;
        Store.TriggerLoginCompleteEvent(email);
    }
}