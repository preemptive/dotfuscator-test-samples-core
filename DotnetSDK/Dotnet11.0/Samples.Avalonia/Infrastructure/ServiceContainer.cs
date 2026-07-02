using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Strategies;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Validators;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;

public static class ServiceContainer
{
    private static ServiceProvider? _provider;

    public static ServiceProvider Provider =>
        _provider ?? throw new InvalidOperationException("ServiceContainer not initialized.");

    public static void Build()
    {
        var services = new ServiceCollection();

        // Observer — EventAggregator registered as singleton
        services.AddSingleton<IEventAggregator, EventAggregator>();

        // Validators
        services.AddSingleton<ITransactionValidator, AmountValidator>();
        services.AddSingleton<ITransactionValidator, ExpenseCategoryValidator>();
        services.AddSingleton<ITransactionValidator, DateValidator>();

        // Factory
        services.AddSingleton<ITransactionFactory, TransactionFactory>();

        // Strategies
        services.AddSingleton<ISummaryStrategy, AllTimeSummaryStrategy>();
        services.AddSingleton<ISummaryStrategy, MonthlySummaryStrategy>();

        // BudgetService
        services.AddSingleton<BudgetService>();
        services.AddSingleton<IBudgetService>(sp => sp.GetRequiredService<BudgetService>());
        services.AddSingleton<ITransactionRepository>(sp => sp.GetRequiredService<BudgetService>());
        services.AddSingleton<IBudgetSummaryService>(sp => sp.GetRequiredService<BudgetService>());

        // ViewModels
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<TransactionListViewModel>();

        _provider = services.BuildServiceProvider();
    }
}