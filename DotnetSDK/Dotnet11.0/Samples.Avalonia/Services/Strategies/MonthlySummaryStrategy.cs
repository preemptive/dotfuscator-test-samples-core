using System;
using System.Collections.Generic;
using System.Linq;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Strategies;

public class MonthlySummaryStrategy : ISummaryStrategy
{
    public string Label => $"This Month ({DateTime.Now:MMMM yyyy})";

    private IEnumerable<TransactionBase> FilterCurrentMonth(IEnumerable<TransactionBase> transactions) =>
        transactions.Where(t =>
            t.Date.Month == DateTime.Now.Month &&
            t.Date.Year == DateTime.Now.Year);

    public decimal CalculateTotalIncome(IEnumerable<TransactionBase> transactions) =>
        FilterCurrentMonth(transactions)
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

    public decimal CalculateTotalExpense(IEnumerable<TransactionBase> transactions) =>
        FilterCurrentMonth(transactions)
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

    public decimal CalculateBalance(IEnumerable<TransactionBase> transactions) =>
        CalculateTotalIncome(transactions) - CalculateTotalExpense(transactions);
}