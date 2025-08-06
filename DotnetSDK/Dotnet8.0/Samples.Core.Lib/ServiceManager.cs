using Microsoft.Extensions.DependencyInjection;

namespace PreEmptive.Dotfuscator.Samples.Core.Lib;

public static class ServiceManager
{
    private static IServiceProvider? _services;
    private static readonly object _lock = new();

    public static IServiceCollection Services { get; } = new ServiceCollection();

    public static IServiceProvider ServiceProvider
    {
        get
        {
            if (_services != null) return _services;

            lock (_lock)
            {
                _services ??= Services.BuildServiceProvider();
            }

            return _services;
        }
    }
}