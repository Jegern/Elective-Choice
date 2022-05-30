using System.Windows;
using Elective_Choice.Infrastructure.Commands.Base;
using Elective_Choice.Infrastructure.EventArgs;
using Elective_Choice.Infrastructure.EventSource;
using Elective_Choice.ViewModels.Base;

namespace Elective_Choice.ViewModels;

public class LoginViewModel : ViewModel
{
    #region Fields

    private string _email = "stud0000211632@study.utmn.ru";
    private string _password = "string.Empty";

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

    #endregion

    public LoginViewModel()
    {
    }

    public LoginViewModel(EventSource source) : base(source)
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
        if (DatabaseAccess.PersonIsStudent(Email.Substring(4, 10)))
            Source?.RaiseLoginSucceed(this, new LoginEventArgs(Email, false));
        else if (DatabaseAccess.PersonIsAdmin(Email.Substring(4, 10)))
            Source?.RaiseLoginSucceed(this, new LoginEventArgs(Email, true));
        else
            MessageBox.Show(
                "Такого пользователя не существует", 
                "Ошибка!", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
    }

    #endregion

    #region PasswordForgottenCommand

    public Command? PasswordForgottenCommand { get; }

    private bool PasswordForgottenCommand_CanExecute(object? parameter) => true;

    private void PasswordForgottenCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #endregion
}