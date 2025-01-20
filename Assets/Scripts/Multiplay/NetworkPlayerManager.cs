using UnityEngine;

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

        // 다른 클라이언트의 소유일 때는 로컬 플레이어 오브젝트를 삭제하여서
        // 해당 캐릭터에게 들어오는 현재 클라이언트의 입력을 막는다.
        if (!photonView.IsMine)
        {
            Destroy(LocalPlayerObject);
            MultiPlayerObject.SetActive(true);

            // 멀티 플레이어 오브젝트에게 들어가는 입력도 막는다.
            MultiPlayerObject.GetComponent<MultiHandManager>().DisableHandInput();
        }

        // 현재 클라이언트의 소유일 때는 로컬 플레이어 오브젝트를 살려둔다.
        // 멀티 플레이어 오브젝트는 숨겨놓는다.
        else
        {
            LocalPlayerObject.SetActive(true);
            multiHandMng.HiddenHandMesh();

            // 멀티 플레이어 오브젝트가 가지고 있는 손의 각 관절 위치를 동기화한다.
            StartCoroutine(SetMultiModelTransformCoroutine());

            // 머리 위치도 동기화한다.
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