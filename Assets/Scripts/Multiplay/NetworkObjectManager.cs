using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectManager : MonoBehaviourPun
{
    /// <summary>
    /// 상호작용이 가능하면서 동기화하도록 설정된 오브젝트 딕셔너리.
    /// 키값은 포톤 뷰의 아이디.
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

    // 씬에 있는 포톤 뷰를 가져오고 포톤뷰 아이디를 통해서 networkInteractableMap 을 초기화한다.
    public void InitObjectMap()
    {
        // 포톤 서버에 접속이 되어있어야 함.
        if (!PhotonNetwork.IsConnected)
            return;

        // 맵을 초기화한다.
        networkObjectMap.Clear();

        // PhotonView 컴포넌트를 가진 오브젝트들을 모두 가져온다.
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        foreach (var view in photonViews)
        {
            networkObjectMap.Add(view.ViewID, view.gameObject);
        }
    }

    // 해당 물체의 중력 사용여부를 다른 클라이언트에서 설정하도록 한다.
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetGravity", RpcTarget.Others, id, _usable);
    }

    // 퍼즐이 풀렸을 때 반응하는 오브젝트들을 다른 클라이언트에서도 반응하도록 한다.
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

    // 매개변수로 들어온 뷰가 유효한 뷰인지 체크.
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
