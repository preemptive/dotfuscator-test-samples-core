using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure.Events;

public class TransactionAddedEvent : IEvent
{
    public TransactionBase Transaction { get; }

    public TransactionAddedEvent(TransactionBase transaction)
    {
        Transaction = transaction;
    }
}