using UnityEngine;

public class RocketCube : MonoBehaviour
{
    public bool iscatched = false;
    public Vector3 catchedObjectPosition = Vector3.zero;
    private BoxCollider BoxCollider =null;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true; // �ڽ� �ݶ��̴� Ȱ��ȭ
        BoxCollider.isTrigger = true; // �ڽ� �ݶ��̴� isTrigger Ȱ��ȭ

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
        iscatched = true;
        catchedObjectPosition = other.transform.position;
    }
}
