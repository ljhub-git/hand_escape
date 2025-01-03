using UnityEngine;

using Photon.Pun;
public class LaserMirror : MonoBehaviourPun
{
    private Rigidbody rb = null;

    public void BlockRotate()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
