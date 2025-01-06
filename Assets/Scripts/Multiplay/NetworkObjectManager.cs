using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPun
{
    /// <summary>
    /// ��ȣ�ۿ��� �����ϸ鼭 ����ȭ�ϵ��� ������ ������Ʈ ��ųʸ�.
    /// Ű���� ���� ���� ���̵�.
    /// </summary>
    /// 

    [SerializeField]
    private GameObject[] runtimeInstantiatePrefabs;

    private Dictionary<int, GameObject> networkObjectMap = null;

    public Dictionary<int, GameObject> NetworkObjectMap
    {
        get { return networkObjectMap; }
    }

    private void Start()
    {
        InitObjectMap();
    }

    public void InitPrefabPool()
    {
        networkObjectMap = new Dictionary<int, GameObject>();

        DefaultPool Pool = PhotonNetwork.PrefabPool as DefaultPool;

        foreach (var prefab in runtimeInstantiatePrefabs)
        {
            Pool.ResourceCache.TryAdd(prefab.name, prefab);
        }
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

    public void SetObjectTransform(PhotonView _view, Transform _tr)
    {
        if (!IsViewValid(_view))
            return;

        SetObjectRotation(_view, _tr.rotation);

        SetObjectPosition(_view, _tr.position);
    }

    public void SetObjectRotation(PhotonView _view, Quaternion _rot)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetRotation", RpcTarget.Others, id, _rot);
    }

    public void SetObjectPosition(PhotonView _view, Vector3 _pos)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetPosition", RpcTarget.Others, id, _pos);
    }

    public void DestroyObject(PhotonView _view)
    {
        if (!IsViewValid(_view))
            return;

        PhotonNetwork.Destroy(_view);
    }

    public void InstantiateObject(string _name, Vector3 _pos, Quaternion _rot)
    {
        PhotonNetwork.Instantiate(_name, _pos, _rot);
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
        Debug.Log("SetRotation RPC Called!");

        networkObjectMap[_viewId].transform.rotation = _rot;
    }

    [PunRPC]
    private void RPC_SetPosition(int _viewId, Vector3 _pos)
    {
        Debug.Log("SetPosition RPC Called!");

        networkObjectMap[_viewId].transform.position = _pos;
    }

    [PunRPC]
    private void RPC_PuzzleSolved(int _viewId)
    {
        PuzzleReactObject reactObj = networkObjectMap[_viewId].GetComponent<PuzzleReactObject>();

        reactObj?.OnPuzzleSolved();
    }
    #endregion
}
