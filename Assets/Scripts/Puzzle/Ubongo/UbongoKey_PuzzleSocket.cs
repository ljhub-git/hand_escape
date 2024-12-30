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
    private void OnEnable() // Ȱ��ȭ��
    {
        socket.selectEntered.AddListener(ObjectSnapped);
        socket.selectExited.AddListener(ObjectRemoved);
        //Debug.Log("OnEnable");
    }
    private void OnDisable() // ��Ȱ��ȭ��
    {
        socket.selectEntered.RemoveListener(ObjectSnapped);
        socket.selectExited.RemoveListener(ObjectRemoved);
        //Debug.Log("OnDisable");
    }
    private void ObjectSnapped(SelectEnterEventArgs _arg0) // ������Ʈ�� ����(���°� ������)�ϰ�
    {
        var snappedObjectName = _arg0.interactableObject;
        attachTransfrom.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation); // ���� ����� ������ �ٲٴ� �Լ�
                                                                                                                 //Debug.Log("����");
        if (IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation) ||
            (isDoubleCorrectRotationX && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.right)) ||
            (isDoubleCorrectRotationY && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.up)) ||
            (isDoubleCorrectRotationZ && IsRotationMatching(attachTransfrom.transform.rotation, CorrectPuzzlePiece.transform.rotation, Vector3.forward)))
        {
            if (snappedObjectName.transform.name == CorrectPuzzlePiece.name && isCorrectPosition)// �̸��� �������� ��ġ�� ���
            {
                snappedObjectName.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation);
                linkedPuzzleManager.completedPuzzle(); //�Ŵ��� �Լ� ȣ�� (�Ϸ�� ���񰹼�++,����ϼ�Ȯ��)
                                                       //Debug.Log("�������");
            }
        }
    }
    private void ObjectRemoved(SelectExitEventArgs _arg0)
    {
        var removedObjectName = _arg0.interactableObject;
        //Debug.Log("����");
        if (removedObjectName.transform.name == CorrectPuzzlePiece.name && isCorrectPosition) //�̸��� �������� ��ġ�� ��� 
        {
            linkedPuzzleManager.PuzzlePieceRemoved(); //�Ŵ��� �Լ� ȣ��(�Ϸ�� ���񰹼�--)
            //Debug.Log("�������");
        }
    }

    private Quaternion GetNearestOrthogonalRotation(Quaternion currentRotation)
    {
        // ���� �����̼��� EulerAngles�� ��ȯ
        Vector3 eulerAngles = currentRotation.eulerAngles;

        // ������ 90���� ������ ���� ����� ���������� ����
        eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
        eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
        eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;

        // ��ȯ�� EulerAngles�� Quaternion�� �����Ͽ� ��ȯ
        return Quaternion.Euler(eulerAngles);
    }
    private bool IsRotationMatching(Quaternion attachRotation, Quaternion correctRotation, Vector3 axis = default)
    {
        // �⺻ ȸ�� ��ġ Ȯ��
        if (Quaternion.Angle(attachRotation, correctRotation) < 0.1f)
        {
            return true;
        }

        // 180�� ȸ������ ���
        if (axis != default)
        {
            // �� �࿡ ���� 180�� ȸ���� �� ���
            Quaternion rotatedCorrect = Quaternion.AngleAxis(180, axis) * correctRotation;
            if (Quaternion.Angle(attachRotation, rotatedCorrect) < 0.1f)
            {
                return true;
            }
        }

        return false;
    }

}
