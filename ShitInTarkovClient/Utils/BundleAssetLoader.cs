using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ShitInTarkovClient.Utils;

internal static class BundleAssetLoader
{
    private static AssetBundle _assetBundle;

    public static async Task LoadAudioBundleAsync(string bundlePath)
    {
        if (_assetBundle != null)
        {
            return;
        }

        if (!File.Exists(bundlePath))
        {
            ST_Plugin.ST_Logger.LogError($"Bundle file missing: {bundlePath}");
            return;
        }

        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        while (!bundleRequest.isDone)
        {
            await Task.Yield();
        }

        _assetBundle = bundleRequest.assetBundle;

        if (_assetBundle == null)
        {
            ST_Plugin.ST_Logger.LogError("Failed to unload AssetBundle pointer.");
        }
    }

    public static AudioClip GetAudioClip(string clipName)
    {
        if (_assetBundle == null)
        {
            ST_Plugin.ST_Logger.LogError("Audio bundle is not loaded!");
            return null;
        }

        var clip = _assetBundle.LoadAsset<AudioClip>(clipName);
        if (clip == null)
        {
            ST_Plugin.ST_Logger.LogError($"Clip '{clipName}' not found in bundle.");
        }

        return clip;
    }

    public static Texture2D GetTexture(string assetName)
    {
        if (_assetBundle == null)
        {
            ST_Plugin.ST_Logger.LogError("Cannot load Texture2D: AssetBundle is null.");
            return null;
        }

        var texture = _assetBundle.LoadAsset<Texture2D>(assetName);
        if (texture == null)
        {
            ST_Plugin.ST_Logger.LogError($"Texture2D '{assetName}' not found in bundle.");
        }

        return texture;
    }

    public static Sprite GetSprite(string assetName)
    {
        var tex = GetTexture(assetName);
        if (tex == null)
        {
            return null;
        }

        return Sprite.Create(tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), 100.0f);
    }
}
