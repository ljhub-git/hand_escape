using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public delegate void OnPlayerSpawnedDelegate();

    public OnPlayerSpawnedDelegate OnPlayerSpawned = null;

    private NetworkPlayerSpawner playerSpawner = null;
    private NetworkObjectManager networkObjectMng = null;

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

    #endregion

    #region Private Func
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
    }

    private void Start()
    {
        StartCoroutine(SpawnPlayerCoroutine());
    }

    #endregion

    #region Photon Callback Func

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        playerSpawner.DestroyPlayer();
    }

    private IEnumerator SpawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Network Player Spawned!");

        playerSpawner.SpawnPlayer();

        yield return new WaitForSeconds(0.5f);

        OnPlayerSpawned?.Invoke();
    }
    #endregion
}
