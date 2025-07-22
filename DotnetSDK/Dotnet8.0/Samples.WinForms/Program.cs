using System;
using System.Windows.Forms;
using SampleLibrary.Classes;
using SampleLibrary.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.WinForms
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "WinForms Sample App";
            Width = 400;
            Height = 300;

            var button = new Button { Text = "Run", Dock = DockStyle.Fill };
            button.Click += (s, e) => {
                Program.RunApp();
            };
            Controls.Add(button);
        }
    }

    class Program : AbstractBase, IExample
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }

        public override void Run() => MessageBox.Show("Running app logic");
        public void InterfaceMethod() => MessageBox.Show("IExample implemented");
        public static void RunApp()
        {
            var p = new Program();
            p.Run();
            p.InterfaceMethod();
			
			
        }
    }
}
