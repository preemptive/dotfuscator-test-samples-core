using System;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure.Events;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Infrastructure;

public interface IEventAggregator
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
    void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;
}