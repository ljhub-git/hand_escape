using UnityEngine;

using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPun
{
    [SerializeField]
    private Transform[] spawnPoints = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    private GameObject spawnedPlayer = null;

    private void Awake()
    {
        DefaultPool Pool = PhotonNetwork.PrefabPool as DefaultPool;
        Pool.ResourceCache.TryAdd("XRMultiCharacter", playerPrefab);
    }
    public void SpawnPlayer()
    {
        spawnedPlayer = PhotonNetwork.Instantiate(
            "XRMultiCharacter", 
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, 
            Quaternion.identity
            );
    }

    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
