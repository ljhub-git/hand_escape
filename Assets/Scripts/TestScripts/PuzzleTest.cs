using UnityEngine;

public class PuzzleTest : PuzzleObject
{
    private PuzzleManager puzzleMng = null;

    private void Start()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        SolvePuzzle();
    }
}
