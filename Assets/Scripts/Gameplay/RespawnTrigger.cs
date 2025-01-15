using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform respawnTr = null;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reaspawn Trigger Enter!");

        if (respawnTr != null)
        {
            other.transform.root.position = new Vector3(respawnTr.position.x, respawnTr.position.y, respawnTr.position.z);
            Debug.Log(other.transform.root.name);
        }
    }
}
