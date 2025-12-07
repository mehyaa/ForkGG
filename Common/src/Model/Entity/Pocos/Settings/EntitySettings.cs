using System.Collections.Generic;
using ForkCommon.Model.Entity.Pocos.Automation;

namespace ForkCommon.Model.Entity.Pocos.Settings;

public class EntitySettings() : AbstractSettings("General")
{
    public ulong Id { get; set; }
    public ServerVersion? ServerVersion { get; set; }
    public JavaSettings JavaSettings { get; set; } = new();
    public int ServerIconId { get; set; }
    public string? EntityName { get; set; }
    public bool StartWithFork { get; set; }
    public List<AutomationTime> AutomationTimes { get; set; } = [];
}