using System.IO;
using BepInEx;
using BepInEx.Logging;
using ShitInTarkovClient.Patches;
using ShitInTarkovClient.Utils;

namespace ShitInTarkovClient;

[BepInPlugin("lcw.lacyway.sit", "ShitInTarkov", PluginVersion)]
public sealed class ST_Plugin : BaseUnityPlugin
{
    public const string PluginVersion = "1.0.1";
    public static readonly string AssetsDirectory = Path.Combine(BepInEx.Paths.PluginPath, @"ShitInTarkovClient\Assets\");

    internal static ManualLogSource ST_Logger;

    private void Awake()
    {
        ST_Logger = Logger;
        ST_Logger.LogInfo($"{nameof(ST_Plugin)} has been loaded.");

        new EatAndDrink_Patch().Enable();
        new PlayerSpawn_Patch().Enable();

        _ = BundleAssetLoader.LoadAudioBundleAsync(Path.Combine(AssetsDirectory, "sit.bundle"));
    }
}
