using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class TitleSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string gameVersion = "0.0.1";

    [SerializeField]
    private bool isTestBuild = false;

    private LoginManager loginManager = null;

    private const int MaxPlayerPerRoom = 2;

    public bool isLogin = false;

    #region Public Func
    public void TryLogin(string _id, string _pw)
    {
        if (!isTestBuild)
            FindAnyObjectByType<DatabaseManager>().LoginCheck(_id, _pw);
        else
            OnLoginSuccess();
    }

    public void OnLoginSuccess()
    {
        isLogin = true;
        Connect();
    }

    public void OnLoginFailed()
    {
        Debug.Log("Login Failed!");
        loginManager.OnLoginFailed();
    }
    #endregion

    #region Private Func

    private void Connect()
    {
        if (!isTestBuild)
        {
            PhotonNetwork.NickName = loginManager.CurrentID;
        }
        else
        {
            PhotonNetwork.NickName = "Nickky";
        }


        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Connect : {0}", gameVersion);

            PhotonNetwork.GameVersion = gameVersion;
            // 포톤 클라우드에 접속을 시작하는 지점
            // 접속에 성공하면 OnConnectedToMaster 메서드 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region Unity Callback Func
    private void Awake()
    {
        loginManager = FindAnyObjectByType<LoginManager>();
    }
    #endregion

    #region PUN Callback Func

    // 마스터 클라이언트와 연결됐을 때
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected To Master");

        if(isTestBuild && PhotonNetwork.CountOfPlayers > 1)
        {
            PhotonNetwork.NickName = "Jason";
        }

        PhotonNetwork.JoinRandomRoom();
    }

    // 연결이 끊어졌을 때
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.LogWarningFormat("Disconnected: {0}", cause);

        // 방을 생성하면 OnJoinedRoom 호출
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions
            {
                MaxPlayers = MaxPlayerPerRoom
            });
    }

    // 방에 입장할 때
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room");

        // 방에 입장하면 바로 대기실 씬으로 간다.
        PhotonNetwork.LoadLevel("S_WaitingRoom");
    }

    // 방 입장이 실패했을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        // 방이 없다는 뜻이므로 방을 만들고, 만든 방에 입장한다.
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom("Room1", new RoomOptions { MaxPlayers = MaxPlayerPerRoom });
    }
    #endregion
}
