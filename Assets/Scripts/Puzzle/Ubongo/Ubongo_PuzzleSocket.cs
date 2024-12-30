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
        {
            if (snappedObjectName.transform.name == CorrectPuzzlePiece.name)// �̸��� �������� ��ġ�� ���
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
        if (removedObjectName.transform.name == CorrectPuzzlePiece.name) //�̸��� �������� ��ġ�� ��� 
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
}
