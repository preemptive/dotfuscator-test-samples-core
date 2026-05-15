using System;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

public abstract class TransactionBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public abstract TransactionType Type { get; }
    public abstract string DisplaySummary();
}