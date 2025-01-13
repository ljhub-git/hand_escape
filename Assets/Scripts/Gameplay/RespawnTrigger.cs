using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform respawnTr = null;

    private void OnTriggerEnter(Collider other)
    {
        if (respawnTr != null)
            other.transform.root.position = respawnTr.position;
    }
}
