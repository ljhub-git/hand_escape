using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;
using NUnit.Framework;

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

    private InputDevice handDevice;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            foreach(var obj in destroyObjects)
            {
                Destroy(obj);
            }
        }
    }
    private void MapPosition(Transform _target, XRNode _node)
    {
        //InputDevices.GetDeviceAtXRNode(_node).TryGetFeatureVal

        
    }
}