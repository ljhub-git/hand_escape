using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DisableNetworkGravity : MonoBehaviour, IXRSelectFilter
{
    private NetworkObjectManager networkObjectMng = null;
    private XRInteractionManager interactionManager = null;

    public bool canProcess => networkObjectMng != null;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        interactable.transform.GetComponent<Rigidbody>().useGravity = false;

        return true;
    }

    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    private void Start()
    {
        interactionManager = GetComponent<XRInteractionManager>();
    }
}
