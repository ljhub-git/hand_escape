using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 상호작용이 가능하면서 동기화하도록 설정된 오브젝트 딕셔너리.
    /// 키값은 포톤 뷰의 아이디.
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
        // 씬에 있는 상호작용 가능 물체들을 가져오고, 해당 물체들의 포톤뷰 아이디를 통해서 networkInteractableMap 을 초기화한다.
        {
            // 포톤 서버에 접속이 되어있어야 함.
            if (!PhotonNetwork.IsConnected)
                return;

            // XRGrabInteractable 컴포넌트를 가진 오브젝트들을 모두 가져온다.
            XRGrabInteractable[] grabInteractables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);

            foreach (var interactable in grabInteractables)
            {
                // XRGrabInteractable 컴포넌트를 가진 오브젝트들의 위치를 동기화하기로 함.
                // 따라서 해당 오브젝트들은 포톤 뷰 컴포넌트를 가지고 있어야만 한다.
                if (interactable.GetComponent<PhotonView>() == null)
                {
                    Debug.LogWarning("XR Interactable Object must have Photon View in Our Project!");
                    break;
                }

                networkInteractableMap.Add(interactable.GetComponent<PhotonView>().ViewID, interactable.gameObject);
            }
        }


    }

    // 물체에 상호작용 시 해당 물체의 중력을 모든 클라이언트에서도 설정하도록 한다.
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!PhotonNetwork.IsConnected || _view == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetObjectGravityUsable", RpcTarget.All, id, _usable);
    }

    [PunRPC]
    private void RPC_SetObjectGravityUsable(int _viewId, bool _usable)
    {
        Debug.Log("SetObjectGravityUsable RPC Called!");

        networkInteractableMap[_viewId].GetComponent<Rigidbody>().useGravity = _usable;
    }
}
