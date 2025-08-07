using Microsoft.Extensions.Configuration;

namespace PreEmptive.Dotfuscator.Samples.Core.Lib;

public static class ConfigurationManager
{
    private static IConfiguration? _configuration;
    private static IConfigurationBuilder? _configurationBuilder;
    private static readonly Lock Lock = new();

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

    public static IConfigurationBuilder Builder
    {
        get
        {
            if (_configurationBuilder != null) return _configurationBuilder;

            lock (Lock)
            {
                _configurationBuilder ??= new ConfigurationBuilder();
            }

            return _configurationBuilder;
        }
    }

    private static IConfiguration BuildConfiguration()
        => _configurationBuilder?.Build() ?? throw new ArgumentException($"{nameof(_configurationBuilder)} not set");
}