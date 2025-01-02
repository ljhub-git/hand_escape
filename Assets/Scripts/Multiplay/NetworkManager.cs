using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private NetworkPlayerSpawner playerSpawner = null;

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
    }

    private void Start()
    {
        playerSpawner.SpawnPlayer();
    }

    #endregion

    #region Photon Callback Func

    #endregion
}
