using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPun
{
    [SerializeField]
    private Transform[] spawnPoints = null;

    private GameObject spawnedPlayer = null;
    public void SpawnPlayer()
    {
        spawnedPlayer = PhotonNetwork.Instantiate(
            "XRMultiPlayer", 
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, 
            Quaternion.identity
            );
    }

    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
