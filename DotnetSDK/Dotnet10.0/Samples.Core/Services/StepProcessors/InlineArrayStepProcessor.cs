using System.Runtime.CompilerServices;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    /// <summary>
    /// Tests InlineArray scenarios.
    ///
    /// Two categories are covered:
    ///   1. Compiler-generated <>y__InlineArray* structs - produced when the compiler lowers
    ///      a "params ReadOnlySpan&lt;T&gt;" call-site that passes literals/locals.  Dotfuscator's
    ///      Renaming and Control Flow transforms must leave these structs untouched.
    ///
    ///   2. User-defined [InlineArray(N)] structs - the explicit C# 12 feature.
    ///      Dotfuscator must not rename the single field or apply control-flow to the type.
    /// </summary>
    internal class InlineArrayStepProcessor : StepProcessorBase
    {
        public string? result = "\e[32m InlineArray: Success\e[0m";

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder();

            // ----------------------------------------------------------------
            // 1. params ReadOnlySpan<T> – compiler generates <>y__InlineArray*
            // ----------------------------------------------------------------
            sb.AppendLine("--- params ReadOnlySpan<int> ---");
            sb.AppendLine($"Sum(1,2,3)       = {Sum(1, 2, 3)}");
            sb.AppendLine($"Sum(10,20,30,40) = {Sum(10, 20, 30, 40)}");

            sb.AppendLine("--- params ReadOnlySpan<string> ---");
            sb.AppendLine($"Join('|', a,b,c) = {Join("|", "alpha", "beta", "gamma")}");

            sb.AppendLine("--- params ReadOnlySpan<double> ---");
            sb.AppendLine($"Average(1.0,2.0,3.0) = {Average(1.0, 2.0, 3.0):F2}");

            // Two-argument overload – compiler generates a different-sized InlineArray struct
            sb.AppendLine("--- params ReadOnlySpan<int> (2 args) ---");
            sb.AppendLine($"Sum(5,7) = {Sum(5, 7)}");

            // ----------------------------------------------------------------
            // 2. User-defined [InlineArray(N)] structs
            // ----------------------------------------------------------------
            sb.AppendLine("--- user-defined [InlineArray(4)] ---");
            var buf4 = new IntBuffer4();
            for (int i = 0; i < 4; i++) buf4[i] = (i + 1) * 5;
            sb.AppendLine($"IntBuffer4: {buf4[0]}, {buf4[1]}, {buf4[2]}, {buf4[3]}");

            sb.AppendLine("--- user-defined [InlineArray(8)] ---");
            var buf8 = new ByteBuffer8();
            for (int i = 0; i < 8; i++) buf8[i] = (byte)(i + 1);
            var bytes = new List<string>();
            for (int i = 0; i < 8; i++) bytes.Add(buf8[i].ToString());
            sb.AppendLine($"ByteBuffer8: {string.Join(", ", bytes)}");

            // ----------------------------------------------------------------
            // 3. Inline array used inside a loop (exercises control-flow paths)
            // ----------------------------------------------------------------
            sb.AppendLine("--- inline array in loop (control flow) ---");
            int total = 0;
            for (int round = 0; round < 3; round++)
            {
                total += Sum(round, round + 1, round + 2);
            }
            sb.AppendLine($"Loop total = {total}");

            sb.AppendLine(result);
            return StepResult.Success(message: $"\nResult:\n{sb}");
        }

        // ----------------------------------------------------------------
        // Helpers that trigger compiler-generated <>y__InlineArray* types
        // ----------------------------------------------------------------

        static int Sum(params ReadOnlySpan<int> values)
        {
            int s = 0;
            foreach (var v in values) s += v;
            return s;
        }

        static string Join(string separator, params ReadOnlySpan<string> values)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0) sb.Append(separator);
                sb.Append(values[i]);
            }
            return sb.ToString();
        }

        static double Average(params ReadOnlySpan<double> values)
        {
            double s = 0;
            foreach (var v in values) s += v;
            return values.Length == 0 ? 0 : s / values.Length;
        }

        // ----------------------------------------------------------------
        // User-defined InlineArray structs
        // ----------------------------------------------------------------

        [InlineArray(4)]
        public struct IntBuffer4
        {
            private int _element0;
        }

        [InlineArray(8)]
        public struct ByteBuffer8
        {
            private byte _element0;
        }
    }
}
