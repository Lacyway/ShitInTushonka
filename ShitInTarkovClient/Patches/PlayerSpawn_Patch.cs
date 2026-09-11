using System.Reflection;
using EFT;
using ShitInTarkovClient.Components;
using SPT.Reflection.Patching;

namespace ShitInTarkovClient.Patches;

public sealed class PlayerSpawn_Patch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(Player)
            .GetMethod(nameof(Player.InitAudioController));
    }

    [PatchPostfix]
    public static void Postfix(Player __instance)
    {
        if (__instance.IsYourPlayer)
        {
            __instance.gameObject.AddComponent<ShitSimulator>();
        }
    }
}
