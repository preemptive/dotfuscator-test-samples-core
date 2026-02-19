using System.Runtime.CompilerServices;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    internal class CSharp13StepProcessor : StepProcessorBase
    {
        [InputArgument]
        public string? Name { get; set; }

        // "\e" Escape character
        public string? result = "\e[32m C# 13: Success\e[0m"; // green text in terminal

        public int[]? numbers = new int[5] { 1, 2, 3, 4, 5 };

        static StringBuilder staticMessage = new StringBuilder();

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var message = new StringBuilder();

            message.Append(Name + "\n");

            // "params" implementation
            message.Append("------\"params\" collections implementation-----\n");
            Show(message, new List<int> { 1, 2, 3 }, new[] { 4, 5, 6 }, Enumerable.Range(7, 3));
            message.Append("\n");

            // Implicit Index Access
            message.Append("------\"Implicit Index Access\" implementation-----\n");
            numbers[^1] = 100;
            numbers[^2] = 99;
            message.Append(string.Join(", ", numbers) + "\n");
            message.Append("\n");

            // Method group natural type
            staticMessage.Append("------\"Method group natural type\" implementation-----\n");
            Action<int> f = PrintProvidedInput;
            Action<string> g = PrintProvidedInput;
            Action<bool> h = PrintProvidedInput;

            f(2025);
            g("Testing");
            h(true);

            //ref local in iterators methods
            message.Append("------\"Ref Local in iterators methods\" implementation-----\n");
            int[] numbersRef = { 1, 2, 3, 4 };
            foreach (int n in GetRefs(numbersRef))
            {
                message.Append(n + "\t");
            }
            message.Append("\n");
            //ref local in async methods
            message.Append("------\"Ref Local in async methods\" implementation-----\n");
            int total = await SumAsync(numbersRef);
            message.Append("Total = " + total + "\n\n");

            staticMessage.Append("------\"allows ref structs\" implementation-----\n");
            var w = new Wrapper<MyRefStruct>();
            MyRefStruct m = new MyRefStruct { X = 10 };
            w.Print(m);
            // IProcessor p = m; Direct interface conversion is still forbidden

            staticMessage.Append("------\"Overload Resolution Priority\" implementation-----\n");
            var logger = new Logger();
            logger.Log("Hello");

            message.Append(staticMessage.ToString() + "\n");

            message.Append("------\"New Escape Character\" implementation-----\n");
            message.Append(result);
            return StepResult.Success(message: $"\nResult : \n{message}\n");
        }

        public static void Show<T>(StringBuilder message, params IEnumerable<T>[] collections)
        {
            foreach (var coll in collections)
            {
                message.Append("Collection: \n");
                foreach (var item in coll)
                {
                    message.Append(item + " ");
                }
                message.Append("\n");
            }
        }

        static void PrintProvidedInput(int x) => staticMessage.Append($"int {x}\n");
        static void PrintProvidedInput(string x) => staticMessage.Append($"string {x}\n");
        static void PrintProvidedInput<T>(T x) => staticMessage.Append($"generic {x}\n\n");

        static IEnumerable<int> GetRefs(int[] array)
        {
            for (int i = 1; i <= 5; i++)
            {
                MyRefStruct local = new MyRefStruct(i);
                int doubled = local.Value * 2;
                yield return doubled;
            }
        }
        static async Task<int> SumAsync(int[] array)
        {
            int sum = 0;

            for (int i = 1; i <= 5; i++)
            {
                MyRefStruct local = new MyRefStruct(i);
                sum += local.Value * 3;
                await Task.Delay(10);
            }

            return sum;
        }

        public interface IProcessor
        {
            void Process();
        }

        // ref struct implementing interfaces
        public ref struct MyRefStruct : IProcessor
        {
            public int X;
            public int Value;

            public MyRefStruct(int value) => Value = value;

            public void Process()
            {
                staticMessage.Append("------\"ref struct interfaces\" implementation-----\n");
                X *= 10;
                staticMessage.Append($"Ref Struct Processed value: {X}\n\n");
            }
        }

        // "allows ref structs" implementation
        public class Wrapper<T> where T : IProcessor, allows ref struct
        {
            public void Print(scoped T value)
            {
                staticMessage.Append($"Ref Struct type processed: {typeof(T).Name}\n\n");
                value.Process();
            }
        }

        // Overload resolution priority
        private class Logger
        {
            [OverloadResolutionPriority(0)]
            public void Log(string message)
            {
                staticMessage.Append($"[Lower Priority] Logging string: {message}\n");
            }

            [OverloadResolutionPriority(1)]
            public void Log(ReadOnlySpan<char> message)
            {
                staticMessage.Append($"[Higher Priority] Logging ReadOnlySpan<char>: {message.ToString()}\n");
            }
        }

    }
}
