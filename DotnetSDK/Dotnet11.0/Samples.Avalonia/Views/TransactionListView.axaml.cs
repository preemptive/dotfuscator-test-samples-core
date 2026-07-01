using Avalonia.Controls;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Views;

public partial class TransactionListView : UserControl
{
    public TransactionListView()
    {
        InitializeComponent();
    }

    public TransactionListView(TransactionListViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}