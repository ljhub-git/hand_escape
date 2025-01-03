using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    protected PuzzleManager puzzleMng = null;

    protected void Awake()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    protected virtual void ResetPuzzle()
    {

    }

    protected virtual void SolvePuzzle()
    {
        puzzleMng?.OnSolvePuzzle(this);
    }
}
