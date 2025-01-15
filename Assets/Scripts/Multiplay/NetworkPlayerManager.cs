using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;
using System.Collections;

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

        // 다른 클라이언트의 소유일 때
        if (!photonView.IsMine)
        {
            //LocalPlayerObject.SetActive(false);
            Destroy(LocalPlayerObject);
            MultiPlayerObject.SetActive(true);

            MultiPlayerObject.GetComponent<MultiHandManager>().DisableHandInput();
        }
        // 현재 클라이언트의 소유일 때
        else
        {
            LocalPlayerObject.SetActive(true);
            multiHandMng.HiddenHandMesh();
            StartCoroutine(SetMultiModelTransformCoroutine());
            HeadSync head = GetComponentInChildren<HeadSync>();

            if (head != null)
            {
                head.StartFollowTransformCoroutine();
                head.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SetMultiModelTransformCoroutine()
    {
        while(true)
        {
            MultiPlayerObject.transform.position = LocalPlayerObject.transform.position;
            MultiPlayerObject.transform.rotation = LocalPlayerObject.transform.rotation;

            yield return null;
        }

    }
}