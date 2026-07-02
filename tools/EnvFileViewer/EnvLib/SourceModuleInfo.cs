namespace EnvLib;

public class SourceModuleInfo
{
    public string FilePath { get; set; }
    public byte[] FileNameHash { get; set; }
    public byte[] ContentHash { get; set; }
}