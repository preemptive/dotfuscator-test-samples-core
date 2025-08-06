using Microsoft.Extensions.Configuration;
using PreEmptive.Dotfuscator.Samples.Core.Configurations;
using System;

namespace PreEmptive.Dotfuscator.Samples.Core
{
    public static class SettingsProvider
    {
        public static Settings Settings { get; }

        static SettingsProvider()
        {
            var settings = Lib.ConfigurationManager.Configuration?.Get<Settings>();
            if (settings is null)
            {
                Console.WriteLine("INVALID SETTINGS");
                settings = new Settings();
            }

            Settings = settings;
        }
    }
}
