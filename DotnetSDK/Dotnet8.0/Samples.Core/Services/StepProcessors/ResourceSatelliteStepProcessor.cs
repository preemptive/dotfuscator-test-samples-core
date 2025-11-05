using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Resources;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors;

public class ResourceSatelliteStepProcessor : IStepProcessor
{
    [InputArgument]
    [Required]
    public string? ResourceName { get; set; }

    private const string _resourceKey = "Greeting";

    private ComplexObjectsClass complexObjectsClass1 = new ComplexObjectsClass();

    public Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var resourceManager = new ResourceManager(ResourceName!, Assembly.GetExecutingAssembly());

        var sb = new StringBuilder();

        sb.AppendLine($"fr: {resourceManager.GetString(_resourceKey, new CultureInfo("fr"))}");
        sb.AppendLine($"default culture: {resourceManager.GetString(_resourceKey, new CultureInfo("de"))}");


        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComplexObjectResources)); 
        resources.ApplyResources(complexObjectsClass1, "ComplexObjectsClass1");
        sb.AppendLine($"Complex Object Name = {complexObjectsClass1.Name}");
        sb.AppendLine($"Complex Object Size = {complexObjectsClass1.Size}");
        return Task.FromResult(StepResult.Success(message: sb.ToString()));
    }
}
