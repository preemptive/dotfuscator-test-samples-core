using System;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

public class Transaction : TransactionBase
{
    public TransactionType TransactionType { get; set; }
    public Category? Category { get; set; }

    public override TransactionType Type => TransactionType;

    public override string DisplaySummary() =>
        $"[{Type}] {Date:yyyy-MM-dd} | {Amount:C} | {Category} | {Note}";
}