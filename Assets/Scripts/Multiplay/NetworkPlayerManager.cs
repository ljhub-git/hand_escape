using UnityEngine;

using Photon.Pun;

public class NetworkPlayerManager : MonoBehaviourPun
{
    [SerializeField]
    private GameObject[] destroyObjects = null;

    [SerializeField]
    private GameObject cameraGo = null;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            foreach (GameObject go in destroyObjects)
            {
                Destroy(go);
            }

            cameraGo.SetActive(false);
        }
    }


}