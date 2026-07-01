using System.Collections.ObjectModel;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

public interface IBudgetSummaryService
{
    ObservableCollection<TransactionBase> Transactions { get; }
    ISummaryStrategy CurrentStrategy { get; }
    void SetStrategy(ISummaryStrategy strategy);
    decimal GetTotalIncome();
    decimal GetTotalExpense();
    decimal GetBalance();
}