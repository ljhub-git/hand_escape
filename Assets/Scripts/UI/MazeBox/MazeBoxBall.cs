using Unity.VisualScripting;
using UnityEngine;

public class MazeBoxBall : MonoBehaviour
{
    private Rigidbody childRigidbody;
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Vector3 acceleration;

    public float forceThreshold = 20f; // �����Ϸ��� ���� �Ӱ谪

    void Start()
    {
        childRigidbody = GetComponent<Rigidbody>();
        previousPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        // �ڽ��� ���� �ӵ� ���
        velocity = (transform.localPosition - previousPosition) / Time.fixedDeltaTime;
        acceleration = (velocity - (previousPosition - transform.localPosition) / Time.fixedDeltaTime) / Time.fixedDeltaTime;

        previousPosition = transform.localPosition;
        Debug.LogFormat("accel : {0}",acceleration);
        // ���� �� �̻� �������� ����
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
        //// ���� ���� ��: �ڽ��� �ӵ��� ��ġ�� �����ϴ� �ڵ� �ۼ�
        //childRigidbody.linearVelocity = Vector3.zero;
        childRigidbody.isKinematic = true;
    }
}
