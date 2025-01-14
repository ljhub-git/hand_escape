using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public delegate void OnPlayerSpawnedDelegate();
    public delegate void OnPlayerLeavedRoomDelegate();

    public OnPlayerSpawnedDelegate OnPlayerSpawned = null;
    public OnPlayerLeavedRoomDelegate OnPlayerLeavedRoom = null;

    private NetworkPlayerSpawner playerSpawner = null;
    private NetworkObjectManager networkObjectMng = null;
    private ScreenUIManager screenUIManager = null;

    public bool IsMaster
    {
        get { return PhotonNetwork.IsMasterClient; }
    }

    public string NickName
    {
        get
        {
            return PhotonNetwork.NickName;
        }
    }
    #region Public Func

    /// <summary>
    /// �濡 �������� �÷��̾�鿡�� ���ο� ������ ������ ��.
    /// </summary>
    /// <param name="_sceneName">�ε��� �� �̸�</param>
    public void LoadScene(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene("S_Title");
        Time.timeScale = 1f;
        PhotonNetwork.Disconnect();
    }

    #endregion

    #region Private Func
    private IEnumerator SpawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Network Player Spawned!");

        playerSpawner.SpawnPlayer();

        yield return new WaitForSeconds(0.5f);

        OnPlayerSpawned?.Invoke();
    }
    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        // ���� ���� �� �� �÷��̾�� �׻� ���� ���� �־�� �Ѵ�.
        // ���� �ڵ����� ����ȭ�ϵ��� ����.
        PhotonNetwork.AutomaticallySyncScene = true;

        playerSpawner = GetComponent<NetworkPlayerSpawner>();

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();

        networkObjectMng.InitPrefabPool();

        screenUIManager = FindAnyObjectByType<ScreenUIManager>();
    }

    private void Start()
    {
        StartCoroutine(SpawnPlayerCoroutine());
    }

    #endregion

    #region Photon Callback Func

    // �ٸ� �÷��̾ ���� ������ �� ȣ��Ǵ� �ݹ� �Լ�.
    // �⺻������ �ڿ��̾�� ����Ǵ� ������. ���� �� �÷��̾ �����ٸ� �÷��̾ �����ٴ� UI�� ����� �� ������.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if(screenUIManager != null)
            screenUIManager.ShowPlayerLeaveConfirm();

        // SceneManager.LoadScene("S_Title");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        playerSpawner.DestroyPlayer();
    }
    #endregion
}
