using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Public Func

    /// <summary>
    /// �濡 �������� �÷��̾�鿡�� ���ο� ������ ������ ��.
    /// </summary>
    /// <param name="_sceneName">�ε��� �� �̸�</param>
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
        // ���� ���� �� �� �÷��̾�� �׻� ���� ���� �־�� �Ѵ�.
        // ���� �ڵ����� ����ȭ�ϵ��� ����.
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
