using PreEmptive.Dotfuscator.Samples.Core.Models;
using PreEmptive.Dotfuscator.Samples.Core.Services;

internal class Program
{
	private static void Main(string[] args)
	{
		var processor = new ConsoleOutputStepProcessor();

		processor.Process(new StepMetadata("Step 1 - Success"), StepResult.Success(value: 42, message: "All good."));
	}
}