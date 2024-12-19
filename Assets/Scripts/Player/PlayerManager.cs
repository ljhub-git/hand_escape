using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour
{
    private float moveSpeed = 60f; // 이동 속도
    private MovementManager movementManager; // 하위 매니저 연결
    private void Awake()
    {
        movementManager = GetComponentInChildren<MovementManager>();
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
}
