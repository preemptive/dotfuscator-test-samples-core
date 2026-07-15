using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Views;
using Avalonia.Platform;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp;

public partial class MainWindow : Window
{
    private readonly IWorkflowExecutor _workflowExecutor;
    private readonly DashboardViewModel _dashboardVm;
    private readonly TransactionListViewModel _transactionsVm;

    public MainWindow()
    {
        InitializeComponent();

        // Core workflow — resolved from Core ServiceManager
        _workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());

        // ViewModels — resolved from DI container (D)
        _dashboardVm = ServiceContainer.Provider.GetRequiredService<DashboardViewModel>();
        _transactionsVm = ServiceContainer.Provider.GetRequiredService<TransactionListViewModel>();

        this.FindControl<DashboardView>("DashboardView")!.DataContext = _dashboardVm;
        this.FindControl<TransactionListView>("TransactionListView")!.DataContext = _transactionsVm;

        this.FindControl<Button>("DashboardBtn")!.Click += (_, __) => ShowDashboard();
        this.FindControl<Button>("TransactionsBtn")!.Click += (_, __) => ShowTransactions();
        this.FindControl<Button>("RunCoreBtn")!.Click += async (_, __) =>
        {
            //ExecLogic();
            var steps = StepsContextFactory.Create(
                ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());

            await _workflowExecutor.ExecuteAsync(steps);
            var output = MessageCollectorStepProcessor.CollectOutput();

            await new Window
            {
                Title = "Execution Result",
                Width = 500,
                Height = 300,
                Content = new ScrollViewer
                {
                    Content = new TextBlock
                    {
                        Text = output,
                        Margin = new Thickness(16),
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            }.ShowDialog(this);
        };
    }

    private static void ExecLogic()
    {
        var service = ServiceManager.ServiceProvider.GetRequiredService<PartiallyUsedTestService>();
        service.ExecuteLogic();
    }

    private void ShowDashboard()
    {
        this.FindControl<DashboardView>("DashboardView")!.IsVisible = true;
        this.FindControl<TransactionListView>("TransactionListView")!.IsVisible = false;
    }

    private void ShowTransactions()
    {
        this.FindControl<DashboardView>("DashboardView")!.IsVisible = false;
        this.FindControl<TransactionListView>("TransactionListView")!.IsVisible = true;
    }
}