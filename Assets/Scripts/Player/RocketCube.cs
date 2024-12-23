using UnityEngine;

public class RocketCube : MonoBehaviour
{
    public bool isFired = false; // ������ġ�� �ߴ��� Ȯ���ϴ� bool
    public bool iscatched = false; // ������ �ڽ����� ��������� Ȯ���ϴ� bool
    private BoxCollider BoxCollider = null; //�� ������Ʈ �ڽ� �ݶ��̴� ������
    private GameObject catchedObject = null; // Trigger ������Ʈ�� �ڽ����� ����� ���� ���۳�Ʈ ������
    public Vector3 catchedObjectPosition = Vector3.zero; // Trigger ������Ʈ position �� �����ϱ� ����
    private Rigidbody catchedObjectRb = null; // Rigidbody useGravitiy�� �¿��� �ϱ����� ���۳�Ʈ ������
    private bool isRbChanged = false; // Rigidbody�� �ٲ����� �ִ��� üũ
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true; // �ڽ� �ݶ��̴� Ȱ��ȭ
        BoxCollider.isTrigger = true; // �ڽ� �ݶ��̴� isTrigger Ȱ��ȭ

    }
    private void OnTriggerEnter(Collider other) // Ʈ���� �߻��ϸ�
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
        if (catchedObject == null && isFired) // ���� ������Ʈ�� ���� ������ġ�� ������
        {
            other.transform.SetParent(transform); // ���� ��ü�� �ڽ����� ����
            iscatched = true;
            catchedObject = other.gameObject;
            catchedObjectPosition = catchedObject.transform.position;
            catchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
            if (catchedObjectRb.useGravity == true)
            {
                catchedObjectRb.useGravity = false; // �߷��� �������̸� �߷� ��Ȱ��ȭ
                isRbChanged = true;
            }

        }
        if (catchedObject != null) // ���� ������Ʈ�� ������
        {
            Debug.Log("�ΰ� �̻� �� ������"); 
        }
    }
    public void ParentNull() // �ڽ��� �ڽ��� �ƴ� ���·� �����
    {
        catchedObject.transform.SetParent(null);
    }
    public void ReUseGravity()
    {
        if(isRbChanged == true)
        {
            catchedObjectRb.useGravity = true;
        }
        if (isRbChanged == false)
        {
            Debug.Log("I didn't Change Use Gravity bool");
        }
    }
}
