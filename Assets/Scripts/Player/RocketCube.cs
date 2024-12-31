using UnityEngine;

public class RocketCube : MonoBehaviour
{
    public bool isFired = false; // 로켓펀치를 했는지 확인하는 bool
    public bool iscatched = false; // 물건을 자식으로 만들었는지 확인하는 bool
    private BoxCollider BoxCollider = null; //현 오브젝트 박스 콜라이더 가져옴
    private GameObject catchedObject = null; // Trigger 오브젝트를 자식으로 만들기 위해 컴퍼넌트 가져옴
    public Vector3 catchedObjectPosition = Vector3.zero; // Trigger 오브젝트 position 값 저장하기 위함
    private Rigidbody catchedObjectRb = null; // Rigidbody useGravitiy를 온오프 하기위해 컴퍼넌트 가저옴
    private Vector3 catcherPosition = Vector3.zero;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true; // 박스 콜라이더 활성화
        BoxCollider.isTrigger = true; // 박스 콜라이더 isTrigger 활성화

    }
    private void OnTriggerEnter(Collider other) // 트리거 발생하면
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
        if (catchedObject == null && isFired) // 잡힌 오브젝트가 없고 로켓펀치를 했으면
        {
            other.transform.SetParent(transform); // 현재 객체의 자식으로 만듦
            iscatched = true;
            catchedObject = other.gameObject;
            catchedObjectPosition = catchedObject.transform.position;
            catchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
            if (catchedObjectRb.useGravity == true)
            {
                catchedObjectRb.useGravity = false; // 중력이 적용중이면 중력 비활성화
            }

        }
        if (catchedObject != null) // 잡힌 오브젝트가 있으면
        {
            Debug.Log("두개 이상 못 가져옴"); 
        }
    }
    public void ParentNull() // 자식을 자식이 아닌 상태로 만들기
    {
        catchedObject.transform.SetParent(null);
    }
    public void UseGravity()
    {
        catchedObjectRb.useGravity = true;
    } 
    public void RemeberCatcherPosition(Vector3 _catcherPosition)
    {
        catcherPosition = _catcherPosition;
    }
    public Vector3 WhoCatchMe()
    {
        return catcherPosition;
    }
}
