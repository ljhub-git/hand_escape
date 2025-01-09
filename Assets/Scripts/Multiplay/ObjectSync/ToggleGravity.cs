using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;

public class ToggleGravity : MonoBehaviour
{
    private NetworkObjectManager _networkObjectMng = null;

    private void Awake()
    {
        _networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    public void SelectEntered(SelectEnterEventArgs _args)
    {
        _networkObjectMng.SetNetworkObjectGravityUsable(_args.interactableObject.transform.GetComponent<PhotonView>(), false);
    }

    public void SelectExited(SelectExitEventArgs _args)
    {
        _networkObjectMng.SetNetworkObjectGravityUsable(_args.interactableObject.transform.GetComponent<PhotonView>(), true);
    }
}
