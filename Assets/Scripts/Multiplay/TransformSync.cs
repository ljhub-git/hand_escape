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
        // 데이터를 다른 클라이언트에게 전송할 때
        if (stream.IsWriting)
        {
            foreach (var tr in syncTargetTransforms)
            {
                if (tr != null)
                {
                    stream.SendNext(tr.position);
                    stream.SendNext(tr.rotation);
                }
            }
        }
        // 데이터를 전송받았을 때
        else
        {
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
