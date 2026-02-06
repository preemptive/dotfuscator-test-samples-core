using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Lib;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;
namespace PreEmptive.Dotfuscator.Samples.Winform
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			ServiceManager.Services.AddStepsProcessors();

			ConfigurationManager.Builder
				.AddJsonFile($"Core/{Constants.CoreAppsettings}")
				.AddJsonFile("appsettings.core.json");
			Application.Run(new Form1());
        }
    }
}
