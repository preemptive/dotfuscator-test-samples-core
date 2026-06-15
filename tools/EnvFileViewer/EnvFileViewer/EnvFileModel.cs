using System.Text.Json.Serialization;

namespace EnvFileViewer;

public class EnvFileModel
{
    public bool SizeValid { get; set; }
    public bool HeaderValid { get; set; }
    public bool FooterValid { get; set; }
    public bool SignatureValid { get; set; }

    public int ModuleCount { get; set; }
    public uint KeySeed { get; set; }

    public VersionInfo Version { get; set; }
    public FeatureFlags Flags { get; set; } = new FeatureFlags(false, false, false);
    public ModuleCheckInfo ModuleCheckInfo { get; set; } = new ModuleCheckInfo(ModuleCheckType.AtLeast, 0);
}

public record FeatureFlags(bool ModuleCheckEnabled, bool DebugCheckEnabled, bool TimestampCheckEnabled);

public record ModuleCheckInfo(ModuleCheckType CheckType, byte CheckValue);

public record VersionInfo(int Major, int Minor, int Patch);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModuleCheckType : byte
{
    All = 1,
    AtLeast = 2
}