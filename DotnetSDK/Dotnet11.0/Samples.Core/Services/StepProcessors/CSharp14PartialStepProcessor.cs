using System;
using System.Collections.Generic;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    // C# 14 Feature partial members demo

    public partial class CSharp14StepProcessor : StepProcessorBase
    {
        public partial CSharp14StepProcessor();

        // Delegate examples
        private delegate bool TryParse<T>(string text, out T result);
        private delegate int RefModifierDelegate(ref int value);
        private delegate void InModifierDelegate(in int value);
        private delegate void RefReadonlyDelegate(ref readonly int value);
        private delegate void ScopedDelegate(scoped ref int value);
    }

    // Helper classes for null-conditional assignment C#14 demo
    public class Order
    {
        public string Description { get; set; }
    }

    public class Customer
    {
        public Order? Order { get; set; }
    }

    public class Counter
    {
        public int Value { get; set; }
    }
}
