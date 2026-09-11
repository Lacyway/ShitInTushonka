using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using Diz.Jobs;
using EFT;
using EFT.InventoryLogic;
using GPUInstancer;
using ShitInTarkovClient.Models;
using ShitInTarkovClient.Utils;
using UnityDiagnostics;
using UnityEngine;
using Random = System.Random;

namespace ShitInTarkovClient.Components;

public sealed class ShitSimulator : MonoBehaviour
{
    public static event Action<ShitPacketEventArgs> OnShitHappened;

    private static readonly MongoID[] _shitPresets = ["6a9eff3a944cbaa6ea9dc46b", "6a9eff3c705e865c6d160dc1", "6a9eff3e3f5a25fe1bc3d647"];
    private static readonly Random _random = new();

    private const float _maxShitVelocity = 0.5f;
    private const float _minShitVelocity = 0.1f;

    private CancellationTokenSource _cts;
    private Player _player;
    private float _shitTimer;
    private float _shitThreshold = 600f;
    private const float _height = 0.5f;

    private void Awake()
    {
        var player = GetComponent<Player>();
        if (player == null)
        {
            ST_Plugin.ST_Logger.LogError("Could not find player");
            return;
        }
        _player = player;
        _cts = new();
    }

    private void Update()
    {
        _shitTimer += Time.deltaTime;
        if (_shitTimer >= _shitThreshold)
        {
            _shitTimer -= _shitThreshold;
            _ = DoShit(_cts.Token);
            _shitThreshold = _random.Range(480f, 900f);
        }
    }

    private async ValueTask DoShit(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return;
        }

        var itemFactory = Singleton<ItemFactory>.Instance;
        if (itemFactory == null)
        {
            ST_Plugin.ST_Logger.LogError("ItemFactory was null!");
            return;
        }

        var index = _random.Next(_shitPresets.Length);
        ST_Plugin.ST_Logger.LogInfo("Shitting " + index);

        var templateId = _shitPresets[index];
        var item = itemFactory.GetPresetItem(templateId);

        List<ResourceKey> collection = [];
        foreach (var subItem in item.GetAllItems())
        {
            collection.AddRange(subItem.Template.AllResources);
        }
        await Singleton<ObjectsFactory>.Instance.LoadBundlesAndCreatePools(ObjectsFactory.PoolsCategory.Raid, ObjectsFactory.AssemblyType.Online,
            [.. collection], JobYieldPriority.Immediate, null, ct);

        if (ct.IsCancellationRequested || _player == null || _player.GameWorld == null)
        {
            return;
        }

        _player.AddStateSpeedLimit(0.25f, Player.ESpeedLimit.Swamp);

        var forwardVector = Quaternion.Euler(Mathf.Clamp(_player.Rotation.y, -90f, 45f), _player.Rotation.x, 0f) * new Vector3(0f, 1f, 1f);
        var reverseVector = -forwardVector * 2f;

        var position = _player.PlayerColliderPointOnCenterAxis(_height) + (_player.Velocity * Time.deltaTime);

        var speed = UnityEngine.Random.Range(_minShitVelocity, _maxShitVelocity);
        var rotation = _player.PlayerBones.WeaponRoot.rotation * Quaternion.Euler(90f, 0f, 0f);
        var velocity = (reverseVector * speed) + (_player.Velocity / 2f);
        var angularVelocity = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f),
            2f * Mathf.Sign((float)UnityEngine.Random.Range(-1, 2)));

        AudioClipType clipType;
        try
        {
            clipType = PlaySound(position);
        }
        catch (Exception ex)
        {
            ST_Plugin.ST_Logger.LogError(ex);
            throw;
        }
        OnShitHappened?.Invoke(new ShitPacketEventArgs(_player, item, position, rotation, velocity, angularVelocity, clipType));

        await Task.Delay(TimeSpan.FromSeconds(0.5d));

        var lootItem = _player.GameWorld.ThrowItem(item, _player, position, rotation, velocity, angularVelocity, true, true,
            EFTHardSettings.Instance.ThrowLootMakeVisibleDelay);

        await Task.Delay(TimeSpan.FromSeconds(0.1d));
        lootItem.gameObject.AddComponent<PoopCollisionHandler>();

        await Task.Delay(TimeSpan.FromSeconds(5d));
        _player.RemoveStateSpeedLimit(Player.ESpeedLimit.Swamp);
    }

    public static async ValueTask ReplicatedShit(Player player, Item item, Vector3 position,
        Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, AudioClipType clipType)
    {
        List<ResourceKey> collection = [];
        foreach (var subItem in item.GetAllItems())
        {
            collection.AddRange(subItem.Template.AllResources);
        }
        await Singleton<ObjectsFactory>.Instance.LoadBundlesAndCreatePools(ObjectsFactory.PoolsCategory.Raid, ObjectsFactory.AssemblyType.Online,
            [.. collection], JobYieldPriority.Immediate, null, default);

        var clip = SoundManager.GetSound(clipType);
        if (clip != null)
        {
            PlaySoundAtPoint(position, clip);
        }
        else
        {
            ST_Plugin.ST_Logger.LogError("Could not find clip");
        }
        await Task.Delay(TimeSpan.FromSeconds(0.5d));

        var lootItem = Singleton<GameWorld>.Instance.ThrowItem(item, player, position, rotation, velocity,
            angularVelocity, true, true,
            EFTHardSettings.Instance.ThrowLootMakeVisibleDelay);

        await Task.Delay(TimeSpan.FromSeconds(0.1d));
        lootItem.gameObject.AddComponent<PoopCollisionHandler>();
    }

    private AudioClipType PlaySound(Vector3 position)
    {
        var clipType = SoundManager.GetRandomSound();
        var clip = SoundManager.GetSound(clipType);

        PlaySoundAtPoint(position, clip);
        return clipType;
    }

    private static void PlaySoundAtPoint(Vector3 position, AudioClip clip)
    {
        var betterAudio = MonoBehaviourSingleton<BetterAudio>.Instance;
        if (betterAudio == null)
        {
            throw new NullReferenceException("BetterAudio was null when playing sound effect");
        }
        betterAudio.PlayAtPoint(position, clip, BetterAudio.AudioSourceGroupType.Character, 50, 2f);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public async ValueTask SimulateLactoseIntolerance()
    {
        _shitTimer = 0f;
        var shitsToTake = (int)_random.Range(2f, 4f);

        await Task.Delay(TimeSpan.FromSeconds(2d));

        for (var i = 0; i < shitsToTake; i++)
        {
            await DoShit(_cts.Token);
        }
    }

    public void IncreaseShitMeter(float value)
    {
#if DEBUG
        ST_Plugin.ST_Logger.LogInfo("Increasing timer by " + value);
#endif
        _shitTimer += value;
    }
}
