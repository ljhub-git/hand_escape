using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HandPhysics : MonoBehaviour
{
    public Transform target = null;
    private Rigidbody rb = null;

    public float smoothPositionFactor = 10f;
    public float smoothRotationFactor = 10f;

    public Renderer nonPhysicalHand;
    public float showNonPhysicalHandDistance = 0.05f;

    private Collider[] handColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<CapsuleCollider>();
        rb.linearDamping = 10f;  // �ӵ� ����
        rb.angularDamping = 10f;  // ȸ�� ����
        rb.mass = 100f;
    }

    public void EnableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = true;
        }
    }
    public void DisableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position); // ������ �񹰸� ������Ʈ �Ÿ� ���� ��

        if (distance > showNonPhysicalHandDistance) // �Ѱ��� �Ÿ��� ���� �����ְ� �Ⱥ����ְ� ��
        {
            nonPhysicalHand.enabled = true;
        }
        else
        {
            nonPhysicalHand.enabled = false;
        }

    }

    void FixedUpdate()
    {
        //position 
        //rb.linearVelocity = (target.position - transform.position)/Time.fixedDeltaTime;
        Vector3 targetPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothPositionFactor);
        rb.MovePosition(smoothedPosition);

        //rotatoin 
        /*
        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
        */

        Quaternion targetRotation = target.rotation;
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothRotationFactor);
        rb.MoveRotation(smoothedRotation);
    }


}
