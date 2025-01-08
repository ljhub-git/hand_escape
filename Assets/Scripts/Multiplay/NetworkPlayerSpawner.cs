using UnityEngine;

using Photon.Pun;
using System.Collections;

public class NetworkPlayerSpawner : MonoBehaviourPun
{
    [SerializeField]
    private Transform[] spawnPoints = null;

    private GameObject spawnedPlayer = null;
    public void SpawnPlayer()
    {
        Vector3 spawnPos = Vector3.zero;

        if(PhotonNetwork.IsMasterClient)
        {
            spawnPos = spawnPoints[0].position;
        }
        else
        {
            spawnPos = spawnPoints[1].position;
        }

        spawnedPlayer = PhotonNetwork.Instantiate(
            "XRMultiPlayer",
            spawnPos, 
            Quaternion.identity
            );
    }

    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(spawnedPlayer);
    }
}
