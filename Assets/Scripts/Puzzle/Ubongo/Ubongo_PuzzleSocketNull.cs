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
    private void OnEnable() // Ȱ��ȭ��
    {
        socket.selectEntered.AddListener(ObjectSnapped);
        //Debug.Log("OnEnable");
    } 
    private void OnDisable() // ��Ȱ��ȭ��
    {
        socket.selectEntered.RemoveListener(ObjectSnapped);
        //Debug.Log("OnDisable");
    }
    private void ObjectSnapped(SelectEnterEventArgs _arg0) // ������Ʈ�� ����(���°� ������)�ϰ�
    {
        var snappedObjectName = _arg0.interactableObject;
        attachTransfrom.transform.rotation = GetNearestOrthogonalRotation(snappedObjectName.transform.rotation); // ������Ʈ�� ������ rotation�� ���� ����� ������ �ٲٴ� �Լ�
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
