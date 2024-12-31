using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform spawnPoint1P = null;
    [SerializeField]
    private Transform spawnPoint2P = null;

    private GameObject spawnedPlayer = null;

    public void SpawnPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            spawnedPlayer = PhotonNetwork.Instantiate("XRMultiTest", spawnPoint1P.position, Quaternion.identity);
        }
        else
        {
            spawnedPlayer = PhotonNetwork.Instantiate("XRMultiTest", spawnPoint2P.position, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
