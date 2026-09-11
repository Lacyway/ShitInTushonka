using ShitInTarkovClient.Utils;
using UnityEngine;

namespace ShitInTarkovClient.Components;

public sealed class PoopCollisionHandler : MonoBehaviour
{
    private bool _hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided)
        {
            return;
        }

        _hasCollided = true;

#if DEBUG
        ST_Plugin.ST_Logger.LogInfo("Collided");
#endif

        var contact = collision.contacts[0];

        DecalManager.SpawnDecalAt(contact.point, contact.normal, size: 0.6f);
        Destroy(this);
    }
}