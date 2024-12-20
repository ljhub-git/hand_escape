using UnityEngine;
using Photon.Pun;

public class MultiPlayerManager : MonoBehaviourPun
{
    [SerializeField]
    private GameObject CameraObj = null;

    private void Start()
    {
        if (photonView.IsMine)
            CameraObj.SetActive(true);
    }
}
