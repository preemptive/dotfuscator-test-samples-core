using System;
using Microsoft.Extensions.Configuration;

namespace PreEmptive.Dotfuscator.Samples.Core.Lib
{
    public static class ConfigurationManager
    {
        private static IConfiguration _configuration;
        private static IConfigurationBuilder _configurationBuilder;
        private static readonly object Lock = new object();

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration != null) return _configuration;

                lock (Lock)
                {
                    if (_configuration == null)
                    {
                        _configuration = BuildConfiguration();
                    }
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
                    if (_configurationBuilder == null)
                    {
                        _configurationBuilder = new ConfigurationBuilder();
                    }
                }

                return _configurationBuilder;
            }
        }

        private static IConfiguration BuildConfiguration()
        {
            if (_configurationBuilder == null)
            {
                throw new ArgumentException("_configurationBuilder not set");
            }

            return _configurationBuilder.Build();
        }
    }
}