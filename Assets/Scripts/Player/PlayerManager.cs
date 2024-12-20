using UnityEngine;
using UnityEngine.Events;
public class PlayerManager : MonoBehaviour
{
    private float moveSpeed = 60f; // �̵� �ӵ�
    private MovementManager movementManager; // ���� �Ŵ��� ����
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
}
