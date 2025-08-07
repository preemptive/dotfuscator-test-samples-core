using Microsoft.Extensions.Configuration;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using System.Windows;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;

namespace Samples.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceManager.Services.AddStepsProcessors();

            ConfigurationManager.Builder
                .AddJsonFile($"Core\\{Constants.CoreAppsettings}")
                .AddJsonFile("appsettings.json");
        }
    }

}
