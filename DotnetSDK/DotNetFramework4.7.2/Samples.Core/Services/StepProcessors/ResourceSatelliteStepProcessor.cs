using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class ResourceSatelliteStepProcessor : IStepProcessor
    {
        [InputArgument]
        [Required]
        public string ResourceName { get; set; }

        private const string _resourceKey = "Greeting";
        public Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(ResourceName))
                return Task.FromResult(StepResult.Failure("ResourceName is required."));

            var resourceManager = new ResourceManager(ResourceName, Assembly.GetExecutingAssembly());

            var sb = new StringBuilder();

            sb.AppendLine($"fr: {resourceManager.GetString(_resourceKey, new CultureInfo("fr"))}");
            sb.AppendLine($"default culture: {resourceManager.GetString(_resourceKey, new CultureInfo("de"))}");

            return Task.FromResult(StepResult.Success(message: sb.ToString()));
        }
    }
}
