using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using System.Text;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public partial class CSharp14StepProcessor : StepProcessorBase
    {
        [InputArgument]

        // C# 14-Auto-property with backing field feature demo
        public  string Name
        {
            get;                         
            set => field = "C#14StepProcessorFieldValue";    // Modify the backing field
        }
        
        // C# 14 Feature partial Constructor demo
        public partial CSharp14StepProcessor()
        {
            string name = null;
            name ??= "C#14StepProcessorConstructorValue";
            Name = name;
        }

        // C# 14 Partial Members - Static Constructor demo
        private static int count;
        static CSharp14StepProcessor()  =>  count = 10;

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {

            // C# 14 extension members feature demo
            var sb = new StringBuilder();
            var numbers = new List<int> { 1, 2, 3, 4 };

            // 1️ Instance extension members
            var isEmptyResult = $"numbers.IsEmpty = {numbers.IsEmpty}";
            var indexAccessResult = $"numbers[2] = {numbers[2]}";

            var evenNumbers = numbers.WhereEven(x => x % 2 == 0);
            var evenNumbersResult = $"Even numbers: {string.Join(", ", evenNumbers)}";

            // 2️ Static extension members
            var combined = IEnumerable<int>.Combine(numbers, new[] { 5, 6 });
            var combinedResult = $"Combined: {string.Join(", ", combined)}";

            var emptySeq = IEnumerable<int>.Empty;
            var emptyResult = $"Empty has elements? {!emptySeq.Any()}";

            // nameof with unbound generic types (C# 14 feature)
            var nameofList = $"nameof(List<>) = {nameof(List<>)}";
            var nameofDict = $"nameof(Dictionary<,>) = {nameof(Dictionary<,>)}";
            var nameofNullable = $"nameof(Nullable<>) = {nameof(Nullable<>)}";

           
            // C# 14 Implicit Span conversions feature demo
            int[] numbersForSpan = { 1, 2, 3, 4 };
            ReadOnlySpan<int> span1 = numbersForSpan;
            Span<int> span2 = numbersForSpan;
            ReadOnlySpan<char> span3 = Name;
           
            // -------------------------------
            // C# 14 Null-conditional assignment demo (properties)
            // -------------------------------
            Customer? customer1 = new Customer();   // not null
            Customer? customer2 = null;             // null

            customer1?.Order = new Order { Description = "Laptop" };
            customer2?.Order = new Order { Description = "Phone" }; // skipped

            // compound assignment with null-conditional
            Counter? counter1 = new Counter { Value = 10 };
            Counter? counter2 = null;

            counter1?.Value += 5;  // works → 15
            counter2?.Value += 5;  // skipped safely

            // -------------------------------
            // C# 14 Null-conditional indexer assignment demo
            // -------------------------------
            Dictionary<string, int>? dict1 = new Dictionary<string, int>();
            Dictionary<string, int>? dict2 = null;

            dict1?["apples"] = 5;   // works
            dict2?["oranges"] = 10; // skipped safely

            // compound assignment via indexer
            dict1?["apples"] += 3;  // 5 + 3 = 8
            dict2?["oranges"] += 2; // skipped safely

            sb.AppendLine($"Name = {Name}");
            sb.AppendLine(isEmptyResult);
            sb.AppendLine(indexAccessResult);
            sb.AppendLine(evenNumbersResult);
            sb.AppendLine(combinedResult);
            sb.AppendLine(emptyResult);
            sb.AppendLine($"----------C#14 Feature - Extension members successfully tested----------\n");

            // nameof with unbound generic types (C# 14 feature)
            sb.AppendLine(nameofList);
            sb.AppendLine(nameofDict);
            sb.AppendLine(nameofNullable);
            sb.AppendLine($"----------C#14 Feature - nameof with unbound generic types tested successfully----------\n");

            // C# 14 Implicit Span conversions feature demo
            sb.AppendLine($"ReadOnlySpan<char> = {span3[0]},{span3[4]}");
            sb.AppendLine($"ReadOnlySpan<int> = {span1[0]},{span1[2]}");
            sb.AppendLine($"Span<int> = {span2[0]},{span2[2]}");
            sb.AppendLine($"----------C#14 Feature - Implicit Span conversions tested successfully----------\n");

            // C# 14 Null-conditional assignment and  Null-conditional indexer assignmentdemo (properties)

            sb.AppendLine($"customer1.Order.Description = {customer1.Order?.Description}");
            sb.AppendLine($"customer2.Order?.Description = {customer2?.Order?.Description ?? "null"}");
            sb.AppendLine($"counter1.Value = {counter1.Value}");
            sb.AppendLine($"counter2?.Value = {(counter2 != null ? counter2.Value.ToString() : "null")}");

            sb.AppendLine($"dict1[\"apples\"] = {dict1?["apples"]}");
            sb.AppendLine($"dict2?[\"oranges\"] = {(dict2 is null ? "null" : dict2["oranges"].ToString())}");
            sb.AppendLine($"----------C#14 Feature - Null-conditional assignment and Null-conditional indexer assignment tested successfully----------\n");


            // -------------------------------
            // C# 14 Simple Lambda Parameters with Modifiers demo
            // -------------------------------

            // 1. 'out' modifier
            TryParse<int> parse = (text, out result) => int.TryParse(text, out result);
            parse("123", out int parseResult);
            sb.AppendLine($"Out modifier: parseResult = {parseResult}");

            // 2. 'ref' modifier
            RefModifierDelegate increment = (ref value) =>
            {
                value++;
                return value;
            };
            int x = 5;
            int newX = increment(ref x);
            sb.AppendLine($"Ref modifier: x = {x}, newX = {newX}");

            // 3. 'in' modifier
            InModifierDelegate printValue = (in value) =>
            {
                sb.AppendLine($"In modifier: value = {value}");
            };
            int y = 10;
            printValue(in y);

            // 4. 'ref readonly' modifier
            RefReadonlyDelegate printReadonly = (ref readonly value) =>
            {
                sb.AppendLine($"Ref readonly modifier: value = {value}");
            };
            int z = 20;
            printReadonly(ref z);

            // 5. 'scoped' modifier
            ScopedDelegate scopedDemo = (scoped ref value) =>
            {
               sb.AppendLine($"Scoped modifier: value = {value}");
            };
            int s = 42;
            scopedDemo( ref s);

            // 6. Discards
            Action<int, int> discardExample = (_, __) =>
            {
                sb.AppendLine("Discard parameters example executed");
            };
            discardExample(1, 2);

            // 7. Lambda returning result with 'ref' parameter
            RefModifierDelegate multiply = (ref value) =>
            {
                value *= 2;
                return value;
            };
            int a = 7;
            int doubled = multiply(ref a);
            sb.AppendLine($"Lambda with ref returning result: doubled = {doubled}");

            sb.AppendLine($"----------C#14 Feature - Simple lambda parameters with modifiers scoped, ref, in, out, or ref readonly tested successfully----------\n");

            return StepResult.Success(message: $"\nResult : \n{sb.ToString()}");
            //return StepResult.Success(message: $"Name = \"{Name}\"");
        }
    }
}
