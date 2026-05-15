using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure.Events;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Strategies;
using PreEmptive.Dotfuscator.Samples.Avalonia.Lib.Helpers;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IBudgetSummaryService _summaryService;
    private readonly IEventAggregator _eventAggregator;

    public IEnumerable<ISummaryStrategy> AvailableStrategies { get; } =
    [
        new AllTimeSummaryStrategy(),
        new MonthlySummaryStrategy()
    ];

    [ObservableProperty]
    private ISummaryStrategy selectedStrategy;

    public string StrategyLabel => _summaryService.CurrentStrategy.Label;
    public decimal Balance => _summaryService.GetBalance();
    public decimal TotalIncome => _summaryService.GetTotalIncome();
    public decimal TotalExpense => _summaryService.GetTotalExpense();

    public DashboardViewModel(IBudgetSummaryService summaryService, IEventAggregator eventAggregator)
    {
        _summaryService = summaryService;
        _eventAggregator = eventAggregator;
        SelectedStrategy = AvailableStrategies.First();

        // Subscribe to events — loosely coupled (Observer)
        _eventAggregator.Subscribe<TransactionAddedEvent>(_ => RefreshSummary());
        _eventAggregator.Subscribe<TransactionRemovedEvent>(_ => RefreshSummary());
    }

    partial void OnSelectedStrategyChanged(ISummaryStrategy value)
    {
        _summaryService.SetStrategy(value);
        RefreshSummary();
    }

    private void RefreshSummary()
    {
        OnPropertyChanged(nameof(Balance));
        OnPropertyChanged(nameof(TotalIncome));
        OnPropertyChanged(nameof(TotalExpense));
        OnPropertyChanged(nameof(StrategyLabel));
        OnPropertyChanged(nameof(BalanceStatus));
        OnPropertyChanged(nameof(SummaryReport));
    }

    // In RefreshSummary or expose to UI:
    public string BalanceStatus =>
        TransactionSummaryHelper.GetBalanceStatus(_summaryService.GetBalance());

    public string SummaryReport =>
        TransactionSummaryHelper.BuildSummaryReport(
            StrategyLabel,
            TotalIncome,
            TotalExpense,
            Balance);
}