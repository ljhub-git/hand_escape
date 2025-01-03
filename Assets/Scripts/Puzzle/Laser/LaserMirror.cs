using UnityEngine;

public class LaserMirror : MonoBehaviour
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
