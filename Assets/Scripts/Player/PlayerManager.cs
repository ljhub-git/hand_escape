using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Transform leftHandTr = null;
    [SerializeField]
    private Transform rightHandTr = null;

    private float moveSpeed = 180f; // 이동 속도
    private float rocketMoveSpeed = 180f; // 로켓 이동 속도
    private MovementManager movementManager; // 하위 매니저 연결
    public bool isStitting = false;

    private void Awake()
    {
        movementManager = GetComponent<MovementManager>();
    }
    #region 이동 정지
    public void MoveFoward()
    {

        movementManager.MoveFoward(moveSpeed);
    }    
    public void MoveBack()
    {

        movementManager.MoveBack(moveSpeed);
    }

    public void StopMove()
    {
        movementManager.Stop();
    }
    #endregion
    #region 좌우 회전
    public void LeftRotate()
    {
        movementManager.L_Rotate90();
    }    
    public void RightRotate()
    {
        movementManager.R_Rotate90();
    }
    #endregion
    #region 로켓펀치
    public void RocketPunchLaunchReadyR()
    {
        movementManager.RocketPunchReadyR(rocketMoveSpeed);
    }   
    public void RocketPunchLaunchReadyL()
    {
        movementManager.RocketPunchReadyL(rocketMoveSpeed);
    }     
    public void RocketPunchLaunchUnreadyR()
    {
        movementManager.RocketPunchUnreadyR();
    }        
    public void RocketPunchLaunchUnreadyL()
    {
        movementManager.RocketPunchUnreadyL();
    }
    #endregion
    #region 앉기 서기 모드
    public void setPlayerHeight(float _height)
    {
        movementManager.characterControllerHeight(_height);
    }
    #endregion

    public float GetHandDistance()
    {
        return Vector3.Distance(leftHandTr.position, rightHandTr.position);
    }
}
