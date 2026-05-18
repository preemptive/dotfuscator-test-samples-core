using System;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services;

public class TransactionFactory : ITransactionFactory
{
    public IncomeTransaction CreateIncome(DateTime date, decimal amount, string? note) =>
        new()
        {
            Date = date,
            Amount = amount,
            Note = note
        };

    public ExpenseTransaction CreateExpense(DateTime date, decimal amount, Category category, string? note) =>
        new()
        {
            Date = date,
            Amount = amount,
            Category = category,
            Note = note
        };
}