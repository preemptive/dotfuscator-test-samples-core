using System.Runtime.InteropServices;
using System.Text;

namespace EnvLib;

internal class Helper
{
    public static string[] GetModuleFiles(string outputPath)
    {
        var files = Directory.GetFiles(outputPath, "*", SearchOption.TopDirectoryOnly)
            .Where(f => f.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                     || IsAotExecutable(f)
            )
            .ToArray();

        return files;
    }

    // On Linux and macOS, AoT-compiled executables have no file extension but carry the execute bit.
    private static bool IsAotExecutable(string filePath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return false;

        if (!string.IsNullOrEmpty(Path.GetExtension(filePath)))
            return false;

        try
        {
            var mode = File.GetUnixFileMode(filePath);
            return (mode & (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0;
        }
        catch
        {
            return false;
        }
    }

    public static byte[] GetSHA256Hash(string filePath, uint seed)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        using (var fileStream = File.OpenRead(filePath))
        {
            var hash = sha256.ComputeHash(fileStream);
            Mix(hash, seed);
            return hash;
        }
    }

    public static byte[] GetStringSHA256Hash(string input, uint seed)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            Mix(hash, seed);
            return hash;
        }
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
}
