using System.Collections.ObjectModel;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

public interface ITransactionRepository
{
    ObservableCollection<TransactionBase> Transactions { get; }
    void Add(TransactionBase transaction);
    void Remove(TransactionBase transaction);
}