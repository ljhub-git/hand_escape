using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPunCallbacks
{
    private Dictionary<int, GameObject> networkInteractableMap = null;

    private void Awake()
    {
        networkInteractableMap = new Dictionary<int, GameObject>();
    }

    private void Start()
    {
        XRGrabInteractable[] grabInteractables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);

        foreach(var interactable in grabInteractables)
        {
            if (interactable.GetComponent<PhotonView>() == null)
            {
                Debug.LogWarning("XR Interactable Object must have Photon View in Our Project!");
                break;
            }

            networkInteractableMap.Add(interactable.GetComponent<PhotonView>().ViewID, interactable.gameObject);
        }
    }

    public void OnInteractSelectEntered(SelectEnterEventArgs selectEnterEvent)
    {
        Debug.Log("Enter");

        if(selectEnterEvent.interactableObject.transform.GetComponent<PhotonView>() == null)
        {
            Debug.LogWarning("XR Interactable Object must have Photon View in Our Project!");
            return;
        }

        int viewId = selectEnterEvent.interactableObject.transform.GetComponent<PhotonView>().ViewID;

        photonView.RPC("SetObjectGravityUsable", RpcTarget.All, viewId, false);
    }

    public void OnInteractSelectExited(SelectExitEventArgs selectExitEvent)
    {
        Debug.Log("Exit");

        if (selectExitEvent.interactableObject.transform.GetComponent<PhotonView>() == null)
        {
            Debug.LogWarning("XR Interactable Object must have Photon View in Our Project!");
            return;
        }

        int viewId = selectExitEvent.interactableObject.transform.GetComponent<PhotonView>().ViewID;

        photonView.RPC("SetObjectGravityUsable", RpcTarget.All, viewId, true);
    }

    [PunRPC]
    public void SetObjectGravityUsable(int _viewId, bool _usable)
    {
        Debug.Log("SetObjectGravityUsable RPC Called!");

        networkInteractableMap[_viewId].GetComponent<Rigidbody>().useGravity = _usable;
    }
}
