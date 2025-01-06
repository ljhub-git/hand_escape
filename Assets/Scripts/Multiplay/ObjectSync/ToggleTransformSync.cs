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
    }

    private IEnumerator TransformSyncCoroutine()
    {
        while(true)
        {
            // OnTransformSync.Invoke();

            _networkObjectMng.SetObjectTransform(photonView, transform);

            yield return null;
        }
    }
}
