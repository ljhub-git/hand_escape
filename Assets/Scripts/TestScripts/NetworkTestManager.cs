using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkTestManager : MonoBehaviourPunCallbacks
{
    private NetworkPlayerSpawner networkPlayerSpawner = null;

    private void Awake()
    {
        networkPlayerSpawner = GetComponent<NetworkPlayerSpawner>();
    }

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        Debug.Log("Try Connect To server...");
        PhotonNetwork.ConnectUsingSettings();
    }

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

        networkPlayerSpawner.SpawnPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player Joined Room!");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
