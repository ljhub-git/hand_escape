using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkTestManager : MonoBehaviourPunCallbacks
{
    private void ConnectToServer()
    {
        Debug.Log("Try Connect To server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        ConnectToServer();
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
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player Joined Room!");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
