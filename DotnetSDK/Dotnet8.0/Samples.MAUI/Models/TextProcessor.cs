namespace PreEmptive.Dotfuscator.TestSamples.MAUI.Models;

public class TextProcessor : BaseProcessor
{
    public override string GetName() => "TextProcessor";

    public override string Describe() => $"[Text] {base.Describe()}";

    public override string ExtraInfo() => "Overridden extra info.";
}
