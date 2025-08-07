using Microsoft.Extensions.Configuration;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
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
