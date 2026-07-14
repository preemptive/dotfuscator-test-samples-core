using Avalonia;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ServiceManager.Services.AddStepsProcessors();
        //ServiceManager.Services.AddTransient<UnusedTestService>();
        //ServiceManager.Services.AddTransient<PartiallyUsedTestService>();

        ConfigurationManager.Builder
            .AddJsonFile(Path.Combine("Core", Constants.CoreAppsettings))
            .AddJsonFile("appsettings.json", optional: true);

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}