using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    // C# 15 Feature: Union types — case types for Pet union
    public record class Cat(string Name);
    public record class Dog(string Name);
    public record class Bird(string Name);
    public union Pet(Cat, Dog, Bird);

    // C# 15 Feature: Closed hierarchies — all direct descendants known at compile time
    public closed record class GateState;
    public record class Closed : GateState;
    public record class Open(float Percent) : GateState;

    internal class CSharp15StepProcessor : StepProcessorBase
    {
        [InputArgument]
        public string? Name { get; set; }

        public string? result = "\e[32m C# 15: Success\e[0m"; // green text in terminal

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var message = new StringBuilder();
            message.Append(Name + "\n");

            // C# 15 Feature: Collection expression arguments
            // Pass constructor arguments inside a collection expression using with(...)
            message.Append("------\"Collection Expression Arguments\" implementation-----\n");
            string[] values = ["one", "two", "three"];
            List<string> names = [with(capacity: values.Length * 2), .. values];
            HashSet<string> set = [with(StringComparer.OrdinalIgnoreCase), "Hello", "HELLO", "hello"];
            message.Append($"Names (capacity preset to {values.Length * 2}): {string.Join(", ", names)}\n");
            message.Append($"Set count (OrdinalIgnoreCase deduplicated): {set.Count}\n\n");

            // C# 15 Feature: Union types
            // A value that can be one of several case types with implicit conversion.
            // The compiler ensures switch expressions are exhaustive across all case types.
            message.Append("------\"Union Types\" implementation-----\n");
            Pet pet1 = new Dog("Rex");
            Pet pet2 = new Cat("Whiskers");
            Pet pet3 = new Bird("Tweety");
            message.Append(DescribePet(pet1) + "\n");
            message.Append(DescribePet(pet2) + "\n");
            message.Append(DescribePet(pet3) + "\n\n");

            // C# 15 Feature: Closed hierarchies
            // A closed class fixes the set of direct descendants at compile time,
            // making switch expressions exhaustive without a default arm.
            message.Append("------\"Closed Hierarchies\" implementation-----\n");
            GateState[] states = [new Closed(), new Open(75.0f), new Open(10.5f)];
            foreach (var state in states)
            {
                message.Append(DescribeGate(state) + "\n");
            }
            message.Append("\n");

            // C# 15 Feature: Memory safety — pointer relaxations
            // Pointer declaration and address-of (&) no longer require an unsafe context.
            // Pointer dereference (*p, p[i]) still requires unsafe.
            message.Append("------\"Memory Safety Pointer Relaxations\" implementation-----\n");
            ShowPointerRelaxations(message);
            message.Append("\n");

            await Task.CompletedTask;
            message.Append(result);
            return StepResult.Success(message: $"\nResult : \n{message}\n");
        }

        static void ShowPointerRelaxations(StringBuilder message)
        {
            int number = 42;
            int* pointer = &number;    // C# 15: no unsafe context needed for declaration and &
            int[] nums = [10, 20, 30];
            fixed (int* pFirst = nums) // C# 15: fixed without unsafe block
            {
                unsafe
                {
                    // Dereferencing still requires unsafe
                    message.Append($"---C# 15: *pointer (dereference) = {*pointer}\n");
                    message.Append($"---C# 15: *pFirst (pinned array) = {*pFirst}\n");
                }
            }
            message.Append("---C# 15: Pointer declared and address-of used outside unsafe context\n");
        }

        // Union type switch: all case types handled — compiler enforces exhaustiveness
        static string DescribePet(Pet pet) => pet switch
        {
            Dog d => $"Dog: {d.Name}",
            Cat c => $"Cat: {c.Name}",
            Bird b => $"Bird: {b.Name}",
        };

        // Closed hierarchy switch: no default arm needed — all descendants are known
        static string DescribeGate(GateState state) => state switch
        {
            Closed => "Gate is closed",
            Open(var percent) => $"Gate is {percent:F1}% open",
        };
    }
}

// C# 15: ClosedAttribute is not yet shipped in the .NET 11 Preview 5 runtime.
// Declaring it manually is required until a future preview includes it.
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class ClosedAttribute : Attribute { }
}

