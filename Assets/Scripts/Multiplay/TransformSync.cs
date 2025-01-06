using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;

public class TransformSync : MonoBehaviourPun, IPunObservable
{
    private List<Transform> syncTargetTransforms = new List<Transform>(); // Array of transforms to sync

    public void AddTransforms(List<Transform> addTrs)
    {
        syncTargetTransforms.AddRange(addTrs);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to others
            foreach (var t in syncTargetTransforms)
            {
                if (t != null)
                {
                    stream.SendNext(t.position);
                    stream.SendNext(t.rotation);
                }
            }
        }
        else
        {
            // Receive data
            foreach (var t in syncTargetTransforms)
            {
                if (t != null)
                {
                    try
                    {
                        t.position = (Vector3)stream.ReceiveNext();
                        t.rotation = (Quaternion)stream.ReceiveNext();
                    }
                    catch
                    {
                        Debug.Log("Transform Sync Receive Error");
                    }
                }
            }
        }
    }
}
