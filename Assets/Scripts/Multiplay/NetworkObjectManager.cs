using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// ��ȣ�ۿ��� �����ϸ鼭 ����ȭ�ϵ��� ������ ������Ʈ ��ųʸ�
    /// Ű���� ���� ���� ���̵���.
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
        // ���� ������ ������ �Ǿ��־�� ��.
        if (!PhotonNetwork.IsConnected)
            return;

        // XRGrabInteractable ������Ʈ�� ���� ������Ʈ���� ��� �����´�.
        XRGrabInteractable[] grabInteractables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);

        foreach(var interactable in grabInteractables)
        {
            // XRGrabInteractable ������Ʈ�� ���� ������Ʈ���� ��ġ�� ����ȭ�ϱ�� ��.
            // ���� �ش� ������Ʈ���� ���� �� ������Ʈ�� ������ �־�߸� �Ѵ�.
            if (interactable.GetComponent<PhotonView>() == null)
            {
                Debug.LogWarning("XR Interactable Object must have Photon View in Our Project!");
                break;
            }

            networkInteractableMap.Add(interactable.GetComponent<PhotonView>().ViewID, interactable.gameObject);
        }
    }

    // ��ü�� ��ȣ�ۿ� �� �ش� ��ü�� �߷��� ��� Ŭ���̾�Ʈ������ �����ϵ��� �Ѵ�.
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
