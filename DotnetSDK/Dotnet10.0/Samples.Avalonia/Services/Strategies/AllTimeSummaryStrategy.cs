using System.Collections.Generic;
using System.Linq;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Strategies;

public class AllTimeSummaryStrategy : ISummaryStrategy
{
    public string Label => "All Time";

    public decimal CalculateTotalIncome(IEnumerable<TransactionBase> transactions) =>
        transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);

    public decimal CalculateTotalExpense(IEnumerable<TransactionBase> transactions) =>
        transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

    public decimal CalculateBalance(IEnumerable<TransactionBase> transactions) =>
        CalculateTotalIncome(transactions) - CalculateTotalExpense(transactions);
}