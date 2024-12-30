using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Ubongo_PuzzleSocket : MonoBehaviour
{
    [SerializeField] private UbongoKey_PuzzleManager linkedPuzzleManager;
    [SerializeField] private Transform CorrectPuzzlePiece;
    private Quaternion CorrectRotation = Quaternion.identity;
    private XRSocketInteractor socket;
    private Transform attachTransfrom;
    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        attachTransfrom = GetComponentInChildren<Transform>();
    }
    private void OnEnable() // 활성화시
    {
        socket.selectEntered.AddListener(ObjectSnapped);
        socket.selectExited.AddListener(ObjectRemoved);
        //Debug.Log("OnEnable");
    }
    private void OnDisable() // 비활성화시
    {
        socket.selectEntered.RemoveListener(ObjectSnapped);
        socket.selectExited.RemoveListener(ObjectRemoved);
        //Debug.Log("OnDisable");
    }
    private void ObjectSnapped(SelectEnterEventArgs _arg0) // 오브젝트가 스냅(딱맞게 들어오게)하고
    {
        var snappedObjectName = _arg0.interactableObject;
        attachTransfrom.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation); // 가장 가까운 직교로 바꾸는 함수
        //Debug.Log("들어옴");
        {
            if (snappedObjectName.transform.name == CorrectPuzzlePiece.name)// 이름을 조건으로 일치할 경우
            {
                snappedObjectName.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation);
                linkedPuzzleManager.completedPuzzle(); //매니저 함수 호출 (완료된 퍼즐갯수++,퍼즐완성확인)
                //Debug.Log("정답들어옴");
            }
        }
    }
    private void ObjectRemoved(SelectExitEventArgs _arg0)
    {
        var removedObjectName = _arg0.interactableObject;
        //Debug.Log("빠짐");
        if (removedObjectName.transform.name == CorrectPuzzlePiece.name) //이름을 조건으로 일치할 경우 
        {
            linkedPuzzleManager.PuzzlePieceRemoved(); //매니저 함수 호출(완료된 퍼즐갯수--)
            //Debug.Log("정답빠짐");
        }
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
        return Quaternion.Euler(eulerAngles);
    }
}
