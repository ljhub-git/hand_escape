using UnityEngine;

using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    [SerializeField]
    private bool isDebugMode = false;

    [SerializeField]
    private string nextLevelName = string.Empty;

    private bool[] playerReady = { false, false };

    private NetworkManager networkMng = null;

    public bool IsDebugMode
    {
        get { return isDebugMode; }
    }

    public void StartDebugMode()
    {
        isDebugMode = true;
    }

    public void EndDebugMode()
    {
        isDebugMode = false;
    }

    public void LoadNextLevel()
    {
        networkMng.LoadScene(nextLevelName);
    }

    public void OnPlayerEnteredDestination()
    {
        if(networkMng.IsMaster)
        {
            photonView.RPC("RPC_OnPlayerEnterDest", RpcTarget.All, true);
        }
        else
        {
            photonView.RPC("RPC_OnPlayerEnterDest", RpcTarget.All, false);
        }
    }

    public void OnPlayerExitedDestination()
    {
        if(networkMng.IsMaster)
        {
            photonView.RPC("RPC_OnPlayerExitDest", RpcTarget.All, true);
        }
        else
        {
            photonView.RPC("RPC_OnPlayerExitDest", RpcTarget.All, false);
        }
    }

    [PunRPC]
    public void RPC_OnPlayerEnterDest(bool _isMaster)
    {
        if(_isMaster)
        {
            playerReady[0] = true;
        }
        else
        {
            playerReady[1] = true;
        }


        if (networkMng.IsMaster)
        {
            foreach (var ready in playerReady)
            {
                if (!ready)
                    return;
            }

            LoadNextLevel();
        }
    }

    [PunRPC]
    public void RPC_OnPlayerExitDest(bool _isMaster)
    {
        if (_isMaster)
        {
            playerReady[0] = false;
        }
        else
        {
            playerReady[1] = false;
        }
    }

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }
}
