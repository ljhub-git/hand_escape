using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private Transform spawnTr = null;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Hand"))
        {
            Instantiate(other.transform.parent.gameObject, spawnTr.position, Quaternion.identity);
        }
    }
}
