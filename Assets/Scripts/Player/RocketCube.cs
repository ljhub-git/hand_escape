using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RocketCube : MonoBehaviour
{
    public bool isFired = false; // 로켓펀치를 했는지 확인하는 bool
    public bool iscatched = false; // 물건을 자식으로 만들었는지 확인하는 bool
    private BoxCollider BoxCollider = null; //현 오브젝트 박스 콜라이더 가져옴
    private GameObject catchedObject = null; // Trigger 오브젝트를 자식으로 만들기 위해 컴퍼넌트 가져옴
    public Vector3 catchedObjectPosition = Vector3.zero; // Trigger 오브젝트 position 값 저장하기 위함
    private List<Rigidbody> catchedObjectRb = new List<Rigidbody>(); // Rigidbody useGravitiy를 온오프 하기위해 컴퍼넌트 가저옴
    private List<bool> isChangedGravity = new List<bool>(); //이 컴퍼넌트에서 중력을 비활성화 했는지 확인하는 bool값
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

        if (other.CompareTag("Ubongo") || other.CompareTag("UbongoCol"))
        {
            iscatched = true;
            catchedObjectPosition = transform.position;
            Debug.Log("우봉고는 로켓펀치로 못 가져옵니다");
            return;
        }

        if (catchedObject == null && isFired) // 잡힌 오브젝트가 없고 로켓펀치를 했으면
        {
            Transform currentParent = other.transform; // 충돌 한 오브젝트 부모 정의
            Transform finalParent = null; //최상위 부모
            while (currentParent.parent != null) // 부모가 있는 동안
            {
                currentParent = currentParent.parent; //부모 재정의
            }
            if (currentParent.parent == null) // 부모가 없으면
            {
                finalParent = currentParent;
                finalParent.transform.SetParent(transform);// 객체를 자식으로 바꿈
            }

            iscatched = true;
            catchedObject = finalParent.gameObject;
            catchedObjectPosition = other.transform.position;
            // GetComponentsInChildren<Rigidbody>()로 배열을 가져오고, 이를 List로 변환
            catchedObjectRb = catchedObject.GetComponentsInChildren<Rigidbody>().ToList();
            XRGrabInteractable catchedObjectGrabInteractable = null;
            catchedObjectGrabInteractable = GetComponentInChildren<XRGrabInteractable>();
            if (catchedObjectGrabInteractable != null)
            {
                // XRGrabInteractable이 비활성화되어 있을 경우, 활성화시킵니다.
                if (!catchedObjectGrabInteractable.enabled)
                {
                    catchedObjectGrabInteractable.enabled = true;
                }
            }
            isChangedGravity.Clear();  // 기존 값 비우기
            for (int n = 0; n < catchedObjectRb.Count; n++)
            {
                Rigidbody rb = catchedObjectRb[n];
                if (rb.useGravity)
                {
                    rb.useGravity = false; // 중력이 적용중이면 중력 비활성화
                    isChangedGravity.Add(true); // 해당 인덱스에 중력 비활성화 기록
                }
                else
                {
                    isChangedGravity.Add(false); // 중력이 적용되지 않으면 false 추가
                }
            }

        }
        if (catchedObject != null) // 잡힌 오브젝트가 있으면
        {
            Debug.Log("두개 이상 못 가져옴"); 
        }
    }
    public void ParentNull() // 자식을 자식이 아닌 상태로 만들기
    {
        if (catchedObject != null)
        {
            catchedObject.transform.SetParent(null);
        }
    }
    public void UseGravity()
    {
        for (int n = 0; n < catchedObjectRb.Count; n++)
        {
            Rigidbody rb = catchedObjectRb[n];

            // 중력이 비활성화되어 있고, 이전에 중력을 비활성화했던 경우
            if (!rb.useGravity && isChangedGravity[n])
            {
                rb.useGravity = true;  // 중력 재활성화
            }
        }
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
