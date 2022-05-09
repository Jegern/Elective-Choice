using System.Windows.Controls;
using Elective_Choice.ViewModels.Base;
using Elective_Choice.Views;

namespace Elective_Choice.ViewModels;

public class MainWindowViewModel :ViewModel
{
    private Page? _frameContent = new LoginForm();

    public Page? FrameContent
    {
        get => _frameContent;
        set => Set(ref _frameContent, value);
    }
}