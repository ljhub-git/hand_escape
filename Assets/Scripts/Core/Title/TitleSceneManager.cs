using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string gameVersion = "0.0.1";

    private LoginManager loginManager = null;

    private const int MaxPlayerPerRoom = 2;


    #region Public Func
    public void TryLogin(string _id, string _pw)
    {
        FindAnyObjectByType<DatabaseManager>().LoginCheck(_id, _pw);
    }

    public void OnLoginSuccess()
    {
        Connect();
    }

    public void OnLoginFailed()
    {
        Debug.Log("Login Failed!");
    }
    #endregion

    #region Private Func

    private void Connect()
    {
        PhotonNetwork.NickName = loginManager.CurrentID;

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
        PhotonNetwork.JoinRandomRoom();
    }

    // 연결이 끊어졌을 때
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        Debug.Log("Joined Room");

        // 방에 입장했으니 이제 대기실로 가면 된다.
    }

    // 방 입장이 실패했을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayerPerRoom });
    }
    #endregion
}
