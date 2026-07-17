using System.Text.Json.Serialization;

namespace EnvLib;

public class EnvFileModel
{
    public bool SizeValid { get; set; }
    public bool HeaderValid { get; set; }
    public bool FooterValid { get; set; }
    public bool SignatureValid { get; set; }

    public int ModuleCount { get; set; }
    public uint KeySeed { get; set; }

    public VersionInfo Version { get; set; }
    public FeatureFlags Flags { get; set; } = new FeatureFlags(false, false, false, false, false, false);
    public ModuleCheckInfo ModuleCheckInfo { get; set; } = new ModuleCheckInfo(ModuleCheckType.AtLeast, 0);
    public DateTime? TimestampUtc { get; set; }
    public uint ModuleHashSeed { get; internal set; }
    public List<ModuleInfo> Modules { get; set; } = new List<ModuleInfo>();
}

public record FeatureFlags(bool ModuleCheckEnabled, bool DebugCheckEnabled, bool TimestampCheckEnabled,
                           bool EntryPointCheckEnabled, bool EnvCheckEnabled, bool HookCheckEnabled);

public record ModuleCheckInfo(ModuleCheckType CheckType, byte CheckValue);

public record VersionInfo(int Major, int Minor, int Patch);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModuleCheckType : byte
{
    All = 1,
    AtLeast = 2
}

public class ModuleInfo
{
    [JsonIgnore]
    public byte[] FileNameHash { get; }
    [JsonIgnore]
    public byte[] ContentHash { get; }

    public bool FileExists { get; set; }
    public bool ContentMatches { get; set; }
    public string? FileName { get; set; }


    public string FileNameHashHex => BitConverter.ToString(FileNameHash).Replace("-", "").ToLowerInvariant();
    public string ContentHashHex => BitConverter.ToString(ContentHash).Replace("-", "").ToLowerInvariant();
    public ModuleInfo(byte[] fileNameHash, byte[] contentHash)
    {
        FileNameHash = fileNameHash;
        ContentHash = contentHash;
    }
}