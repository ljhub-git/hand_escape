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
            puzzleMng.OnResetPuzzle(this);
    }

    public virtual void SolvePuzzle()
    {
        if(puzzleMng != null)
            puzzleMng.OnSolvePuzzle(this);
    }
}
