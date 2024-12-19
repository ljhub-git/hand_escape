using UnityEngine;

public class PuzzleTest : MonoBehaviour, IPuzzleObject
{
    private PuzzleManager puzzleMng = null;

    private void Start()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    public void ResetPuzzle()
    {
        Debug.Log("Reset Puzzle");
    }

    public void SolvePuzzle()
    {
        puzzleMng.OnSolvePuzzle(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        SolvePuzzle();
    }
}
