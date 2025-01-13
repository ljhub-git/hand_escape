using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

public class ToggleTransformSync : MonoBehaviourPun
{
    // public UnityEvent OnTransformSync;

    private NetworkObjectManager _networkObjectMng = null;

    private void Awake()
    {
        _networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    public void TurnOnTransformSync()
    {
        StartCoroutine(TransformSyncCoroutine());
    }

    public void TurnOffTransformSync()
    {
        StopAllCoroutines();

        _networkObjectMng.SetObjectTransform(photonView, transform);
    }

    private IEnumerator TransformSyncCoroutine()
    {
        if(_networkObjectMng == null)
        {
            Debug.LogWarning("networkObjectManager is null!");
            yield break;
        }

        while(true)
        {
            _networkObjectMng.SetObjectTransform(photonView, transform);

            yield return null;
        }
    }

    
}
