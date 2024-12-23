using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //ĳ���� ��Ʈ�ѷ�
    private Coroutine moveCoroutine; // �ڷ�ƾ�� ������ ����
    private Camera playerCamera; // ī�޶� ���� (ī�޶� ���� ���� ���ϱ��);
    private Vector3 HeadVector3 = Vector3.zero; // ��� ��ġ
    private Quaternion HeadDir = Quaternion.identity; // ��� ����

    [SerializeField]// ���� ��ġ�ϸ� �������� �������� ���� ť�� 
    GameObject rocketCubePrefab = null;
    RocketCube rocketCube = null; // Ʈ���� �߻��� �޾ƿ� Ŭ����
    public int preparedHandCnt = 0;
    private Coroutine rocketMoveCoroutine; // �ڷ�ƾ�� ������ ����
    private void Start()
    {
        if (rocketCubePrefab == null) // �������� ������ ����ť�� �ε�
        {
            rocketCubePrefab = (GameObject)Resources.Load("Characters\\Prefabs\\Rocket_Cube"); 
        }

    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();


    }

    #region ����, ����, ����
    public void MoveFoward(float _moveSpeed) // ���� �̵�
    {
        // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� ���� ���߰�, ���ο� �ڷ�ƾ ����
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveFowardCo(_moveSpeed));
    }    
    public void MoveBack(float _moveSpeed) // ���� �̵�
    {
        // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� ���� ���߰�, ���ο� �ڷ�ƾ ����
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveBackCo(_moveSpeed));
    }
    public void Stop() // ����
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // �ڷ�ƾ�� ���� ��, ������ null�� �ʱ�ȭ
        }
    }


    private IEnumerator MoveFowardCo(float _moveSpeed) //PlayerManager���� ��ȣ�� ������ ���� ��ų �ڷ�ƾ ���� �̵�
    {
        while (true)
        {
            HeadVector3 = playerCamera.transform.forward;
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }    
    private IEnumerator MoveBackCo(float _moveSpeed) //PlayerManager���� ��ȣ�� ������ ���� ��ų �ڷ�ƾ ���� �̵�
    {
        while (true)
        {
            HeadVector3 = playerCamera.transform.forward;
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.deltaTime * -1f);
            yield return null;
        }
    }

    #endregion
    #region ȸ��
    public void L_Rotate90() // ���� 90�� ȸ�� 
    {
        transform.Rotate(-Vector3.up * 90f, Space.Self);
    }
    public void R_Rotate90() // ������ 90�� ȸ��
    {
        transform.Rotate(Vector3.up * 90f, Space.Self);
    }
    #endregion
    #region ������ġ
    public void RocketPunchReady(float _rocketMoveSpeed)
    {
        preparedHandCnt++;
        //Debug.Log("������ġ �غ�� �� ���� : " + preparedHandCnt);
        if (preparedHandCnt == 1) // ī��Ʈ 2���� �Ǹ� ����
        {
            // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� ���� ����
            if (rocketMoveCoroutine != null)
            {
                return;
            }

            rocketMoveCoroutine = StartCoroutine(RocketMoveCo(_rocketMoveSpeed));
        }
    }
    public void RocketPunchUnready()
    {
        preparedHandCnt--;
    }

    public IEnumerator RocketMoveCo(float _rocketMoveSpeed)
    {
        HeadVector3 = playerCamera.transform.position + playerCamera.transform.forward; // ��ó�� �߻��� ��ġ
        HeadDir = Quaternion.LookRotation(HeadVector3); // �߻��� ����
        GameObject rocketCubeGo = Instantiate(rocketCubePrefab, HeadVector3, HeadDir); // �ν��Ͻ�ȭ ����
        rocketCube = rocketCubeGo.GetComponent<RocketCube>(); // Ʈ���� �߻��� ���⼭ ��ȣ�� �´�
        Vector3 curPlayerPosition = Vector3.zero; // ���������� ���ƿ;��ϴ� ��ġ ��������
        Vector3 catchedPosition = Vector3.zero; // ���� ���� ��ġ ��������
        float t = 0;

        while (t == 0 && 2 > preparedHandCnt) // �غ� �� ���� �ϳ� �� ��� �÷��� �տ� �׻� ��ġ
        {
            rocketCubeGo.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            rocketCubeGo.transform.rotation = playerCamera.transform.rotation;
            if (preparedHandCnt == 0)
            {
                Destroy(rocketCubeGo); // ���� ������Ʈ �ı�
                Debug.Log("�ı�");
                rocketCubeGo = null;  // �η� ����
                yield break; // �ڷ�ƾ ����
            }
            yield return null;
        }
        if (preparedHandCnt == 2) // �غ�� ���� �ΰ� �� ���
        {
            while (t <= 3)
            {
                if (!rocketCube.iscatched) // ������ Ʈ���� �Ǳ� ������
                {
                    //rocketCubeGo.transform.position = HeadVector3 * Time.deltaTime * _rocketMoveSpeed; // �� ������ ��� �������� ���ư�
                    rocketCubeGo.transform.Translate(HeadVector3 * Time.deltaTime * _rocketMoveSpeed); // �� ������ ��� �������� ���ư�
                }
                if (rocketCube.iscatched) // ������ Ʈ���� �Ǹ� 
                {
                    catchedPosition = rocketCube.catchPosition; //���� ��ġ ����
                    curPlayerPosition = playerCamera.transform.forward; // �÷��̾� ��ġ ����
                    if (1 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(rocketCubeGo.transform.position, catchedPosition, t); // ���õ� ������Ʈ�� ť�� �̵�
                    }
                    if (t >= 1 && 2 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(catchedPosition, HeadVector3, t - 1); // �߻��� ��ġ�� ť�� �̵�
                    }
                    if (t >= 2)
                    {
                        //rocketCubeGo.transform.position = Vector3.Lerp(HeadVector3, curPlayerPosition, t - 2); // �� ���� �÷��� ������ġ�� ť�� �̵�
                        rocketCubeGo.transform.position = Vector3.Lerp(curPlayerPosition, HeadVector3, 3 - t); // �� ���� �÷��� ������ġ�� ť�� �̵�
                    }

                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
    #endregion
}
