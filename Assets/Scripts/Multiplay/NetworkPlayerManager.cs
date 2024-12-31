using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;

public class NetworkPlayerManager : MonoBehaviourPun
{
    [SerializeField]
    private Transform headTr = null;
    [SerializeField]
    private Transform leftHandTr = null;
    [SerializeField]
    private Transform rightHandTr = null;

    [SerializeField]
    private GameObject[] destroyObjects = null;

    [SerializeField]
    private GameObject cameraGo = null;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            cameraGo.SetActive(false);
        }
    }

    private void MapPosition(Transform _target, XRNode _node)
    {
        //InputDevices.GetDeviceAtXRNode(_node).TryGetFeatureVal

        
    }
}