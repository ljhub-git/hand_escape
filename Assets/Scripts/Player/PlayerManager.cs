using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour
{
    private float moveSpeed = 60f; // 이동 속도
    private Vector3 HeadVector3 = Vector3.zero; // 헤드 방향
    private MovementManager movementManager; // 하위 매니저 연결
    private Camera playerCamera; // 카메라 연결 (헤드 벡터 구하기용);
    private void Awake()
    {
        movementManager = GetComponentInChildren<MovementManager>();
        playerCamera = GetComponentInChildren<Camera>();
    }
    #region 이동 정지
    public void MoveFoward()
    {
        HeadVector3 = playerCamera.transform.forward;
        movementManager.MoveFoward(HeadVector3,moveSpeed);
    }

    public void StopMove()
    {
        movementManager.Stop();
    }
    #endregion
}
