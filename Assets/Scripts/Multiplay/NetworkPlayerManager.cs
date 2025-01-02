using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;

public class NetworkPlayerManager : MonoBehaviourPun
{
    [SerializeField]
    private GameObject LocalPlayerObject = null;
    [SerializeField]
    private GameObject MultiPlayerObject = null;

    private void Start()
    {
        MultiHandManager multiHandMng = MultiPlayerObject.GetComponent<MultiHandManager>();

        multiHandMng.InitHandJointTrs();

        // �ٸ� Ŭ���̾�Ʈ ������ ��
        if (!photonView.IsMine)
        {
            LocalPlayerObject.SetActive(false);
            MultiPlayerObject.SetActive(true);

            MultiPlayerObject.GetComponent<MultiHandManager>().DisableHandInput();
        }
        // ���� Ŭ���̾�Ʈ ������ ��
        else
        {
            LocalPlayerObject.SetActive(true);
            multiHandMng.HiddenHandMesh();
        }
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            MultiPlayerObject.transform.position = LocalPlayerObject.transform.position;
        }
    }
}