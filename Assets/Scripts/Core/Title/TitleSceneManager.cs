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
        PhotonNetwork.JoinRandomRoom();
    }

    // ������ �������� ��
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        Debug.Log("Joined Room");

        // �濡 ���������� ���� ���Ƿ� ���� �ȴ�.
    }

    // �� ������ �������� ��
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayerPerRoom });
    }
    #endregion
}
