using EnvFileViewer;
using EnvLib;

if (args.Contains("--register"))
{
    var applicationPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    if (applicationPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
    {
        var exePath = Path.Combine(Path.GetDirectoryName(applicationPath) ?? string.Empty, Path.GetFileNameWithoutExtension(applicationPath) + ".exe");
        if (File.Exists(exePath))
        {
            applicationPath = exePath;
        }
    }
    FileAssociationManager.RegisterPreviewAction(applicationPath);
    return;
}
if (args.Contains("--unregister"))
{
    FileAssociationManager.UnregisterPreviewAction();
    return;
}

var noWait = args.Contains("--no-wait");
var filePath = args.FirstOrDefault(a => a != "--no-wait");

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("Usage: EnvFileViewer <file-path> [--no-wait]");
    Console.WriteLine("  EnvFileViewer --register    Register the application as a preview handler for .bin files");
    Console.WriteLine("  EnvFileViewer --unregister  Unregister the application as a preview handler for .bin files");
    return;
}

var result = EnvFileParser.Parse(filePath);
var resultText = System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

Console.WriteLine(resultText);

if (!noWait)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
}
