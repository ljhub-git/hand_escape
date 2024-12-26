using UnityEngine;

public class LaserMirrorVelocityLimit : MonoBehaviour
{
    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.maxAngularVelocity = 1f;
    }
}
