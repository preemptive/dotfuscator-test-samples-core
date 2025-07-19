using System.Windows;
using SampleLibrary.Classes;
using SampleLibrary.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.WPF
{
    public partial class MainWindow : Window, IExample
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Program1.RunApp(this);
        }

        public void InterfaceMethod() => MessageBox.Show("IExample implemented");
    }

    public class Program1 : AbstractBase
    {
        public override void Run() => MessageBox.Show("Running app logic");

        public static void RunApp(MainWindow context)
        {
            var p = new Program1();
            p.Run();
            context.InterfaceMethod();

            var prog = new Program();
            prog.Run();
            prog.InterfaceMethod();
            prog.LoadResources();

        }
    }
}
