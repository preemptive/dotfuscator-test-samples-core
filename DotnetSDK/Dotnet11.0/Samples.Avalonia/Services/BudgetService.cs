using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure.Events;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Strategies;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services;

public class BudgetService : IBudgetService
{
    private readonly IEnumerable<ITransactionValidator> _validators;
    private readonly IEventAggregator _eventAggregator;
    private ISummaryStrategy _strategy;

    public ObservableCollection<TransactionBase> Transactions { get; } = new();
    public ISummaryStrategy CurrentStrategy => _strategy;

    public BudgetService(
        IEnumerable<ITransactionValidator> validators,
        IEventAggregator eventAggregator)
    {
        _validators = validators;
        _eventAggregator = eventAggregator;
        _strategy = new AllTimeSummaryStrategy();
    }

    public void SetStrategy(ISummaryStrategy strategy) =>
        _strategy = strategy;

    public void Add(TransactionBase transaction)
    {
        foreach (var validator in _validators)
        {
            if (!validator.IsValid(transaction, out var error))
                throw new InvalidOperationException(error);
        }

        Transactions.Add(transaction);

        // Publish event — notify all subscribers
        _eventAggregator.Publish(new TransactionAddedEvent(transaction));
    }

    public void Remove(TransactionBase transaction)
    {
        Transactions.Remove(transaction);

        // Publish event — notify all subscribers
        _eventAggregator.Publish(new TransactionRemovedEvent(transaction));
    }

    public decimal GetTotalIncome() =>
        _strategy.CalculateTotalIncome(Transactions);

    public decimal GetTotalExpense() =>
        _strategy.CalculateTotalExpense(Transactions);

    public decimal GetBalance() =>
        _strategy.CalculateBalance(Transactions);
}