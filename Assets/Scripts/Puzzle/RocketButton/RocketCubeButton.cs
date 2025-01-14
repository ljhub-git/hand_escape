using UnityEngine;

public class RocketCubeButton : PuzzleObject
{
    private BoxCollider boxCollider = null;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
            Debug.Log("RocketCubeButton boxCollider == null");        
        if (boxCollider != null)
            Debug.Log("RocketCubeButton boxCollider != null");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.GetComponent<RocketCube>())
        {
            Debug.Log("SolvePuzzle");
            SolvePuzzle();
        }
    }
}
