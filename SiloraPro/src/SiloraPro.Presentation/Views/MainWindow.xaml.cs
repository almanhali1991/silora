using System.Windows;
using SiloraPro.Presentation.ViewModels;

namespace SiloraPro.Presentation.Views;

/// <summary>
/// النافذة الرئيسية للتطبيق
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
