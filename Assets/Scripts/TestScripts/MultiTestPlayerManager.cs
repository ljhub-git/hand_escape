using UnityEngine;
using Photon.Pun;

public class MultiTestPlayerManager : MonoBehaviourPun
{
    private void Update()
    {
        if (!photonView.IsMine)
            return;


    }
}
