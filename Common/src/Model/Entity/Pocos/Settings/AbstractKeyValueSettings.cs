using System.Collections.Generic;

namespace ForkCommon.Model.Entity.Pocos.Settings;

public class AbstractKeyValueSettings : AbstractSettings
{
    public AbstractKeyValueSettings(string name, Dictionary<string, string> settingsDictionary) : base(name)
    {
        SettingsDictionary = settingsDictionary;
    }

    public Dictionary<string, string> SettingsDictionary { get; protected set; }
}