using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UbongoKey_PuzzleSocket : MonoBehaviour
{
    [SerializeField] private UbongoKey_PuzzleManager linkedPuzzleManager;
    [SerializeField] private Transform CorrectPuzzlePiece;
    private Quaternion CorrectRotation = Quaternion.identity;
    private XRSocketInteractor socket;
    private Transform attachTransfrom;
    [SerializeField] private bool isCorrectPosition;
    [SerializeField] private bool isDoubleCorrectRotationX;
    [SerializeField] private bool isDoubleCorrectRotationY;
    [SerializeField] private bool isDoubleCorrectRotationZ;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        attachTransfrom = GetComponentInChildren<Transform>();
        CorrectRotation = CorrectPuzzlePiece.transform.rotation;
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
        if (IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation) ||
            (isDoubleCorrectRotationX && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.right)) ||
            (isDoubleCorrectRotationY && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.up)) ||
            (isDoubleCorrectRotationZ && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.forward)))
        {
            if (snappedObjectName.transform.name == CorrectPuzzlePiece.name && isCorrectPosition)// 이름을 조건으로 일치할 경우
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
        if (removedObjectName.transform.name == CorrectPuzzlePiece.name && isCorrectPosition) //이름을 조건으로 일치할 경우 
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
    private bool IsRotationMatching(Quaternion attachRotation, Quaternion correctRotation, Vector3 axis = default)
    {
        // 기본 회전 일치 확인
        if (Quaternion.Angle(attachRotation, correctRotation) < 0.1f)
        {
            return true;
        }

        // 180도 회전까지 고려
        if (axis != default)
        {
            // 각 축에 대해 180도 회전한 값 계산
            Quaternion rotatedCorrect = Quaternion.AngleAxis(180, axis) * correctRotation;
            if (Quaternion.Angle(attachRotation, rotatedCorrect) < 0.1f)
            {
                return true;
            }
        }

        return false;
    }

}
