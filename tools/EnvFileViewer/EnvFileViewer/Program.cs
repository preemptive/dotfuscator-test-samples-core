using EnvFileViewer;

var noWait = args.Contains("--no-wait");
var filePath = args.FirstOrDefault(a => a != "--no-wait");

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("Usage: EnvFileViewer <file-path> [--no-wait]");
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
