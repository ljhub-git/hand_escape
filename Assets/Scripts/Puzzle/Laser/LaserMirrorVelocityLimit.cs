using UnityEngine;

public class LaserMirrorVelocityLimit : MonoBehaviour
{
    [SerializeField]
    private float maxAngularVelocity = 1f;

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
