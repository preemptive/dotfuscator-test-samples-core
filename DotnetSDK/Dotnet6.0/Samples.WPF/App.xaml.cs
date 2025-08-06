using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using System.Windows;

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

            ConfigurationManager.AddSource($"Core\\{Constants.CoreAppsettings}");
            ConfigurationManager.AddSource("appsettings.json");
        }
    }

}
