using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;

public class NetworkObjectManager : MonoBehaviourPun
{
    /// <summary>
    /// 상호작용이 가능하면서 동기화하도록 설정된 오브젝트 딕셔너리.
    /// 키값은 포톤 뷰의 아이디.
    /// </summary>
    /// 

    [SerializeField]
    private GameObject[] runtimeInstantiatePrefabs;

    [SerializeField]
    private GameObject[] viewErrorObjects;

    public void SyncInGameManagerViewID()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            throw new System.Exception("Only Master CLient can send sync manager view ID event.");
        }

        foreach(var obj in viewErrorObjects)
        {
            PhotonView targetView = obj.GetComponent<PhotonView>();

            if (targetView != null && PhotonNetwork.AllocateViewID(targetView))
            {
                object content = new object[] {
                    targetView.ViewID,
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
                {
                    Receivers = ReceiverGroup.Others,
                };

                PhotonNetwork.RaiseEvent(6, content, raiseEventOptions, SendOptions.SendReliable);
            }
            else
            {
                throw new System.Exception("Failed allocate View ID");
            }
        }
    }

    public void InitPrefabPool()
    {
        DefaultPool Pool = PhotonNetwork.PrefabPool as DefaultPool;

        foreach (var prefab in runtimeInstantiatePrefabs)
        {
            Pool.ResourceCache.TryAdd(prefab.name, prefab);
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
        {
            return;
        }

        int id = _view.ViewID;

        photonView.RPC("RPC_PuzzleSolved", RpcTarget.Others, id);
    }

    public void CallOnPuzzleResetToOthers(PhotonView _view)
    {
        if (!IsViewValid(_view) || _view.GetComponent<PuzzleReactObject>() == null)
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_PuzzleReset", RpcTarget.Others, id);
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

    public void SetObjectActive(PhotonView _view, bool _active)
    {
        if (!IsViewValid(_view))
            return;

        int id = _view.ViewID;

        photonView.RPC("RPC_SetActive", RpcTarget.Others, id, _active);
    }

    public void DestroyObject(PhotonView _view)
    {
        if (!IsViewValid(_view))
        {
            Debug.Log("view is not valid!");
            return;
        }


        PhotonNetwork.Destroy(_view);
    }

    public GameObject InstantiateObject(string _name, Vector3 _pos, Quaternion _rot)
    {
        GameObject obj = PhotonNetwork.Instantiate(_name, _pos, _rot);

        return obj;
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
