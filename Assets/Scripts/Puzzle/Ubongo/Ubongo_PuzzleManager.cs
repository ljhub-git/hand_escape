using UnityEngine;
using UnityEngine.Events;

public class Ubongo_PuzzleManager : MonoBehaviour
{
    [SerializeField] private int numOfCompleteSpace;
    private int curCompleteSpace = 0;

    public UnityEvent onPuzzleComplete;

    public void completedPuzzle(int _spacePlus, int _spaceMinus)
    {
        curCompleteSpace += _spacePlus;
        curCompleteSpace -= _spaceMinus;
        CheckForPuzzleCompletion();
    }

    private void CheckForPuzzleCompletion()
    {
        if (curCompleteSpace >= numOfCompleteSpace)
        {
            onPuzzleComplete.Invoke();
        }
    }

    public void PuzzlePieceRemoved(int _spaceMinus)
    {
        curCompleteSpace--;
    }
}
