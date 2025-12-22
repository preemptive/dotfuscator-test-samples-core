using System.ComponentModel.DataAnnotations;
using System.IO;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;

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
        public string? OutputPath { get; set; }

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var path = string.Empty;
            if (!string.IsNullOrEmpty(OutputPath))
            {
                path = Path.Combine(OutputPath, FileName);
            }
            else
            {
                path = FileName;
            }

            await File.WriteAllTextAsync(path, OutputContent, cancellationToken);
            return StepResult.Success(message: $"Written to {path}");
        }
    }
}
