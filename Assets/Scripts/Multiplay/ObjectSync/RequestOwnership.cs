using UnityEngine;

using Photon.Pun;

public class RequestOwnership : MonoBehaviour
{
    NetworkObjectManager networkObjectMng = null;

    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    public void GetOwnership()
    {
        if (networkObjectMng != null)
            networkObjectMng.RequestOwnership(GetComponent<PhotonView>());
    }
}
