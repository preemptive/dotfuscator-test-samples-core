using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class WriteToFileStepProcessor : StepProcessorBase
    {
        [InputArgument]
        [Required]
        public string FileName { get; set; } = "writeToFileResult.txt";

        [InputArgument]
        public string OutputContent { get; set; } = "File Written Successfully";

        [InputArgument]
        public string OutputPath { get; set; }

        protected override Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var path = string.IsNullOrEmpty(OutputPath)
                ? FileName
                : Path.Combine(OutputPath, FileName);

            // File.WriteAllText is synchronous in .NET Framework 4.7.2
            File.WriteAllText(path, OutputContent);

            return Task.FromResult(StepResult.Success(message: $"Written to {path}"));
        }
    }
}
