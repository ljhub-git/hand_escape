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
    /// 방에 접속중인 플레이어들에게 새로운 씬으로 가도록 함.
    /// </summary>
    /// <param name="_sceneName">로드할 씬 이름</param>
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
        // 게임 시작 시 두 플레이어는 항상 같은 씬에 있어야 한다.
        // 씬을 자동으로 동기화하도록 설정.
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

    // 다른 플레이어가 방을 떠났을 때 호출되는 콜백 함수.
    // 기본적으로 코옵이어야 진행되는 게임임. 게임 중 플레이어가 나간다면 플레이어가 떠났다는 UI를 출력한 후 나간다.
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
