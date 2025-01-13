using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
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
        if(isDebugMode && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            networkMng.LoadScene("S_Stage1");
        }

        photonView.RPC("RPC_PlayerReady", RpcTarget.All, _playerInd);
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
        ui_PlayerReadyArr[0].SetNickName(PhotonNetwork.CurrentRoom.Players[1].NickName);
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            ui_PlayerReadyArr[1].SetNickName(PhotonNetwork.CurrentRoom.Players[2].NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        SetNicknameUIs();
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
