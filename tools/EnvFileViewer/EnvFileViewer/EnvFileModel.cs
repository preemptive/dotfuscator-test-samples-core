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
    public FeatureFlags Flags { get; set; } = new FeatureFlags();
}

public class FeatureFlags
{
    public bool ModuleCheckEnabled { get; set; }
    public bool DebugCheckEnabled { get; set; }
    public bool TimestampCheckEnabled { get; set; }
}

public  record VersionInfo(int Major, int Minor, int Patch);