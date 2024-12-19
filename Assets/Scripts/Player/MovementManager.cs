using UnityEngine;
using System.Collections;
public class MovementManager : MonoBehaviour
{
    private CharacterController characterController; //캐릭터 컨트롤러
    private Coroutine moveCoroutine; // 코루틴을 추적할 변수

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    #region 이동, 정지
    public void MoveFoward(Vector3 _Dir, float _moveSpeed) // 정면 이동
    {
        // 이미 실행 중인 코루틴이 있다면 먼저 멈추고, 새로운 코루틴 시작
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveFowardCo(_Dir, _moveSpeed));
    }
    public void Stop() // 정지
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // 코루틴을 멈춘 후, 참조를 null로 초기화
        }
    }


    private IEnumerator MoveFowardCo(Vector3 _Dir, float _moveSpeed) //PlayerManager에서 신호를 받으면 실행 시킬 코루틴 정면 이동
    {
        while (true)
        {
            characterController.SimpleMove(_Dir * _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
    public void L_Rotate90()
    {
        transform.Rotate(-Vector3.up * 90f, Space.Self);
    }
    public void R_Rotate90()
    {
        transform.Rotate(Vector3.up * 90f, Space.Self);
    }
}
