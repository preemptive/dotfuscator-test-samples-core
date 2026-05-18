using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Validators;

public class ExpenseCategoryValidator : ITransactionValidator
{
    public bool IsValid(TransactionBase transaction, out string errorMessage)
    {
        if (transaction is ExpenseTransaction expense && expense.Category == null)
        {
            errorMessage = "Expense transactions must have a category.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }
}