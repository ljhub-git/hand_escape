using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// ��ȣ�ۿ��� �����ϸ鼭 ����ȭ�ϵ��� ������ ������Ʈ ��ųʸ�.
    /// Ű���� ���� ���� ���̵�.
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
        // ���� �ִ� ���� �並 �������� ����� ���̵� ���ؼ� networkInteractableMap �� �ʱ�ȭ�Ѵ�.
        {
            // ���� ������ ������ �Ǿ��־�� ��.
            if (!PhotonNetwork.IsConnected)
                return;

            // PhotonView ������Ʈ�� ���� ������Ʈ���� ��� �����´�.
            PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

            foreach (var view in photonViews)
            {
                networkObjectMap.Add(view.ViewID, view.gameObject);
            }
        }


    }

    // �ش� ��ü�� �߷��� ��� Ŭ���̾�Ʈ������ �����ϵ��� �Ѵ�.
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
