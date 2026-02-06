using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class ReadFromEmbeddedResourceStepProcessor : StepProcessorBase
    {
        [InputArgument]
        [Required]
        public string ResourceName { get; set; }

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(ResourceName))
            {
                return StepResult.Failure("file_not_found");
            }

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(ResourceName);

            if (stream == null)
            {
                return StepResult.Failure("file_not_found");
            }

            using (var reader = new StreamReader(stream))
            {
                var result = await reader.ReadToEndAsync();
                return StepResult.Success(value: result);
            }
        }
    }
}
