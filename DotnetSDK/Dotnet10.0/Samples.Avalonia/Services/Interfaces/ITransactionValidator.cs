using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

public interface ITransactionValidator
{
    bool IsValid(TransactionBase transaction, out string errorMessage);
}