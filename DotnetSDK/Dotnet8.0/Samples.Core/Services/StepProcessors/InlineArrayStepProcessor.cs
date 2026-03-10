using System.Runtime.CompilerServices;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    /// <summary>
    /// Tests user-defined [InlineArray(N)] structs (C# 12 explicit feature).
    /// Note: params ReadOnlySpan&lt;T&gt; (C# 13) is not available on net8.0,
    /// so compiler-generated &lt;&gt;y__InlineArray* structs are not exercised here.
    /// Dotfuscator must not rename the single backing field or prune it away.
    /// </summary>
    internal class InlineArrayStepProcessor : StepProcessorBase
    {
        public string? result = "\u001b[32m InlineArray: Success\u001b[0m";

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder();

            // ----------------------------------------------------------------
            // User-defined [InlineArray(N)] structs (C# 12)
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
            // Inline array used inside a loop (exercises control-flow paths)
            // ----------------------------------------------------------------
            sb.AppendLine("--- inline array in loop (control flow) ---");
            int total = 0;
            var buf3 = new IntBuffer4();
            for (int round = 0; round < 3; round++)
            {
                buf3[0] = round;
                buf3[1] = round + 1;
                buf3[2] = round + 2;
                for (int i = 0; i < 3; i++) total += buf3[i];
            }
            sb.AppendLine($"Loop total = {total}");

            sb.AppendLine(result);
            return StepResult.Success(message: $"\nResult:\n{sb}");
        }

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
