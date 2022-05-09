using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Elective_Choice.ViewModels.Base;
using Laboratory_work_1.Commands.Base;

namespace Elective_Choice.ViewModels;

public class LoginFormViewModel : ViewModel
{
    #region Fields

    private string _username = string.Empty;
    private string _password = string.Empty;

    private ImageSource _lockSource =
        new BitmapImage(
            new Uri(@"C:\Users\sever\Documents\Programming\Elective Choice\Views\Styles\Icons\Lock (Locked).png"));

    private bool AdminRights { get; set; }

    public string Username
    {
        get => _username;
        set => Set(ref _username, value);
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

    public LoginFormViewModel()
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

    private bool SignInCommand_CanExecute(object? parameter) => Username.Length > 0 && Password.Length > 0;

    private void SignInCommand_OnExecute(object? parameter)
    {
    }

    #endregion

    #region PasswordForgottenCommand

    public Command? PasswordForgottenCommand { get; }

    private bool PasswordForgottenCommand_CanExecute(object? parameter) => true;

    private void PasswordForgottenCommand_OnExecute(object? parameter)
    {
        AdminRights = !AdminRights;
        LockSource = AdminRights
            ? new BitmapImage(
                new Uri("pack://application:,,,/Elective Choice;component/Views/Styles/Icons/Lock (Unlocked).png"))
            : new BitmapImage(
                new Uri("pack://application:,,,/Elective Choice;component/Views/Styles/Icons/Lock (Locked).png"));
    }

    #endregion

    #endregion
}