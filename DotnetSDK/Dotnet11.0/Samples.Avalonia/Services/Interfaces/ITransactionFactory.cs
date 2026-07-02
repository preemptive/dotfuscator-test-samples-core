using System;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

public interface ITransactionFactory
{
    IncomeTransaction CreateIncome(DateTime date, decimal amount, string? note);
    ExpenseTransaction CreateExpense(DateTime date, decimal amount, Category category, string? note);
}