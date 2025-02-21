using UnityEngine;
using System.Collections;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //캐릭터 컨트롤러
    private Coroutine moveCoroutine; // 코루틴을 추적할 변수
    private Camera playerCamera; // 카메라 연결 (카메라 방향 벡터 구하기용);
    private Vector3 HeadVector3 = Vector3.zero; // 헤드 위치

    [SerializeField]// 로켓 펀치하면 아이템을 가져오기 위한 큐브 
    GameObject rocketCubePrefab = null;
    RocketCube rocketCube = null; // 트리거 발생시 받아올 클래스
    public int preparedHandCntR = 0;
    public int preparedHandCntL = 0;
    private Coroutine rocketMoveCoroutine; // 코루틴을 추적할 변수
    private GameObject rocketCubeGo = null;
    float positionSmoothTime = 0.1f;  // 위치 부드럽게 처리하는 시간
    float rotationSmoothTime = 0.1f;  // 회전 부드럽게 처리하는 시간
    Vector3 velocity = Vector3.zero;  // 위치 보간에 사용될 변수
    Quaternion angularVelocity = Quaternion.identity;  // 회전 보간에 사용될 변수


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
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
    }    
    private IEnumerator MoveBackCo(float _moveSpeed) //PlayerManager에서 신호를 받으면 실행 시킬 코루틴 후진 이동
    {
        while (true)
        {
            HeadVector3 = playerCamera.transform.forward;
            characterController.SimpleMove(HeadVector3 * _moveSpeed * Time.fixedDeltaTime * -1f);
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

    public void RocketPunchReadyR(float _rocketMoveSpeed)
    {
        preparedHandCntR++;
        if (preparedHandCntR < 1) // 1 미만 경우가 생겼을 시
        {
            preparedHandCntR = 1; // 1 로 초기화
        }
        if (preparedHandCntR > 1) // 1 초과 할 경우가 생겼을 시
        {
            preparedHandCntR = 1; // 1 로 초기화
        }
        RocketPunchReady(_rocketMoveSpeed);
    }
    public void RocketPunchReadyL(float _rocketMoveSpeed)
    {
        preparedHandCntL++;
        if (preparedHandCntL < 1) // 1 미만 경우가 생겼을 시
        {
            preparedHandCntL = 1; // 1 로 초기화
        }
        if (preparedHandCntL > 1) // 1 초과 할 경우가 생겼을 시
        {
            preparedHandCntL = 1; // 1 로 초기화
        }
        RocketPunchReady(_rocketMoveSpeed);
    }
    public void RocketPunchReady(float _rocketMoveSpeed)
    {
        //Debug.Log("로켓펀치 준비된 손 갯수 : " + preparedHandCnt);
        if ((preparedHandCntL + preparedHandCntR) == 1) // 한손이라도 준비 되면
        {       
            // 이미 실행 중인 코루틴이 있다면 먼저 멈추고, 새로운 코루틴 시작
            if (rocketMoveCoroutine != null)
            {
                if (rocketCubeGo)
                {
                    if (!rocketCube.IsChangedGravityListClear()) // 중력을 비활성화 했음을 확인하는 리스트가 남아있다면
                    {
                        rocketCube.UseGravity(); // 비활성화 중력 재활성화
                    }
                    if (rocketCube.transform.childCount > 3)
                    {
                        rocketCube.ParentNull(); // 자식 해제
                    }
                    if (rocketCube.transform.childCount == 3)
                    {
                        Destroy(rocketCubeGo); // 로켓 큐브 파괴
                        rocketCubeGo = null;  // 널로 설정
                    }
                }
                StopCoroutine(rocketMoveCoroutine);
                rocketMoveCoroutine = null;
                RocketPunchReady(_rocketMoveSpeed);
                return;
            }
            if (rocketCubeGo && rocketCube.isFired == false)
            {
                if (!rocketCube.IsChangedGravityListClear()) // 중력을 비활성화 했음을 확인하는 리스트가 남아있다면
                {
                    rocketCube.UseGravity(); // 비활성화 중력 재활성화
                }
                if (rocketCube.transform.childCount > 3)
                {
                    rocketCube.ParentNull(); // 자식 해제
                }
                if (rocketCube.transform.childCount == 3)
                {
                    Destroy(rocketCubeGo); // 로켓 큐브 파괴
                    rocketCubeGo = null;  // 널로 설정
                }
            }
            rocketMoveCoroutine = StartCoroutine(RocketMoveCo(_rocketMoveSpeed));
        }
    }
    public void RocketPunchUnreadyR()
    {
        preparedHandCntR--;
        if(preparedHandCntR < 0) // 0 미만으로 내려가는 경우가 생겼을 시
        {
            preparedHandCntR = 0; // 0 으로 초기화
        }
        if (preparedHandCntR > 0) // 0 초과 되는 경우
        {
            preparedHandCntR = 0; // 0 으로 초기화
        }

    }    
    public void RocketPunchUnreadyL()
    {
        preparedHandCntL--;
        if(preparedHandCntL < 0) // 0 미만으로 내려가는 경우가 생겼을 시
        {
            preparedHandCntL = 0; // 0 으로 초기화
        }
        if (preparedHandCntL > 0 ) // 0 초과 되는 경우
        {
            preparedHandCntL = 0; // 0 으로 초기화
        }
    }

    public IEnumerator RocketMoveCo(float _rocketMoveSpeed)
    {
        rocketCubeGo = Instantiate(rocketCubePrefab, HeadVector3, Quaternion.identity); // 인스턴스화 생성
        rocketCube = rocketCubeGo.GetComponent<RocketCube>(); // 트리거 발생시 여기서 신호가 온다
        Vector3 curPlayerPosition = Vector3.zero; // 최종적으로 돌아와야하는 위치 지역변수
        Vector3 catchedPosition = Vector3.zero; // 물건 잡힌 위치 지역변수
        float t = 0; // 오브젝트에 닿은 후 돌아오는 경과 시간
        rocketCubeGo.transform.position = playerCamera.transform.position + playerCamera.transform.forward; // 큐브 position 초기화
        rocketCubeGo.transform.rotation = playerCamera.transform.rotation;// 큐브 rotation 초기화
        Debug.Log("발사준비");
        while (t == 0 && (preparedHandCntL + preparedHandCntR) == 1) //준비 된 손이 하나고 발사 경과 시간이 0 일 경우  플레어 앞에 항상 위치
        {
            //rocketCubeGo.transform.position = playerCamera.transform.position + playerCamera.transform.forward; // 매 프레임 플레이어 카메라 앞에 위치
            //rocketCubeGo.transform.rotation = playerCamera.transform.rotation; // 매 프레임 카메라 로테이션이랑 일치 시킴
            //yield return null;
            // 카메라 위치 보간 (부드럽게 이동)
                rocketCubeGo.transform.position = Vector3.SmoothDamp(
                rocketCubeGo.transform.position,
                playerCamera.transform.position + playerCamera.transform.forward,
                ref velocity,
                positionSmoothTime
            );

            // 카메라 회전 보간 (부드럽게 회전)
                rocketCubeGo.transform.rotation = Quaternion.Slerp(
                rocketCubeGo.transform.rotation,
                playerCamera.transform.rotation,
                rotationSmoothTime
            );

            yield return null;
        }
        if (preparedHandCntR == 0 && preparedHandCntL == 0)
        {
            Destroy(rocketCubeGo); // 게임 오브젝트 파괴
            Debug.Log("발사준비 해제");
            rocketCubeGo = null;  // 널로 설정
            yield break; // 코루틴 종료
        }
        if (preparedHandCntR == 1 && preparedHandCntL == 1) // 준비된 손이 두개 일 경우
        {
            Debug.Log("발사");
            rocketCube.FireSound();
            HeadVector3 = playerCamera.transform.position + playerCamera.transform.forward;
            rocketCube.RemeberCatcherPosition(HeadVector3);
            Vector3 HeadDir = Vector3.Normalize(playerCamera.transform.forward);
            rocketCube.isFired = true;
            while (t <= 4)
            {
                
                if (!rocketCube.iscatched) // 물건이 트리거 되기 전까지
                {
                    //rocketCubeGo.transform.position = HeadVector3 * Time.deltaTime * _rocketMoveSpeed; 
                    rocketCubeGo.transform.position += (HeadDir * Time.deltaTime * _rocketMoveSpeed * 0.05f); // 매 프레임 헤드 정면으로 나아감
                    //rocketCubeGo.transform.Translate(HeadDir * _rocketMoveSpeed * 0.0005f);
                }
                if (rocketCube.iscatched && rocketCubeGo != null) // 물건이 트리거 되고 게임오브젝트가 null 상태가 아니라면
                {
                    if (t == 0)
                    {
                        catchedPosition = rocketCube.catchedObjectPosition; //잡힌 오브젝트 위치 맨처음 저장
                    }
                    curPlayerPosition = playerCamera.transform.position + (playerCamera.transform.forward * 0.5f); // 플레이어 앞 위치 매 프레임 갱신
                    if (1 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(rocketCubeGo.transform.position, catchedPosition, t); // 감시된 오브젝트로 큐브 이동
                    }
                    if (t >= 1 && 2 > t)
                    {
                        rocketCubeGo.transform.position = Vector3.Lerp(catchedPosition, rocketCube.WhoCatchMe(), t - 1); // 발사한 위치로 큐브 이동
                    }
                    if (t >= 2 && 3 > t)
                    {
                        //rocketCubeGo.transform.position = Vector3.Lerp(HeadVector3, curPlayerPosition, t - 2); //
                        rocketCubeGo.transform.position = Vector3.Lerp(curPlayerPosition, rocketCube.WhoCatchMe(), 3 - t); //  그 다음 플레이 발사한 위치에서 최종위치로 큐브 이동
                    }
                    if (t >= 3.5)
                    {
                        if (rocketCubeGo)
                        {
                            if (!rocketCube.IsChangedGravityListClear()) // 중력을 비활성화 했음을 확인하는 리스트가 남아있다면
                            {
                                rocketCube.UseGravity(); // 비활성화 중력 재활성화
                            }
                            if (rocketCube.transform.childCount > 3)
                            {
                                rocketCube.ParentNull(); // 자식 해제
                            }
                            if (rocketCube.transform.childCount == 3)
                            {
                                Destroy(rocketCubeGo); // 로켓 큐브 파괴
                            }
                        }
                    }
                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
    #endregion
    #region 앉기서기모드
    public void characterControllerHeight(float _height)
    {
        _height = Mathf.Clamp(_height, 0.5f, 1.8f);
        characterController.height = _height;
    }
    #endregion
}
