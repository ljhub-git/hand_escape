using UnityEngine;
using System.Collections;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //캐릭터 컨트롤러
    private Coroutine moveCoroutine; // 코루틴을 추적할 변수
    private Camera playerCamera; // 카메라 연결 (카메라 방향 벡터 구하기용);
    private Vector3 HeadVector3 = Vector3.zero; // 헤드 방향

    [SerializeField]// 로켓 펀치하면 아이템을 가져오기 위한 큐브 
    GameObject rocketCubePrefab = null;
    public int preparedHandCnt = 0;
    private void Start()
    {
        if (rocketCubePrefab == null)
        {
            rocketCubePrefab = (GameObject)Resources.Load("Characters\\Prefabs\\Rocket_Cube"); // 로켓큐브 로딩
        }
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

    }

    #region 전진, 후진, 정지
    public void MoveFoward(float _moveSpeed) // 정면 이동
    {
        // 이미 실행 중인 코루틴이 있다면 먼저 멈추고, 새로운 코루틴 시작
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveFowardCo(_moveSpeed));
    }    
    public void MoveBack(float _moveSpeed) // 후진 이동
    {
        // 이미 실행 중인 코루틴이 있다면 먼저 멈추고, 새로운 코루틴 시작
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveBackCo(_moveSpeed));
    }
    public void Stop() // 정지
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // 코루틴을 멈춘 후, 참조를 null로 초기화
        }
    }


    private IEnumerator MoveFowardCo(float _moveSpeed) //PlayerManager에서 신호를 받으면 실행 시킬 코루틴 정면 이동
    {
        while (true)
        {
            HeadVector3 = playerCamera.transform.forward;
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }    
    private IEnumerator MoveBackCo(float _moveSpeed) //PlayerManager에서 신호를 받으면 실행 시킬 코루틴 후진 이동
    {
        while (true)
        {
            HeadVector3 = playerCamera.transform.forward;
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.deltaTime * -1f);
            yield return null;
        }
    }

    #endregion
    #region 회전
    public void L_Rotate90() // 왼쪽 90도 회전 
    {
        transform.Rotate(-Vector3.up * 90f, Space.Self);
    }
    public void R_Rotate90() // 오른쪽 90도 회전
    {
        transform.Rotate(Vector3.up * 90f, Space.Self);
    }
    #endregion
    #region 로켓펀치
    public void RocketPunch(float _rocketMoveSpeed)
    {
        preparedHandCnt++;
        if (preparedHandCnt == 2)
        {

        }
    }
    #endregion
}
