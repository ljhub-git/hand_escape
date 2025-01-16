using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Transform leftHandTr = null;
    [SerializeField]
    private Transform rightHandTr = null;

    private float moveSpeed = 180f; // �̵� �ӵ�
    private float rocketMoveSpeed = 180f; // ���� �̵� �ӵ�
    private MovementManager movementManager; // ���� �Ŵ��� ����
    public bool isStitting = false;

    private void Awake()
    {
        movementManager = GetComponent<MovementManager>();
    }
    #region �̵� ����
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
    #region �¿� ȸ��
    public void LeftRotate()
    {
        movementManager.L_Rotate90();
    }    
    public void RightRotate()
    {
        movementManager.R_Rotate90();
    }
    #endregion
    #region ������ġ
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
    #region �ɱ� ���� ���
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
