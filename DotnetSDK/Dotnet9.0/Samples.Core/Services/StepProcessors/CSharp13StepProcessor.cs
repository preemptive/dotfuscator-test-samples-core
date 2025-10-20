using Microsoft.Extensions.Primitives;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    internal class CSharp13StepProcessor : StepProcessorBase
    {
        [InputArgument]
        public string? Name { get; set; }
        
        // "\e" Escape character
        public string? result = "\e[32mSuccess\e[0m"; // green text in terminal

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
            // Before c#13 the below lines of code would have confused the compiler
            staticMessage.Append("------\"Method group natural type\" implementation-----\n");
            Action<int> f = PrintProvidedInput;
            Action<string> g = PrintProvidedInput;
            Action<bool> h = PrintProvidedInput;

            f(2025);
            g("Testing");
            h(true);

            //int[] numbersRef = { 1, 2, 3, 4 };
            //foreach (ref int n in GetRefs(numbersRef))
            //{
            //    n *= 10;
            //}

            //fixed (int* p = numbers)
            //{
            //    int result = SumAsync(p, numbers.Length).Result;
            //    Console.WriteLine(result);
            //}

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
            return StepResult.Success(message: $"\nResult : \n{message}");
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

        //static IEnumerable<ref int> GetRefs(int[] array)
        //{
        //    for (int i = 0; i < array.Length; i++)
        //        yield return ref array[i];
        //}

        //unsafe static async Task<int> SumAsync(int* p, int length)
        //{
        //    int sum = 0;
        //    for (int i = 0; i < length; i++)
        //        sum += p[i];
        //    await Task.Delay(10);
        //    return sum;
        //}

        public interface IProcessor
        {
            void Process();
        }

        // ref struct implementing interfaces
        public ref struct MyRefStruct : IProcessor
        {
            public int X;

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
        class Logger
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
