using Microsoft.Extensions.DependencyInjection;

namespace PreEmptive.Dotfuscator.Samples.Core.Lib;

public static class ServiceManager
{
    private static IServiceProvider? _services;
    private static readonly Lock Lock = new();

    public static IServiceCollection Services { get; } = new ServiceCollection();

    public static IServiceProvider ServiceProvider
    {
        get
        {
            if (_services != null) return _services;

            lock (Lock)
            {
                _services ??= Services.BuildServiceProvider();
            }

            return _services;
        }
    }
}