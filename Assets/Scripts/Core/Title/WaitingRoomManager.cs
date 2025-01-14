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

    #region Public Func

    public void OnReadyButtonSelect(SelectEnterEventArgs _enterEventArgs)
    {
        if (PhotonNetwork.IsMasterClient)
            TogglePlayerReady(0);
        else
            TogglePlayerReady(1);
    }

    #endregion

    #region Private Func
    private void TogglePlayerReady(int _playerInd)
    {
        if(isDebugMode && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            networkMng.LoadScene("S_Stage1");
        }

        photonView.RPC("RPC_PlayerReady", RpcTarget.All, _playerInd, !isPlayersReadyArr[_playerInd]);
    }

    private void SetNicknameUIs()
    {
        ui_PlayerReadyArr[0].SetNickName(PhotonNetwork.CurrentRoom.Players[1].NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            ui_PlayerReadyArr[1].SetNickName(PhotonNetwork.CurrentRoom.Players[2].NickName);
        }
        else
        {
            ui_PlayerReadyArr[1].SetNickName("");

            ui_PlayerReadyArr[0].SetPlayerReady(false);
            ui_PlayerReadyArr[1].SetPlayerReady(false);

            isPlayersReadyArr[0] = false;
            isPlayersReadyArr[1] = false;
        }
    }

    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }

    private void Start()
    {
        SetNicknameUIs();
    }

    #endregion

    #region Photon Callback Func

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        SetNicknameUIs();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        SetNicknameUIs();
    }

    #endregion

    #region RPC Func

    [PunRPC]
    public void RPC_PlayerReady(int _playerInd, bool _isReady)
    {
        isPlayersReadyArr[_playerInd] = _isReady;
        ui_PlayerReadyArr[_playerInd].SetPlayerReady(_isReady);

        if (PhotonNetwork.IsMasterClient)
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
    #endregion
}
