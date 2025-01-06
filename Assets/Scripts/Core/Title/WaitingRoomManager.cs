using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class WaitingRoomManager : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        PhotonNetwork.LoadLevel("S_Stage1Door");
    }
}
