using System.Collections.Generic;
using UnityEngine;

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
        InitObjectMap();
    }

    // ���� �ִ� ���� �並 �������� ����� ���̵� ���ؼ� networkInteractableMap �� �ʱ�ȭ�Ѵ�.
    public void InitObjectMap()
    {
        // ���� ������ ������ �Ǿ��־�� ��.
        if (!PhotonNetwork.IsConnected)
            return;

        // ���� �ʱ�ȭ�Ѵ�.
        networkObjectMap.Clear();

        // PhotonView ������Ʈ�� ���� ������Ʈ���� ��� �����´�.
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        foreach (var view in photonViews)
        {
            networkObjectMap.Add(view.ViewID, view.gameObject);
        }
    }

    // �ش� ��ü�� �߷� ��뿩�θ� �ٸ� Ŭ���̾�Ʈ���� �����ϵ��� �Ѵ�.
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetGravity", RpcTarget.Others, id, _usable);
    }

    // ������ Ǯ���� �� �����ϴ� ������Ʈ���� �ٸ� Ŭ���̾�Ʈ������ �����ϵ��� �Ѵ�.
    public void CallOnPuzzleSolvedToOthers(PhotonView _view)
    {
        if (!IsViewValid(_view) || _view.GetComponent<PuzzleReactObject>() == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_PuzzleSolved", RpcTarget.Others, id);
    }

    // �Ű������� ���� �䰡 ��ȿ�� ������ üũ.
    private bool IsViewValid(PhotonView _view)
    {
        if (!PhotonNetwork.IsConnected || _view == null)
            return false;

        return true;
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

    [PunRPC]
    private void RPC_PuzzleSolved(int _viewId)
    {
        PuzzleReactObject reactObj = networkObjectMap[_viewId].GetComponent<PuzzleReactObject>();

        reactObj?.OnPuzzleSolved();
    }
    #endregion
}
