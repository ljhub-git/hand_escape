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
    public void OnTriggerEnter(Collider other) // 손이 들어오면
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
    public void OnTriggerExit(Collider other) // 손이 나가면 
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
