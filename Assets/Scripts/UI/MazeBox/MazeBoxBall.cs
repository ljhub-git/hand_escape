using Unity.VisualScripting;
using UnityEngine;

public class MazeBoxBall : MonoBehaviour
{
    private Rigidbody childRigidbody;
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Vector3 acceleration;

    public float forceThreshold = 20f; // 제어하려는 힘의 임계값

    void Start()
    {
        childRigidbody = GetComponent<Rigidbody>();
        previousPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        // 자식의 현재 속도 계산
        velocity = (transform.localPosition - previousPosition) / Time.fixedDeltaTime;
        acceleration = (velocity - (previousPosition - transform.localPosition) / Time.fixedDeltaTime) / Time.fixedDeltaTime;

        previousPosition = transform.localPosition;
        Debug.LogFormat("accel : {0}",acceleration);
        // 일정 힘 이상 가해지면 제어
        if (acceleration.magnitude > forceThreshold)
        {
            ControlChild();
        }
        else
        {
            childRigidbody.isKinematic = false;
        }
    }

    void ControlChild()
    {
        transform.localPosition = previousPosition;
        //// 제어 로직 예: 자식의 속도나 위치를 제한하는 코드 작성
        //childRigidbody.linearVelocity = Vector3.zero;
        childRigidbody.isKinematic = true;
    }
}
