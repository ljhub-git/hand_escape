using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //캐릭터 컨트롤러
    private Coroutine moveCoroutine; // 코루틴을 추적할 변수
    private Camera playerCamera; // 카메라 연결 (카메라 방향 벡터 구하기용);
    private Vector3 HeadVector3 = Vector3.zero; // 헤드 위치
    private Quaternion HeadDir = Quaternion.identity; // 헤드 방향

    [SerializeField]// 로켓 펀치하면 아이템을 가져오기 위한 큐브 
    GameObject rocketCubePrefab = null;
    RocketCube rocketCube = null; // 트리거 발생시 받아올 클래스
    public int preparedHandCnt = 0;
    private Coroutine rocketMoveCoroutine; // 코루틴을 추적할 변수
    private void Start()
    {
        if (rocketCubePrefab == null) // 프리펩이 없으면 로켓큐브 로딩
        {
            rocketCubePrefab = (GameObject)Resources.Load("Characters\\Prefabs\\Rocket_Cube"); 
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
    public void RocketPunchReady(float _rocketMoveSpeed)
    {
        preparedHandCnt++;
        //Debug.Log("로켓펀치 준비된 손 갯수 : " + preparedHandCnt);
        if (preparedHandCnt == 1) // 카운트 2개가 되면 실행
        {
            // 이미 실행 중인 코루틴이 있다면 실행 안함
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
        HeadVector3 = playerCamera.transform.position + playerCamera.transform.forward; // 맨처음 발사할 위치
        HeadDir = Quaternion.LookRotation(HeadVector3); // 발사할 방향
        GameObject rocketCubeGo = Instantiate(rocketCubePrefab, HeadVector3, HeadDir); // 인스턴스화 생성
        rocketCube = rocketCubeGo.GetComponent<RocketCube>(); // 트리거 발생시 여기서 신호가 온다
        Vector3 curPlayerPosition = Vector3.zero; // 최종적으로 돌아와야하는 위치 지역변수
        Vector3 catchedPosition = Vector3.zero; // 물건 잡힌 위치 지역변수
        float t = 0;

        while (t == 0 && 2 > preparedHandCnt) // 준비 된 손이 하나 일 경우 플레어 앞에 항상 위치
        {
            rocketCubeGo.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            rocketCubeGo.transform.rotation = playerCamera.transform.rotation;
            if (preparedHandCnt == 0)
            {
                Destroy(rocketCubeGo); // 게임 오브젝트 파괴
                Debug.Log("파괴");
                rocketCubeGo = null;  // 널로 설정
                yield break; // 코루틴 종료
            }
            yield return null;
        }
        if (preparedHandCnt == 2) // 준비된 손이 두개 일 경우
        {
            while (t <= 3)
            {
                if (!rocketCube.iscatched) // 물건이 트리거 되기 전까지
                {
                    //rocketCubeGo.transform.position = HeadVector3 * Time.deltaTime * _rocketMoveSpeed; // 매 프레임 헤드 정면으로 나아감
                    rocketCubeGo.transform.Translate(HeadVector3 * Time.deltaTime * _rocketMoveSpeed); // 매 프레임 헤드 정면으로 나아감
                }
                if (rocketCube.iscatched) // 물건이 트리거 되면 
                {
                    catchedPosition = rocketCube.catchPosition; //잡힌 위치 갱신
                    curPlayerPosition = playerCamera.transform.forward; // 플레이어 위치 갱신
                    if (1 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(rocketCubeGo.transform.position, catchedPosition, t); // 감시된 오브젝트로 큐브 이동
                    }
                    if (t >= 1 && 2 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(catchedPosition, HeadVector3, t - 1); // 발사한 위치로 큐브 이동
                    }
                    if (t >= 2)
                    {
                        //rocketCubeGo.transform.position = Vector3.Lerp(HeadVector3, curPlayerPosition, t - 2); // 그 다음 플레이 최종위치로 큐브 이동
                        rocketCubeGo.transform.position = Vector3.Lerp(curPlayerPosition, HeadVector3, 3 - t); // 그 다음 플레이 최종위치로 큐브 이동
                    }

                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
    #endregion
}
