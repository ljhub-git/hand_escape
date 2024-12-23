using UnityEngine;

public class RocketCube : MonoBehaviour
{
    BoxCollider boxCol = null;
    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
    }
}
