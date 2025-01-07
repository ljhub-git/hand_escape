using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UbongoKey_PuzzleSocket : MonoBehaviour
{
    [SerializeField] private UbongoKey_PuzzleManager linkedPuzzleManager;
    [SerializeField] private Transform CorrectPuzzlePiece;
    private XRSocketInteractor socket;
    private Transform attachTransfrom;
    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        attachTransfrom = GetComponentInChildren<Transform>();
        linkedPuzzleManager.AddNumOfComplete();
        transform.tag = "UbongoCol";
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
        //Debug.Log("����");
        {
            if (snappedObjectName.transform.name.Contains(CorrectPuzzlePiece.name))// �̸��� �����ϴ� �������� ��ġ�� ���
            {
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
}
