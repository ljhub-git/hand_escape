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

        // �ٸ� Ŭ���̾�Ʈ�� ������ ���� ���� �÷��̾� ������Ʈ�� �����Ͽ���
        // �ش� ĳ���Ϳ��� ������ ���� Ŭ���̾�Ʈ�� �Է��� ���´�.
        if (!photonView.IsMine)
        {
            Destroy(LocalPlayerObject);
            MultiPlayerObject.SetActive(true);

            // ��Ƽ �÷��̾� ������Ʈ���� ���� �Էµ� ���´�.
            MultiPlayerObject.GetComponent<MultiHandManager>().DisableHandInput();
        }

        // ���� Ŭ���̾�Ʈ�� ������ ���� ���� �÷��̾� ������Ʈ�� ����д�.
        // ��Ƽ �÷��̾� ������Ʈ�� ���ܳ��´�.
        else
        {
            LocalPlayerObject.SetActive(true);
            multiHandMng.HiddenHandMesh();

            // ��Ƽ �÷��̾� ������Ʈ�� ������ �ִ� ���� �� ���� ��ġ�� ����ȭ�Ѵ�.
            StartCoroutine(SetMultiModelTransformCoroutine());

            // �Ӹ� ��ġ�� ����ȭ�Ѵ�.
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