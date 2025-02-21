using UnityEngine;

using Photon.Pun;

/// <summary>
/// 네트워크 상에서 오브젝트의 동기화 처리를 담당하는 매니저 클래스
/// </summary>
public class NetworkObjectManager : MonoBehaviourPun
{
    // 해당 물체의 중력 사용여부 RPC 호출
    public void SetNetworkObjectGravityUsable(PhotonView _view, bool _usable)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetGravity", RpcTarget.Others, id, _usable);
    }

    // 퍼즐 해결 RPC 호출
    public void CallOnPuzzleSolvedToOthers(PhotonView _view)
    {
        if (!IsViewValid(_view) || _view.GetComponent<PuzzleReactObject>() == null)
        {
            return;
        }

        int id = _view.ViewID;

        photonView.RPC("RPC_PuzzleSolved", RpcTarget.Others, id);
    }

    // 퍼즐 리셋 RPC 호출
    public void CallOnPuzzleResetToOthers(PhotonView _view)
    {
        if (!IsViewValid(_view) || _view.GetComponent<PuzzleReactObject>() == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_PuzzleReset", RpcTarget.Others, id);
    }

    // 회전값과 포지션값을 동기화하는 RPC를 호출.
    public void SetObjectTransform(PhotonView _view, Transform _tr)
    {
        if (!IsViewValid(_view))
            return;

        SetObjectRotation(_view, _tr.rotation);

        SetObjectPosition(_view, _tr.position);
    }

    // 회전값 동기화 RPC 호출.
    public void SetObjectRotation(PhotonView _view, Quaternion _rot)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetRotation", RpcTarget.Others, id, _rot);
    }

    // 포지션값 동기화 RPC 호출.
    public void SetObjectPosition(PhotonView _view, Vector3 _pos)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetPosition", RpcTarget.Others, id, _pos);
    }

    // 오브젝트를 활성화시키는 RPC 호출
    public void SetObjectActive(PhotonView _view, bool _active)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetActive", RpcTarget.Others, id, _active);
    }

    // 네트워크 상에서 오브젝트를 파괴.
    public void DestroyObject(PhotonView _view)
    {
        if (!IsViewValid(_view))
        {
            Debug.Log("view is not valid!");
            return;
        }


        PhotonNetwork.Destroy(_view);
    }

    // 네트워크 상에서 오브젝트를 생성.
    public GameObject InstantiateObject(string _name, Vector3 _pos, Quaternion _rot)
    {
        GameObject obj = PhotonNetwork.Instantiate(_name, _pos, _rot);

        return obj;
    }

    // 이 오브젝트의 네트워크 보유 주체를 설정함.
    public void RequestOwnership(PhotonView _view)
    {
        _view.RequestOwnership();
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

        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if (view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        Rigidbody targetRb = view.GetComponent<Rigidbody>();

        if(targetRb != null)
        {
            targetRb.useGravity = _usable;
        }
    }

    [PunRPC]
    private void RPC_SetRotation(int _viewId, Quaternion _rot)
    {
        Debug.Log("SetRotation RPC Called!");
        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if(view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        Transform targetTr = view.transform;

        if(targetTr != null)
        {
            targetTr.rotation = _rot;
        }
    }

    [PunRPC]
    private void RPC_SetPosition(int _viewId, Vector3 _pos)
    {
        Debug.Log("SetPosition RPC Called!");

        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if (view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        Transform targetTr = view.transform;

        if (targetTr != null)
        {
            targetTr.position = _pos;
        }
    }

    [PunRPC]
    private void RPC_PuzzleSolved(int _viewId)
    {
        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if (view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        PuzzleReactObject reactObj = view.GetComponent<PuzzleReactObject>();

        if (reactObj != null)
        {
            reactObj.OnPuzzleSolved();
        }
    }

    [PunRPC]
    private void RPC_PuzzleReset(int _viewId)
    {
        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if (view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        PuzzleReactObject reactObj = view.GetComponent<PuzzleReactObject>();

        if (reactObj != null)
        {
            reactObj.OnPuzzleReset();
        }
    }

    [PunRPC]
    private void RPC_SetActive(int _viewId, bool _active)
    {
        Debug.Log("SetPosition RPC Called!");

        PhotonView view = PhotonNetwork.GetPhotonView(_viewId);

        if (view == null)
        {
            Debug.LogWarningFormat("{0} view is not exist in Scene!", _viewId);

            return;
        }

        view.gameObject.SetActive(_active);
    }
    #endregion
}
