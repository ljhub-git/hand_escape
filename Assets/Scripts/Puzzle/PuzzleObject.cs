using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    private PuzzleManager puzzleMng = null;

    private void Awake()
    {
        puzzleMng = FindAnyObjectByType<PuzzleManager>();
    }

    public virtual void ResetPuzzle()
    {

    }

    public virtual void SolvePuzzle()
    {
        puzzleMng.OnSolvePuzzle(this);
    }
}
