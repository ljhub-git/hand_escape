using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Ubongo_PuzzleSocketNull : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    private Transform attachTransfrom;
    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        attachTransfrom = transform;
    }
    private void OnEnable() // 활성화시
    {
        socket.selectEntered.AddListener(ObjectSnapped);
        //Debug.Log("OnEnable");
    } 
    private void OnDisable() // 비활성화시
    {
        socket.selectEntered.RemoveListener(ObjectSnapped);
        //Debug.Log("OnDisable");
    }
    private void ObjectSnapped(SelectEnterEventArgs _arg0) // 오브젝트가 스냅(딱맞게 들어오게)하고
    {
        var snappedObjectName = _arg0.interactableObject;
        attachTransfrom.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation); // 오브젝트가 스냅될 rotation을 가장 가까운 직교로 바꾸는 함수
    }
    private Quaternion GetNearestOrthogonalRotation(Quaternion currentRotation) 
    {
        // 현재 로테이션을 EulerAngles로 변환
        Vector3 eulerAngles = currentRotation.eulerAngles;

        // 각도를 90도로 나누어 가장 가까운 직교값으로 맞춤
        eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
        eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
        eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;

        // 변환된 EulerAngles로 Quaternion을 생성하여 반환
        return Quaternion.Euler(eulerAngles); //eulerAngles 는 -180 ~ 180으로 정규화됌
    }
}
