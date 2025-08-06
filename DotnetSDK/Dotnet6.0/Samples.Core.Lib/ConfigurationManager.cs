using Microsoft.Extensions.Configuration;

namespace PreEmptive.Dotfuscator.Samples.Core.Lib;

public static class ConfigurationManager
{
    private static IConfiguration? _configuration;
    private static readonly object Lock = new object();
    private static readonly HashSet<string> _sources = new HashSet<string>();

    public static IConfiguration? Configuration
    {
        get
        {
            if (_configuration != null) return _configuration;
            
            lock (Lock)
            {
                _configuration ??= BuildConfiguration();
            }

            return _configuration;
        }
    }

    public static void AddSource(string source) => _sources.Add(source);

    public static void ClearSources() => _sources.Clear();

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

        foreach (var source in _sources)
        {
            builder.AddJsonFile(source);
        }
        
        return builder.Build();
    }
}