

using PreEmptive.Dotfuscator.TestSamples.MAUI.Models;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private FileStream _lockedStream;
        private string _filePath;

        public MainPage()
        {
            InitializeComponent();
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "testfile.txt");
            
        }

        private void OnLockFileClicked(object sender, EventArgs e)
        {
            BaseProcessor processor = new TextProcessor();
            string description = processor.Describe();

            string staticType = StaticTextProcessor.ProcessorType();

            DisplayAlert("Info", $"{description}\nStatic: {staticType}", "OK");
            try
            {
                _lockedStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                using (var writer = new StreamWriter(_lockedStream))
                {
                    writer.WriteLine("File is locked and writer not disposed.");
                    writer.Flush();
                }
                DisplayAlert("Success", "File locked and writer not disposed.", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to lock file: {ex.Message}", "OK");
            }
        }

        private async void OnWriteFileClicked(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    int taskId = i;
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            using (var stream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            using (var writer = new StreamWriter(stream))
                            {
                                writer.WriteLine($"Concurrent write from Task {taskId}");
                                writer.Flush();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Task {taskId} failed: {ex.Message}");
                        }
                    });
                }

                await DisplayAlert("Concurrent Write", "Multiple tasks launched to write the file.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Concurrent write failed: {ex.Message}", "OK");
            }
        }
    }
}
