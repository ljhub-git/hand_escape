using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
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

    private void ConnectToServer()
    {
        Debug.Log("Try Connect To server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        // 게임 시작 시 두 플레이어는 항상 같은 씬에 있어야 한다.
        // 씬을 자동으로 동기화하도록 설정.
        // PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        ConnectToServer();
    }

    #endregion

    #region Photon Callback Func

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server...");
        base.OnConnectedToMaster();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player Joined Room!");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    #endregion
}
