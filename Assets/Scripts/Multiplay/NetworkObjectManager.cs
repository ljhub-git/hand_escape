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
    private Dictionary<int, GameObject> networkObjectMap = null;

    public Dictionary<int, GameObject> NetworkObjectMap
    {
        get { return networkObjectMap; }
    }

    private void Awake()
    {
        networkObjectMap = new Dictionary<int, GameObject>();
    }

    private void Start()
    {
        // 씬에 있는 포톤 뷰를 가져오고 포톤뷰 아이디를 통해서 networkInteractableMap 을 초기화한다.
        {
            // 포톤 서버에 접속이 되어있어야 함.
            if (!PhotonNetwork.IsConnected)
                return;

            // PhotonView 컴포넌트를 가진 오브젝트들을 모두 가져온다.
            PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

            foreach (var view in photonViews)
            {
                networkObjectMap.Add(view.ViewID, view.gameObject);
            }
        }


    }

    // 해당 물체의 중력을 모든 클라이언트에서도 설정하도록 한다.
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!PhotonNetwork.IsConnected || _view == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetGravity", RpcTarget.Others, id, _usable);
    }

    #region RPC

    [PunRPC]
    private void RPC_SetGravity(int _viewId, bool _usable)
    {
        Debug.Log("SetObjectGravityUsable RPC Called!");

        networkObjectMap[_viewId].GetComponent<Rigidbody>().useGravity = _usable;
    }

    [PunRPC]
    private void RPC_SetRotation(int _viewId, Quaternion _rot)
    {
        Debug.Log("SetObjectGravityUsable RPC Called!");

        networkObjectMap[_viewId].transform.rotation = _rot;
    }

    #endregion
}
