using UnityEngine;

public class DuplicatedTriggerHandMannager : MonoBehaviour
{

    public GameObject duplicatedHandR;
    public GameObject duplicatedHandL;

    private void Awake()
    {
        duplicatedHandR.SetActive(false);
        duplicatedHandL.SetActive(false);
    }
    public void OnTriggerEnter(Collider other) // ���� ������
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics"))
        {
            duplicatedHandR.SetActive(true);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics"))
        {
            duplicatedHandL.SetActive(true);
        }

    }
    public void OnTriggerExit(Collider other) // ���� ������ 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics"))
        {
            duplicatedHandR.SetActive(false);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics"))
        {
            duplicatedHandL.SetActive(false);
        }
    }
}
