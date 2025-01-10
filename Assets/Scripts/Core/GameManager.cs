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
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            networkMng.LoadScene(nextLevelName);
        }
    }

    public void OnPlayerEnteredDestination()
    {
        if(networkMng.IsMaster)
        {
            photonView.RPC("RPC_OnPlayerEnterDest", RpcTarget.MasterClient, true);
        }
        else
        {
            photonView.RPC("RPC_OnPlayerEnterDest", RpcTarget.MasterClient, false);
        }
    }

    public void OnPlayerExitedDestination()
    {
        if(networkMng.IsMaster)
        {
            photonView.RPC("RPC_OnPlayerExitDest", RpcTarget.MasterClient, true);
        }
        else
        {
            photonView.RPC("RPC_OnPlayerExitDest", RpcTarget.MasterClient, false);
        }
    }

    [PunRPC]
    public void RPC_OnPlayerEnterDest(int _idx)
    {
        playerReady[_idx] = true;

        foreach (var ready in playerReady)
        {
            if (!ready)
                return;
        }

        LoadNextLevel();
    }

    [PunRPC]
    public void RPC_OnPlayerExitDest(int _idx)
    {
        playerReady[_idx] = false;
    }

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadNextLevel();
        }
    }
}
