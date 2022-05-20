using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Elective_Choice.Commands.Base;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.ViewModels.Store;

namespace Elective_Choice.ViewModels;

public class LoginViewModel : ViewModel
{
    #region Fields

    private string _email = "stud0000123456@study.utmn.ru";
    private string _password = "string.Empty";

    private ImageSource _lockSource =
        new BitmapImage(
            new Uri(@"pack://application:,,,/Elective Choice;component/Views/Styles/Icons/Lock (Locked).png"));

    private bool Rights { get; set; }

    public string Email
    {
        get => _email;
        set => Set(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => Set(ref _password, value);
    }

    public ImageSource LockSource
    {
        get => _lockSource;
        set => Set(ref _lockSource, value);
    }

    #endregion

    public LoginViewModel()
    {
        
    }

    public LoginViewModel(ViewModelStore store) : base(store)
    {
        SignInCommand = new Command(
            SignInCommand_OnExecute,
            SignInCommand_CanExecute);
        PasswordForgottenCommand = new Command(
            PasswordForgottenCommand_OnExecute,
            PasswordForgottenCommand_CanExecute);
    }

    #region Commands

    #region SignInCommand

    public Command? SignInCommand { get; }

    private bool SignInCommand_CanExecute(object? parameter) => 
        Email.Length > 0 &&
        Password.Length > 0;

    private void SignInCommand_OnExecute(object? parameter)
    {
        if (CheckUserData())
            Store?.TriggerSuccessfulLogin(Email, Rights);
    }

    private bool CheckUserData()
    {
        // TODO: Реализовать обращение к БД и проверку электронной почты и пароля
        return true;
    }

    #endregion

    #region PasswordForgottenCommand

    public Command? PasswordForgottenCommand { get; }

    private bool PasswordForgottenCommand_CanExecute(object? parameter) => true;

    private void PasswordForgottenCommand_OnExecute(object? parameter)
    {
        Rights = !Rights;
        LockSource = Rights
            ? new BitmapImage(
                new Uri("pack://application:,,,/Elective Choice;component/Views/Styles/Icons/Lock (Unlocked).png"))
            : new BitmapImage(
                new Uri("pack://application:,,,/Elective Choice;component/Views/Styles/Icons/Lock (Locked).png"));
    }

    #endregion

    #endregion
}