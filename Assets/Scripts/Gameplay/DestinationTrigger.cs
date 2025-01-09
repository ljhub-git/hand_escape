using UnityEngine;

public class DestinationTrigger : MonoBehaviour
{
    private GameManager gameMng = null;

    private void Awake()
    {
        gameMng = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>() != null)
        {
            gameMng.OnPlayerEnteredDestination();
        }
    }
}
