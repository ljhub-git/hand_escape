using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    protected PuzzleManager puzzleMng = null;

    protected void Awake()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    public virtual void ResetPuzzle()
    {
        if (puzzleMng != null)
        {
            Debug.LogWarning("Puzzle Manager is not exist in hierarchy!");
        }

        puzzleMng.OnResetPuzzle(this);
    }

    public virtual void SolvePuzzle()
    {
        if(puzzleMng != null)
        {
            Debug.LogWarning("Puzzle Manager is not exist in hierarchy!");
        }

        puzzleMng.OnSolvePuzzle(this);
    }
}
