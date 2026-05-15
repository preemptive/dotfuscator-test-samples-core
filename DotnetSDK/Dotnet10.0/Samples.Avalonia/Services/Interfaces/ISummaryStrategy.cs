using System.Collections.Generic;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

public interface ISummaryStrategy
{
    string Label { get; }
    decimal CalculateTotalIncome(IEnumerable<TransactionBase> transactions);
    decimal CalculateTotalExpense(IEnumerable<TransactionBase> transactions);
    decimal CalculateBalance(IEnumerable<TransactionBase> transactions);
}