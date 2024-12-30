using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 상호작용이 가능하면서 동기화하도록 설정된 오브젝트 딕셔너리
    /// 키값은 포톤 뷰의 아이디임.
    /// </summary>
    private Dictionary<int, GameObject> networkInteractableMap = null;

    public Dictionary<int, GameObject> NetworkInteractableMap
    {
        get { return networkInteractableMap; }
    }

    private void Awake()
    {
        networkInteractableMap = new Dictionary<int, GameObject>();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;

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

    // 물체에 상호작용 시 해당 물체의 중력을 모든 클라이언트에서도 설정하도록 한다.
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!PhotonNetwork.IsConnected || _view == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("SetObjectGravityUsableRPC", RpcTarget.All, id, _usable);
    }

    [PunRPC]
    private void SetObjectGravityUsableRPC(int _viewId, bool _usable)
    {
        Debug.Log("SetObjectGravityUsable RPC Called!");

        networkInteractableMap[_viewId].GetComponent<Rigidbody>().useGravity = _usable;
    }
}
