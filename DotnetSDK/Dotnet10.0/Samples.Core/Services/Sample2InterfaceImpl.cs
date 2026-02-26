using PreEmptive.Dotfuscator.Samples.Core.Abstractions;

namespace PreEmptive.Dotfuscator.Samples.Core.Services;

internal class Sample2InterfaceImpl : ISample2<string, string>
{
    public string GetSampleValue() => $"SUCCESS";
}

