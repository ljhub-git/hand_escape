using UnityEngine;
using System.Collections;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //ĳ���� ��Ʈ�ѷ�
    private Coroutine moveCoroutine; // �ڷ�ƾ�� ������ ����
    private Camera playerCamera; // ī�޶� ���� (ī�޶� ���� ���� ���ϱ��);
    private Vector3 HeadVector3 = Vector3.zero; // ��� ����

    [SerializeField]// ���� ��ġ�ϸ� �������� �������� ���� ť�� 
    GameObject rocketCubePrefab = null;
    public int preparedHandCnt = 0;
    private void Start()
    {
        if (rocketCubePrefab == null)
        {
            rocketCubePrefab = (GameObject)Resources.Load("Characters\\Prefabs\\Rocket_Cube"); // ����ť�� �ε�
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
    public void RocketPunch(float _rocketMoveSpeed)
    {
        preparedHandCnt++;
        if (preparedHandCnt == 2)
        {

        }
    }
    #endregion
}
