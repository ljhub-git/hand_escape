using UnityEngine;

public class RocketCube : MonoBehaviour
{
    public bool iscatched = false;
    public Vector3 catchPosition = Vector3.zero;
    private BoxCollider BoxCollider =null;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true; // 박스 콜라이더 활성화
        BoxCollider.isTrigger = true; // 박스 콜라이더 isTrigger 활성화

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
        iscatched = true;
        catchPosition = other.transform.position;
    }
}
