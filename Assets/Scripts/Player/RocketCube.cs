using UnityEngine;

public class RocketCube : MonoBehaviour
{
    BoxCollider boxCol = null;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rocket Cube OnTriggerEnter");
    }
}
