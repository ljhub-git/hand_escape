using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Ubongo_PuzzleSocket : MonoBehaviour
{
    [SerializeField] private Ubongo_PuzzleManager linkedPuzzleManager;
    //[SerializeField] private Transform CorrectPuzzlePiece;
    [SerializeField] private XRSocketInteractor socket;
    private BoxCollider BoxCollider;
    private Transform attachTransfrom;
    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        attachTransfrom = GetComponentsInChildren<Transform>()[1];
        BoxCollider = GetComponent<BoxCollider>();
        linkedPuzzleManager.AddNumOfComplete();
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
        //var snappedObjectName = _arg0.interactableObject;
        //attachTransfrom.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation); // ������Ʈ�� ������ rotation�� ���� ����� ������ �ٲٴ� �Լ�
        //Debug.Log("����");
        //{   
        //    if (snappedObjectName.transform.name == CorrectPuzzlePiece.name)// �̸��� �������� ��ġ�� ���
        //    {
        //        linkedPuzzleManager.completedPuzzle(); //�Ŵ��� �Լ� ȣ�� (�Ϸ�� ���񰹼�++,����ϼ�Ȯ��)
        //        //Debug.Log("�������");
        //    }
        //}
        linkedPuzzleManager.isEnd = true;
        linkedPuzzleManager.SnapCheckComplete();
        Debug.Log("linkedPuzzleManager.isEnd = " + linkedPuzzleManager.isEnd);

    }
    private void ObjectRemoved(SelectExitEventArgs _arg0)
    {
        //var removedObjectName = _arg0.interactableObject;
        ////Debug.Log("����");
        //if (removedObjectName.transform.name == CorrectPuzzlePiece.name)// �̸��� ��ġ
        //{
        //    linkedPuzzleManager.PuzzlePieceRemoved(); //�Ŵ��� �Լ� ȣ��(�Ϸ�� ���񰹼�--)
        //    //Debug.Log("�������");
        //}
        linkedPuzzleManager.isEnd = false;
        Debug.Log("linkedPuzzleManager.isEnd = " + linkedPuzzleManager.isEnd);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("����");
        if (other.CompareTag("UbongoCol"))
        {
            //Debug.Log("Ubongo����");
            linkedPuzzleManager.completedPuzzle();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ubongo"))
        {
            transform.rotation = GetNearestOrthogonalRotation(other.transform.rotation);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UbongoCol"))
        {
            linkedPuzzleManager.PuzzlePieceRemoved();
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
        return Quaternion.Euler(eulerAngles); //eulerAngles �� -180 ~ 180���� ����ȭ��
    }
}
