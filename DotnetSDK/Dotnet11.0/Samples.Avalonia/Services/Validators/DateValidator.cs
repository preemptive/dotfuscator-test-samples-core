using System;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Validators;

public class DateValidator : ITransactionValidator
{
    public bool IsValid(TransactionBase transaction, out string errorMessage)
    {
        if (transaction.Date > DateTime.Now)
        {
            errorMessage = "Transaction date cannot be in the future.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}