namespace PreEmptive.Dotfuscator.Samples.Blazor.Services;

public class PartiallyUsedTestService
{
    public void ExecuteLogic()
    {
        Console.WriteLine("This method is used.");
    }

    public void ExecuteUnusedLogic()
    {
        Console.WriteLine("This method is unused.");
    }
}
