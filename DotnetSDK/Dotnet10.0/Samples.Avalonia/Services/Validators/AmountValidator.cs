using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Validators;

public class AmountValidator : ITransactionValidator
{
    public bool IsValid(TransactionBase transaction, out string errorMessage)
    {
        if (transaction.Amount <= 0)
        {
            errorMessage = "Amount must be greater than zero.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}