using EFT;
using EFT.InventoryLogic;
using ShitInTarkovClient.Utils;
using UnityEngine;

namespace ShitInTarkovClient.Models;

public readonly struct ShitPacketEventArgs(Player player, Item item, Vector3 position,
    Quaternion rotation, Vector3 velocity,
    Vector3 angularVelocity, AudioClipType clipType)
{
    public readonly Player Player { get; } = player;
    public readonly Item Item { get; } = item;
    public readonly Vector3 Position { get; } = position;
    public readonly Quaternion Rotation { get; } = rotation;
    public readonly Vector3 Velocity { get; } = velocity;
    public readonly Vector3 AngularVelocity { get; } = angularVelocity;
    public readonly AudioClipType ClipType { get; } = clipType;
}