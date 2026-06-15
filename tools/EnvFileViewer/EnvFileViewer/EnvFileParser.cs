using System.Security.Cryptography;

namespace EnvFileViewer;

internal class EnvFileParser
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
    private const byte ModuleCheckFlag = 0x01;
    private const byte DebugCheckFlag = 0x02;
    private const byte TimestampCheckFlag = 0x04;

    private const int MatchTypeOffset = 25;
    private const byte MatchTypeXorValue = 0x9F;
    private const int MatchValueOffset = 29;
    private const byte MatchValueXorValue = 0xD3;

    public static EnvFileModel Parse(string filePath)
    {
        var content = System.IO.File.ReadAllBytes(filePath);

        var version = GetVersion(content);

        var model = new EnvFileModel
        {
            SizeValid = ValidateSize(content),
            HeaderValid = ValidateHeader(content),
            FooterValid = ValidateFooter(content),
            SignatureValid = ValidateSignature(content),
            Version = GetVersion(content),
            ModuleCount = GetModuleCount(content),
            KeySeed = GetKeySeed(content),
            Flags = GetFeatureFlags(content),
            ModuleCheckInfo = GetModuleCheckInfo(content),
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

    private static FeatureFlags GetFeatureFlags(byte[] content)
    {
        if (content.Length < FeatureFlagsOffset + 1)
            return new FeatureFlags(false, false, false);
        byte flagsByte = (byte)(content[FeatureFlagsOffset] ^ FeatureFlagsXorValue);
        return new FeatureFlags((flagsByte & ModuleCheckFlag) != 0, (flagsByte & DebugCheckFlag) != 0, (flagsByte & TimestampCheckFlag) != 0);
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
}
