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

    private const int MaxPlayerPerRoom = 5;

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
            // ���� Ŭ���忡 ������ �����ϴ� ����
            // ���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
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

    // ������ Ŭ���̾�Ʈ�� ������� ��
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected To Master");

        // ���� �г����� �÷��̾ �̹� �����ߴٸ� ������ �� �ϰ� ���´�.
        foreach(var player in PhotonNetwork.PlayerListOthers)
        {
            Debug.Log(player.NickName);

            if(player.NickName == PhotonNetwork.NickName)
            {
                Debug.Log("Same Id is current Login! Go back!");
                return;
            }
        }

        PhotonNetwork.JoinRandomRoom();
    }

    // ������ �������� ��
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.LogWarningFormat("Disconnected: {0}", cause);

        // ���� �����ϸ� OnJoinedRoom ȣ��
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions
            {
                MaxPlayers = MaxPlayerPerRoom
            });
    }

    // �濡 ������ ��
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room");

        // �濡 �����ϸ� �ٷ� ���� ������ ����.
        PhotonNetwork.LoadLevel("S_WaitingRoom");
    }

    // �� ������ �������� ��
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        // ���� ���ٴ� ���̹Ƿ� ���� �����, ���� �濡 �����Ѵ�.
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom("Room1", new RoomOptions { MaxPlayers = MaxPlayerPerRoom });
    }
    #endregion
}
