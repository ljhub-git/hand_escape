using UnityEngine;
using Photon.Pun;

public class MultiplayManager : MonoBehaviourPun
{
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
        // PhotonNetwork.AutomaticallySyncScene = true;
    }

    #endregion

    #region Photon Callback Func

    #endregion
}
