using EFT.InventoryLogic;
using Fika.Core.Main.Players;
using Fika.Core.Networking;
using Fika.Core.Networking.LiteNetLib.Utils;
using ShitInTarkovClient.Models;
using ShitInTarkovClient.Utils;
using UnityEngine;

namespace ShitInTarkovFika.Packets;

public struct ShitPacket : INetSerializable
{
    public ShitPacket(ShitPacketEventArgs args)
    {
        NetId = (args.Player as FikaPlayer).NetId;
        Item = args.Item;
        Position = args.Position;
        Rotation = args.Rotation;
        Velocity = args.Velocity;
        AngularVelocity = args.AngularVelocity;
        AudioClipType = args.ClipType;
    }

    public int NetId;
    public Item Item;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Velocity;
    public Vector3 AngularVelocity;
    public AudioClipType AudioClipType;

    public readonly void Serialize(NetDataWriter writer)
    {
        writer.Put(NetId);
        writer.PutItem(Item);
        writer.PutUnmanaged(Position);
        writer.PutUnmanaged(Rotation);
        writer.PutUnmanaged(Velocity);
        writer.PutUnmanaged(AngularVelocity);
        writer.PutEnum(AudioClipType);
    }

    public void Deserialize(NetDataReader reader)
    {
        NetId = reader.GetInt();
        Item = reader.GetItem();
        Position = reader.GetUnmanaged<Vector3>();
        Rotation = reader.GetUnmanaged<Quaternion>();
        Velocity = reader.GetUnmanaged<Vector3>();
        AngularVelocity = reader.GetUnmanaged<Vector3>();
        AudioClipType = reader.GetEnum<AudioClipType>();
    }
}
