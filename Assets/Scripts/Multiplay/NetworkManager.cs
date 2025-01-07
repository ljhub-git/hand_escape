using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private NetworkPlayerSpawner playerSpawner = null;

    public string NickName
    {
        get
        {
            return PhotonNetwork.NickName;
        }
    }
    #region Public Func

    /// <summary>
    /// 방에 접속중인 플레이어들에게 새로운 씬으로 가도록 함.
    /// </summary>
    /// <param name="_sceneName">로드할 씬 이름</param>
    public void LoadScene(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }

    #endregion

    #region Private Func
    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        // 게임 시작 시 두 플레이어는 항상 같은 씬에 있어야 한다.
        // 씬을 자동으로 동기화하도록 설정.
        PhotonNetwork.AutomaticallySyncScene = true;

        playerSpawner = GetComponent<NetworkPlayerSpawner>();
    }

    private void Start()
    {
        playerSpawner.SpawnPlayer();

        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("Number of players in room: " + PhotonNetwork.PlayerList.Length);
            foreach (var player in PhotonNetwork.PlayerList)
            {
                Debug.Log("Player in room: " + player.NickName);
            }
        }
    }

    #endregion

    #region Photon Callback Func

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        playerSpawner.DestroyPlayer();
    }
    #endregion
}
