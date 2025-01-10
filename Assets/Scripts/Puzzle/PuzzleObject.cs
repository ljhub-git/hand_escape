using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    protected PuzzleManager puzzleMng = null;

    protected void Awake()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    public void ResetPuzzle()
    {
        if (puzzleMng == null)
        {
            Debug.LogWarning("Puzzle Manager is not exist in hierarchy!");
            return;
        }

        puzzleMng.OnResetPuzzle(this);
    }

    public void SolvePuzzle()
    {
        if(puzzleMng == null)
        {
            Debug.LogWarning("Puzzle Manager is not exist in hierarchy!");
            return;
        }

        puzzleMng.OnSolvePuzzle(this);
    }
}
