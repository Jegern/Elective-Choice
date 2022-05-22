﻿using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels;

namespace Elective_Choice.Views;

public partial class Login
{
    public Login(EventSource store)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(store);
    }
}