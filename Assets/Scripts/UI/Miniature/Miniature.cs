using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Miniature : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Rigidbody rb;
    private XRGrabInteractable interactable;

    private void Update()
    {
        target.rotation = transform.rotation;
    }
}
