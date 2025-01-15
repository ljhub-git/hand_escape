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
        // �����͸� �ٸ� Ŭ���̾�Ʈ���� ������ ��
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
        // �����͸� ���۹޾��� ��
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
