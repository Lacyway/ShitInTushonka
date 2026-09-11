using System.Reflection;
using EFT;
using ShitInTarkovClient.Components;
using SPT.Reflection.Patching;

namespace ShitInTarkovClient.Patches;

public sealed class EatAndDrink_Patch : ModulePatch
{
    private static readonly MongoID[] _milkBasedTpls = ["575146b724597720a27126d5", "5734773724597737fd047c14", "57505f6224597709a92585a9", "544fb6cc4bdc2d34748b456e"];

    protected override MethodBase GetTargetMethod()
    {
        return typeof(Player.MedsController)
            .GetMethod(nameof(Player.MedsController.Destroy));
    }

    [PatchPrefix]
    public static void Prefix(Player.MedsController __instance)
    {
        if (!__instance._player.IsYourPlayer || !__instance._player.gameObject.TryGetComponent<ShitSimulator>(out var shitSim))
        {
            return;
        }

        foreach (var tpl in _milkBasedTpls)
        {
            if (__instance.Item.TemplateId == tpl)
            {
#if DEBUG
                ST_Plugin.ST_Logger.LogInfo("Found template match");
#endif
#if DEBUG
                ST_Plugin.ST_Logger.LogInfo("We drank a milk based item. Time to shit");
#endif
                _ = shitSim.SimulateLactoseIntolerance();
                return;
            }
        }

#if DEBUG
        ST_Plugin.ST_Logger.LogInfo("Did not match milk based items, was: " + __instance.Item.TemplateId);
#endif

        var value = UnityEngine.Random.Range(45f, 90f);
        shitSim.IncreaseShitMeter(value);
    }
}