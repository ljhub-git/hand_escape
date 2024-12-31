using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private NetworkPlayerSpawner playerSpawner = null;

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
    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        // ���� ���� �� �� �÷��̾�� �׻� ���� ���� �־�� �Ѵ�.
        // ���� �ڵ����� ����ȭ�ϵ��� ����.
        PhotonNetwork.AutomaticallySyncScene = true;

        playerSpawner = GetComponent<NetworkPlayerSpawner>();
    }

    private void Start()
    {
        playerSpawner.SpawnPlayer();
    }

    #endregion

    #region Photon Callback Func

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected To Server...");
    //    base.OnConnectedToMaster();

    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = 10;
    //    roomOptions.IsVisible = true;
    //    roomOptions.IsOpen = true;

    //    PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
    //}

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("Joined a Room");
    //    base.OnJoinedRoom();
    //}

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log("A new player Joined Room!");
    //    base.OnPlayerEnteredRoom(newPlayer);
    //}

    #endregion
}
