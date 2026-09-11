using UnityEngine;

namespace ShitInTarkovClient.Utils;

public static class DecalManager
{
    private static readonly string[] _spriteKeys =
    [
        "stain_1.png",
        "stain_2.png"
    ];

    private static Material _spriteMaterial;

    /// <summary>
    /// Spawns a Quad aligned to the collision hit surface using a Sprite from the bundle.
    /// </summary>
    public static void SpawnDecalAt(Vector3 hitPoint, Vector3 hitNormal, float size = 0.5f)
    {
        var randomKey = _spriteKeys[Random.Range(0, _spriteKeys.Length)];
        var sprite = BundleAssetLoader.GetSprite(randomKey);

        if (sprite == null)
        {
            ST_Plugin.ST_Logger.LogError($"Failed to load decal sprite: {randomKey}");
            return;
        }

        var decal = GameObject.CreatePrimitive(PrimitiveType.Quad);
        decal.name = "PoopDecal";

        Object.Destroy(decal.GetComponent<Collider>());

        decal.transform.position = hitPoint + (hitNormal * 0.005f);
        decal.transform.rotation = Quaternion.LookRotation(-hitNormal);

        var randomAngle = Random.Range(0f, 360f);
        decal.transform.Rotate(0f, 0f, randomAngle, Space.Self);

        decal.transform.localScale = new Vector3(size, size, 1f);

        if (_spriteMaterial == null)
        {
            var shader = Shader.Find("Sprites/Default") ?? Shader.Find("Legacy Shaders/Transparent/Cutout/Diffuse");
            _spriteMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }

        var renderer = decal.GetComponent<MeshRenderer>();

        var propBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetTexture("_MainTex", sprite.texture);
        renderer.SetPropertyBlock(propBlock);

        renderer.sharedMaterial = _spriteMaterial;

        Object.Destroy(decal, 30f);
    }
}