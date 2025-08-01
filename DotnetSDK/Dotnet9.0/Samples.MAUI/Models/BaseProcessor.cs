namespace PreEmptive.Dotfuscator.TestSamples.MAUI.Models;

public abstract class BaseProcessor
{
    // Must be overridden
    public abstract string GetName();

    // Can be optionally overridden
    public virtual string Describe()
    {
        return $"Processor: {GetName()}";
    }

    // Can be optionally overridden
    public virtual string ExtraInfo()
    {
        return "Extra info from base processor.";
    }
}
