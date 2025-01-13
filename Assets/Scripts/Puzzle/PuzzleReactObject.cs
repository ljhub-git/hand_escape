using UnityEngine;

public abstract class PuzzleReactObject : MonoBehaviour
{
    public virtual void OnPuzzleSolved()
    {
        Debug.Log($"{name} On Puzzle Solved!");
    }

    public virtual void OnPuzzleReset()
    {
        Debug.Log($"{name} On Puzzle Reset!");
    }
}
