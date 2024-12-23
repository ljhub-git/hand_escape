using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayer = null;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        spawnedPlayer = PhotonNetwork.Instantiate("P_XRCharacterTest", transform.position, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
