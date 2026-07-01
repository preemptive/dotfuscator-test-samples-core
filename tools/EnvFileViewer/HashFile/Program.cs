using EnvLib;
using System.ComponentModel;

const string EnvFileName = "environment0f617e02.bin";

var noWait = args.Contains("--no-wait");
var filePath = args.FirstOrDefault(a => a != "--no-wait");

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("Usage: HashFile <file-path> [--no-wait]");
    return;
}

var loweredFileName = Path.GetFileName(filePath).ToLowerInvariant();


byte[] loweredFileNameHash = null;
using (var sha256 = System.Security.Cryptography.SHA256.Create())
{
    loweredFileNameHash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loweredFileName));
}

var loweredFileNameHashString = BitConverter.ToString(loweredFileNameHash).Replace("-", "").ToLowerInvariant();
Console.WriteLine($"SHA256 hash of lowered file name {loweredFileName}: {loweredFileNameHashString}");

var envFilePath = Path.Combine(Path.GetDirectoryName(filePath), EnvFileName);
byte[] hash = null;

// Hash the content of filePath using SHA256
using (var sha256 = System.Security.Cryptography.SHA256.Create())
using (var fileStream = File.OpenRead(filePath))
{
    hash = sha256.ComputeHash(fileStream);
}
var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
Console.WriteLine($"SHA256 hash of file content: {hashString}");

if (File.Exists(envFilePath))
{
    var env = EnvFileParser.Parse(envFilePath);

    var mixedFileNameHash = (byte[])loweredFileNameHash.Clone();
    Mix(mixedFileNameHash, env.ModuleHashSeed);
    var mixedFileNameHashString = BitConverter.ToString(mixedFileNameHash).Replace("-", "").ToLowerInvariant();
    Console.WriteLine($"Mixed SHA256 hash of lowered file name {loweredFileName}: {mixedFileNameHashString}");

    var mixedContentHash = (byte[])hash.Clone();
    Mix(mixedContentHash, env.ModuleHashSeed);
    var mixedHashString = BitConverter.ToString(mixedContentHash).Replace("-", "").ToLowerInvariant();
    Console.WriteLine($"Mixed SHA256 hash of file content: {mixedHashString}");

    var module = env.Modules.FirstOrDefault(m => m.FileNameHash.SequenceEqual(mixedFileNameHash));
    Console.WriteLine("Env file contains module name hash: " + (module != null ? "Yes" : "No"));
    var moduleContentMatch = module != null && module.ContentHash.SequenceEqual(mixedContentHash);
    Console.WriteLine("Env file content hash matches: " + (moduleContentMatch ? "Yes" : "No"));
}

if (!noWait)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
}

static void Mix(byte[] data, uint seed)
{
    uint s = seed;
    for (int i = 0; i < data.Length; i++)
    {
        s ^= s << 13;
        s ^= s >> 17;
        s ^= s << 5;
        data[i] ^= (byte)(s & 0xFF);
    }
}