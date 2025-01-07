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

    }

    public virtual void SolvePuzzle()
    {
        puzzleMng?.OnSolvePuzzle(this);
    }
}
