using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using Fika.Core.Main.Components;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using Fika.Core.Networking.LiteNetLib;
using ShitInTarkovClient.Components;
using ShitInTarkovClient.Models;
using ShitInTarkovFika.Packets;

namespace ShitInTarkovFika;

[BepInPlugin("lcw.lacyway.stf", "ShitInTarkovFika", PluginVersion)]
public sealed class STF_Plugin : BaseUnityPlugin
{
    public const string PluginVersion = "1.0.0";

    internal static ManualLogSource STF_Logger;

    private void Awake()
    {
        STF_Logger = Logger;
        STF_Logger.LogInfo($"{nameof(STF_Plugin)} has been loaded.");

        FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(OnNetworkManagerCreated);
        ShitSimulator.OnShitHappened += ShitSimulator_OnShitHappened;
    }

    private void ShitSimulator_OnShitHappened(ShitPacketEventArgs obj)
    {
        var manager = Singleton<IFikaNetworkManager>.Instance;
        if (manager != null)
        {
            var packet = new ShitPacket(obj);
            manager.SendData(ref packet, DeliveryMethod.ReliableOrdered, true);
        }
    }

    private void OnNetworkManagerCreated(FikaNetworkManagerCreatedEvent createdEvent)
    {
        createdEvent.Manager.RegisterPacket<ShitPacket>(OnShitPacketReceived);
    }

    private void OnShitPacketReceived(ShitPacket packet)
    {
#if DEBUG
        STF_Logger.LogWarning("Received ShitPacket from " + packet.NetId);
#endif
        if (!CoopHandler.TryGetCoopHandler(out var handler))
        {
            STF_Logger.LogError("There was no coop handler");
            return;
        }

        if (!handler.Players.TryGetValue(packet.NetId, out var player))
        {
            STF_Logger.LogError("There was no player with id " + packet.NetId);
        }

        _ = ShitSimulator.ReplicatedShit(player, packet.Item,
            packet.Position, packet.Rotation, packet.Velocity,
            packet.AngularVelocity, packet.AudioClipType);
    }
}
