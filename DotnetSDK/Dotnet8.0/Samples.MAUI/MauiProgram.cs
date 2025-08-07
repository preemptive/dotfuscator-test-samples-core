using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using System.Reflection;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;

namespace Samples.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ServiceManager.Services.AddStepsProcessors();

            ConfigurationManager.Builder
                .AddJsonFile($"Core\\{Constants.CoreAppsettings}")
                .AddJsonFile("appsettings.json");

            //var assembly = Assembly.GetExecutingAssembly();
            //using var coreAppSettingsStream = assembly.GetManifestResourceStream($"PreEmptive.Dotfuscator.Samples.Maui.Core.{Constants.CoreAppsettings}");
            //if(coreAppSettingsStream is not null)
            //{
            //    ConfigurationManager.Builder.AddJsonStream(coreAppSettingsStream);
            //}

            //using var appSettingsStream = assembly.GetManifestResourceStream($"PreEmptive.Dotfuscator.Samples.Maui.appsettings.json");
            //if (appSettingsStream is not null)
            //{
            //    ConfigurationManager.Builder.AddJsonStream(appSettingsStream);
            //}
            //ConfigurationManager.Builder.Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
