using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;

public class WaitingRoomManager : MonoBehaviourPun
{
    [SerializeField]
    private bool isDebugMode = false;

    [SerializeField]
    private PlayerReadyUI[] ui_PlayerReadyArr;

    private bool[] isPlayersReadyArr = { false, false };

    private NetworkManager networkMng = null;    

    public void OnReadyButtonSelect(SelectEnterEventArgs _enterEventArgs)
    {
        if (PhotonNetwork.IsMasterClient)
            TogglePlayerReady(0);
        else
            TogglePlayerReady(1);
    }

    private void TogglePlayerReady(int _playerInd)
    {
        isPlayersReadyArr[_playerInd] = !isPlayersReadyArr[_playerInd];

        // UI
        ui_PlayerReadyArr[_playerInd].ToggleReady();

        if(isDebugMode && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            networkMng.LoadScene("S_Stage1");
        }

        photonView.RPC("RPC_PlayerReady", RpcTarget.Others, _playerInd);
    }

    [PunRPC]
    public void RPC_PlayerReady(int _playerInd)
    {
        isPlayersReadyArr[_playerInd] = !isPlayersReadyArr[_playerInd];
        ui_PlayerReadyArr[_playerInd].ToggleReady();

        if(PhotonNetwork.IsMasterClient)
        {
            foreach (var isReady in isPlayersReadyArr)
            {
                if (!isReady)
                    return;
            }

            PhotonNetwork.DestroyAll();
            networkMng.LoadScene("M_Stage_1");
        }
    }

    private void SetNicknameUIs()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            ui_PlayerReadyArr[0].SetNickName(networkMng.NickName);
        }
        else
        {
            ui_PlayerReadyArr[1].SetNickName(networkMng.NickName);
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
