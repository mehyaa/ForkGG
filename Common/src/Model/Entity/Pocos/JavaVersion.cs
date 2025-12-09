namespace ForkCommon.Model.Entity.Pocos;

public class JavaVersion
{
    public string? Version { get; set; }
    public int VersionComputed { get; set; }
    public bool Is64Bit { get; set; }
    public string? JavaPath { get; set; }
}