using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform spawnPoint1P = null;
    [SerializeField]
    private Transform spawnPoint2P = null;

    private GameObject spawnedPlayer = null;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if(PhotonNetwork.IsMasterClient)
        {
            spawnedPlayer = PhotonNetwork.Instantiate("XRMultiTest", spawnPoint1P.position, Quaternion.identity);
        }
        else
        {
            spawnedPlayer = PhotonNetwork.Instantiate("P_NetworkObserver", spawnPoint2P.position, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
