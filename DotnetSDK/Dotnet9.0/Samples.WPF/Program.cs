using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Samples.Common.Classes;
using Samples.Common.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.WPF
{
    class Program : AbstractBase, IExample
    {
        static void main()
        {
            var prog = new Program();
            prog.Run();
            prog.InterfaceMethod();
            prog.LoadResources();
        }

        public override void Run()
        {
            MessageBox.Show("Running app logic");
            Helper();
        }

        public void InterfaceMethod()
        {
            MessageBox.Show("IExample implemented");
        }

        public void LoadResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string resName in assembly.GetManifestResourceNames())
            {
                using var stream = assembly.GetManifestResourceStream(resName);
                MessageBox.Show($"Resource: {resName}, Size: {stream?.Length}");
            }
        }
    }
}
