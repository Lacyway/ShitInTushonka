using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShitInTarkovClient.Utils;

public static class SoundManager
{
    private static readonly Dictionary<AudioClipType, string> _soundKeyMap = new()
    {
        { AudioClipType.Poop1, "poop_1.ogg" },
        { AudioClipType.Poop2, "poop_2.ogg" },
        { AudioClipType.Poop3, "poop_3.ogg" },
        { AudioClipType.Poop4, "poop_4.ogg" },
        { AudioClipType.Poop5, "poop_5.ogg" }
    };

    private static readonly AudioClipType[] _audioClipTypes = (AudioClipType[])Enum.GetValues(typeof(AudioClipType));

    /// <summary>
    /// Fetches the requested AudioClip directly from the loaded AssetBundle.
    /// </summary>
    public static AudioClip GetSound(AudioClipType type)
    {
        if (!_soundKeyMap.TryGetValue(type, out var key))
        {
            ST_Plugin.ST_Logger.LogError($"AudioClipType '{type}' is not mapped to a valid bundle key.");
            return null;
        }

        var clip = BundleAssetLoader.GetAudioClip(key);

        if (clip == null || Mathf.Approximately(clip.length, 0f))
        {
            ST_Plugin.ST_Logger.LogError($"Failed to retrieve valid audio clip for key '{key}'.");
            return null;
        }

        return clip;
    }

    /// <summary>
    /// Fetches a random sound effect key
    /// </summary>
    /// <returns>The sound effect key</returns>
    public static AudioClipType GetRandomSound()
    {
        var values = _audioClipTypes;
        return values[UnityEngine.Random.Range(0, values.Length)];
    }
}

public enum AudioClipType : byte
{
    Poop1,
    Poop2,
    Poop3,
    Poop4,
    Poop5
}