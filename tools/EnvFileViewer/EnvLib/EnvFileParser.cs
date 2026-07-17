using System;
using System.Security.Cryptography;

namespace EnvLib;

public class EnvFileParser
{
    private const int VersionMajorOffset = 10;
    private const int VersionMinorOffset = 11;
    private const int VersionPatchOffset = 12;
    private const byte VersionMajorXorValue = 0x4A;
    private const byte VersionMinorXorValue = 0xC8;
    private const byte VersionPatchXorValue = 0x1C;

    private const int ModuleCountOffset = 8;
    private const byte ModuleCountXorValue = 0xA7;

    private const int KeySeedOffset = 16 * 1024;
    private const uint KeySeedXorValue = 0x3B527B1C;

    private const int FeatureFlagsOffset = 60;
    private const byte FeatureFlagsXorValue = 0x5C;
    private const byte ModuleCheckFlag = 1 << 0;
    private const byte DebugCheckFlag = 1 << 1;
    private const byte TimestampCheckFlag = 1 << 2;
    private const byte EntryPointCheckFlag = 1 << 3;
    private const byte EnvCheckFlag = 1 << 4;
    private const byte HookCheckFlag = 1 << 5;

    private const int MatchTypeOffset = 25;
    private const byte MatchTypeXorValue = 0x9F;
    private const int MatchValueOffset = 29;
    private const byte MatchValueXorValue = 0xD3;

    private const int TimestampOffset = 30;
    private const long TimestampXorValue = 0x0A5DFB499C6830CD;

    private const int MixSeedOffset = 20;

    private const int ModuleDataOffset = 64;

    public static EnvFileModel Parse(string filePath)
    {
        var content = System.IO.File.ReadAllBytes(filePath);
        var folder = System.IO.Path.GetDirectoryName(filePath);

        var version = GetVersion(content);
        var moduleCount = GetModuleCount(content);

        var hashSeed = GetModuleHashSeedGetModuleHashSeed(content);

        var model = new EnvFileModel
        {
            SizeValid = ValidateSize(content),
            HeaderValid = ValidateHeader(content),
            FooterValid = ValidateFooter(content),
            SignatureValid = ValidateSignature(content),
            Version = version,
            ModuleCount = moduleCount,
            Modules = GetModules(folder, hashSeed, content, moduleCount),
            KeySeed = GetKeySeed(content),
            ModuleHashSeed = hashSeed,
            Flags = GetFeatureFlags(content),
            ModuleCheckInfo = GetModuleCheckInfo(content),
            TimestampUtc = GetTimestampUtc(content)
        };
        return model;
    }

    private static bool ValidateSize(byte[] content)
    {
        return content.Length == 20 * 1024;
    }

    private static bool ValidateHeader(byte[] content)
    {
        return content.Length >= 4 &&
               content[0] == 0x31 &&
               content[1] == 0x9A &&
               content[2] == 0xFF &&
               content[3] == 0xA2;
    }

    private static bool ValidateFooter(byte[] content)
    {
        return content.Length >= 4 &&
               content[^4] == 0x44 &&
               content[^3] == 0x5A &&
               content[^2] == 0x4D &&
               content[^1] == 0xAA;
    }

    private static bool ValidateSignature(byte[] content)
    {
        byte[] public_key =
        [
            0x30, 0x59, 0x30, 0x13, 0x06, 0x07, 0x2A, 0x86, 0x48, 0xCE, 0x3D, 0x02, 0x01, 0x06, 0x08, 0x2A,
            0x86, 0x48, 0xCE, 0x3D, 0x03, 0x01, 0x07, 0x03, 0x42, 0x00, 0x04, 0x5E, 0xD9, 0xCB, 0x24, 0x9E,
            0xC0, 0x43, 0x16, 0x04, 0x26, 0xCC, 0xCC, 0xE2, 0x2F, 0xE5, 0x31, 0x0D, 0x06, 0x62, 0x7D, 0xE1,
            0x8A, 0x06, 0x83, 0xEF, 0x0B, 0x4E, 0xFA, 0xEB, 0x2C, 0x7F, 0xE1, 0x69, 0xFC, 0x4B, 0x13, 0x08,
            0xD3, 0x0F, 0xC1, 0x97, 0x52, 0x99, 0x98, 0x36, 0x5D, 0x87, 0x7C, 0x32, 0x0D, 0x44, 0x2F, 0x13,
            0xEC, 0xB4, 0xE3, 0xB3, 0x66, 0x84, 0x61, 0x63, 0xF2, 0xBE, 0xF3
        ];

        ECDsa ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(public_key, out _);

        // validate the signature using the public key
        var hashAlgorithm = HashAlgorithmName.SHA256;
        return ecdsa.VerifyData(new ReadOnlySpan<byte>(content, 0, 18 * 1024), new ReadOnlySpan<byte>(content, 18 * 1024, 64), hashAlgorithm);
    }

    private static VersionInfo GetVersion(byte[] content)
    {
        int major = content[VersionMajorOffset] ^ VersionMajorXorValue;
        int minor = content[VersionMinorOffset] ^ VersionMinorXorValue;
        int patch = content[VersionPatchOffset] ^ VersionPatchXorValue;
        return new VersionInfo(major, minor, patch);
    }

    private static byte GetModuleCount(byte[] content)
    {
        return (byte)(content[ModuleCountOffset] ^ ModuleCountXorValue);
    }

