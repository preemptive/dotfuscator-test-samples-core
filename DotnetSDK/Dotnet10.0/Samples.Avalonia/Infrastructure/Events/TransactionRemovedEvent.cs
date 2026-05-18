using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure.Events;

public class TransactionRemovedEvent : IEvent
{
    public TransactionBase Transaction { get; }

    public TransactionRemovedEvent(TransactionBase transaction)
    {
        Transaction = transaction;
    }
}