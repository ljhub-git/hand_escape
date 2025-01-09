using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TestXRInteractionManager : XRInteractionManager
{
    private NetworkObjectManager networkObjectMng = null;

    private ToggleTransformSync toggleTransformSync = null;

    protected override void Awake()
    {
        base.Awake();

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        toggleTransformSync = GetComponent<ToggleTransformSync>();
    }

    public override void SelectEnter(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        base.SelectEnter(interactor, interactable);

        networkObjectMng.SetNetworkObjectGravityUsable(interactable.transform.GetComponent<PhotonView>(), false);
        toggleTransformSync.TurnOnTransformSync();
    }

    public override void SelectExit(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        base.SelectExit(interactor, interactable);

        networkObjectMng.SetNetworkObjectGravityUsable(interactable.transform.GetComponent<PhotonView>(), true);
        toggleTransformSync.TurnOffTransformSync();
    }
}