    private static uint GetKeySeed(byte[] content)
    {
        if (content.Length < KeySeedOffset + 4)
            return 0;
        uint result =
            (uint)(content[KeySeedOffset + 0] << 24 |
                   content[KeySeedOffset + 1] << 16 |
                   content[KeySeedOffset + 2] << 8 |
                   content[KeySeedOffset + 3]);

        return result ^ KeySeedXorValue;
    }

    private static uint GetModuleHashSeedGetModuleHashSeed(byte[] content)
    {
        if (content.Length < MixSeedOffset + 4)
            return 0;
        uint result =
            (uint)(content[MixSeedOffset + 0] << 24 |
                   content[MixSeedOffset + 1] << 16 |
                   content[MixSeedOffset + 2] << 8 |
                   content[MixSeedOffset + 3]);
        return result;
    }

    private static DateTime? GetTimestampUtc(byte[] content)
    {
        if (content.Length < TimestampOffset + 8)
            return null;

        try
        {
            var slice = new byte[8];
            Array.Copy(content, TimestampOffset, slice, 0, 8);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(slice);

            long timestamp = BitConverter.ToInt64(slice, 0);
            timestamp ^= TimestampXorValue;
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }
        catch
        {
            return null;
        }
    }

    private static FeatureFlags GetFeatureFlags(byte[] content)
    {
        if (content.Length < FeatureFlagsOffset + 1)
            return new FeatureFlags(false, false, false, false, false, false);
        byte flagsByte = (byte)(content[FeatureFlagsOffset] ^ FeatureFlagsXorValue);
        return new FeatureFlags(
            (flagsByte & ModuleCheckFlag) != 0,
            (flagsByte & DebugCheckFlag) != 0,
            (flagsByte & TimestampCheckFlag) != 0,
            (flagsByte & EntryPointCheckFlag) != 0,
            (flagsByte & EnvCheckFlag) != 0,
            (flagsByte & HookCheckFlag) != 0);
    }

    private static ModuleCheckInfo GetModuleCheckInfo(byte[] content)
    {
        if (content.Length < MatchValueOffset + 1)
            return new ModuleCheckInfo(ModuleCheckType.AtLeast, 0);
        byte checkTypeByte = (byte)(content[MatchTypeOffset] ^ MatchTypeXorValue);
        byte checkValueByte = (byte)(content[MatchValueOffset] ^ MatchValueXorValue);
        var checkType = (ModuleCheckType)checkTypeByte;

        return new ModuleCheckInfo(checkType, checkValueByte);
    }

    private static List<ModuleInfo> GetModules(string folder, uint seed, byte[] content, int moduleCount)
    {
        var modules = new List<ModuleInfo>();
        int offset = ModuleDataOffset;
        for (int i = 0; i < moduleCount; i++)
        {
            if (offset + 64 > content.Length)
                break;
            byte[] fileNameHash = new byte[32];
            byte[] contentHash = new byte[32];
            Array.Copy(content, offset, fileNameHash, 0, 32);
            Array.Copy(content, offset + 32, contentHash, 0, 32);
            modules.Add(new ModuleInfo(fileNameHash, contentHash));
            offset += 64;
        }

        FillModuleStatus(folder, seed, modules);

        return modules;
    }

    private static void FillModuleStatus(string folder, uint seed, IEnumerable<ModuleInfo> modules)
    {
        var files = Helper.GetModuleFiles(folder);
        List<(byte[] FileNameHash, byte[] ContentHash, string FileName)> moduleHashes = new List<(byte[] FileNameHash, byte[] ContentHash, string FileName)>();
        foreach (var module in files)
        {
            var rawRelativePath = Path.GetRelativePath(folder, module).Replace("\\", "/");
            var relativeFilePath = GetModuleRelativePath(module, folder);
            if (relativeFilePath == null)
            {
                continue;
            }
            var relativePathHash = Helper.GetStringSHA256Hash(relativeFilePath, seed);
            var contentHash = Helper.GetSHA256Hash(module, seed);
            moduleHashes.Add((relativePathHash, contentHash, rawRelativePath));
        }

        foreach(var module in modules)
        {
            var matchingModule = moduleHashes.FirstOrDefault(m => m.FileNameHash.SequenceEqual(module.FileNameHash));
            if (matchingModule.FileNameHash != null)
            {
                module.FileExists = true;
                module.FileName = matchingModule.FileName;
                module.ContentMatches = matchingModule.ContentHash.SequenceEqual(module.ContentHash);
            }
            else
            {
                module.FileExists = false;
                module.ContentMatches = false;
            }
        }
    }

    private static string GetModuleRelativePath(string fullPath, string basePath)
    {
        var fullPathNormalized = Path.GetFullPath(fullPath);
        var basePathNormalized = Path.GetFullPath(basePath);

        // Ensure base path ends with separator for proper prefix matching
        if (!basePathNormalized.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.InvariantCulture))
            basePathNormalized += Path.DirectorySeparatorChar;

        // Extract relative path by removing base path prefix
        string relativePath;
        if (fullPathNormalized.Length > basePathNormalized.Length &&
            fullPathNormalized.Substring(0, basePathNormalized.Length).Equals(basePathNormalized, StringComparison.OrdinalIgnoreCase))
        {
            relativePath = fullPathNormalized.Substring(basePathNormalized.Length);
        }
        else
        {
            // Fallback: if fullPath is not under basePath, use filename
            relativePath = Path.GetFileName(fullPathNormalized);
        }

        // Replace backslashes with forward slashes
        relativePath = relativePath.Replace('\\', '/');

        // Convert to lowercase
        return relativePath.ToLower();
    }
}
