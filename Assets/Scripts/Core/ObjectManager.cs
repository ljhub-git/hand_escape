using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectManager : MonoBehaviour
{
    private XRGrabInteractable[] interactableObjList = null;

    private void Awake()
    {
    }

    private void Start()
    {
        interactableObjList = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.InstanceID);

        foreach(var interactObj in interactableObjList)
        {
            Debug.Log(interactObj.gameObject.name);
        }
    }

}
