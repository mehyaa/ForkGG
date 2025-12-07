namespace ForkCommon.Model.Entity.Pocos.Settings;

public abstract class AbstractSettings
{
    protected AbstractSettings(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}