using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;

namespace ShitInTarkov;

public sealed record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "lcw.lacyway.sit";
    public string Name { get; init; } = "ShitInTarkov";
    public string Author { get; init; } = "Lacyway";
    public List<string>? Contributors { get; init; } = ["GrooveypenguinX"];
    public SemanticVersioning.Version Version { get; init; } = new(typeof(ModMetadata).Assembly.GetName().Version?.ToString(3));
    public Range SptVersion { get; init; } = new(">=4.1.3");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range(">=3.0.6") }
    };
    public string? Url { get; init; }
    public bool HasPrepatcher { get; init; } = false;
    public string License { get; init; } = "MIT";
}


[Injectable(TypePriority = OnLoadOrder.Preload + 2)]
public sealed class Mod(WTTServerCommonLib.WTTServerCommonLib wttCommon) : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
    }
}
