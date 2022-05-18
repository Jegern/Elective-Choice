﻿using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class StudentViewModel : ViewModel
{
    private string Email { get; set; } = string.Empty;

    public StudentViewModel()
    {
        
    }

    public StudentViewModel(ViewModelStore store) : base(store)
    {
        store.LoginCompleted += email => Email = email;
    }
}