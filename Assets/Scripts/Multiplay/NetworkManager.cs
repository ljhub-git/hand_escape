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
