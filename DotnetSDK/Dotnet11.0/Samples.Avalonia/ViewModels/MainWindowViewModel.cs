using CommunityToolkit.Mvvm.ComponentModel;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private object? currentViewModel;
}