using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform spawnPoint1P = null;
    [SerializeField]
    private Transform spawnPoint2P = null;

    private GameObject spawnedPlayer = null;

    public void SpawnPlayer()
    {
        if (PhotonNetwork.CountOfPlayers == 1)
        {
            spawnedPlayer = PhotonNetwork.Instantiate("XRMultiCharacter", spawnPoint1P.position, Quaternion.identity);
        }
        else
        {
            spawnedPlayer = PhotonNetwork.Instantiate("XRMultiCharacter", spawnPoint2P.position, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
