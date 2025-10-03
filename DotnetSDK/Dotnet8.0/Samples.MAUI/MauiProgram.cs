using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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


#if ANDROID
            // Load appsettings.core.json from Resources/Raw
            var stream = FileSystem.OpenAppPackageFileAsync("appsettings.core.json").GetAwaiter().GetResult();
            ConfigurationManager.Builder.AddJsonStream(stream);
            
#else
            ConfigurationManager.Builder
                .AddJsonFile($"Core\\{Constants.CoreAppsettings}")
                .AddJsonFile("appsettings.json");
#endif
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
