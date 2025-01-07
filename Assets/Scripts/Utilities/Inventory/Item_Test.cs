using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item_Test : MonoBehaviour
{
    public XRGrabInteractable XRGrabInteractable;

    public Color itemColor;

    private void Awake()
    {
        XRGrabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void Start()
    {
        MeshRenderer mr =
            GetComponentInChildren<MeshRenderer>();
        itemColor = mr.material.color;
    }
}
