namespace SiloraPro.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private object? _currentView;

    public object? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public MainViewModel()
    {
        // يمكن تعيين العرض الافتراضي هنا
    }
}
