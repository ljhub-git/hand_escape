using UnityEngine;

using Photon.Pun;

public class HeadTransformSync : MonoBehaviourPun
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Send data to others
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            try
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
            }
            catch
            {
                Debug.Log("Transform Sync Receive Error");
            }
        }
    }
}
