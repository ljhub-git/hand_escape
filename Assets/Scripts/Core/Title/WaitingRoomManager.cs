using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;

public class WaitingRoomManager : MonoBehaviour
{
    [SerializeField]
    private PlayerReadyUI[] ui_PlayerReadyArr;

    private bool[] isPlayersReadyArr = { false, false };

    private NetworkManager networkMng = null;    

    public void OnReadyButtonSelect(SelectEnterEventArgs _enterEventArgs)
    {
        //if (PhotonNetwork.IsMasterClient)
        //    TogglePlayerReady(0);
        //else
        //    TogglePlayerReady(1);

        // networkMng.LoadScene("S_Stage1Door");
        networkMng.LoadScene("S_Stage3Hand");
    }

    private void TogglePlayerReady(int _playerInd)
    {
        isPlayersReadyArr[_playerInd] = !isPlayersReadyArr[_playerInd];

        // UI
        ui_PlayerReadyArr[_playerInd].ToggleReady();

        // �÷��̾���� �غ� ���θ� Ȯ���ϰ� ��� �غ������� ������ �� �Լ��� �����Ѵ�.
        foreach (var isReady in isPlayersReadyArr)
        {
            if (!isReady)
                return;
        }

        // ��� �÷��̾ �غ����. ���� 1 ������ �Ѿ.
        networkMng.LoadScene("S_Stage1Door");
    }

    private void SetNicknameUIs()
    {
        foreach(var ui_PlayerReady in ui_PlayerReadyArr)
        {
            ui_PlayerReady.SetNickName(networkMng.NickName);
        }
    }

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }

    private void Start()
    {
        SetNicknameUIs();
    }
}
