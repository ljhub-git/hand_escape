using UnityEngine;
using UnityEngine.Events;


public class UbongoKey_PuzzleManager : MonoBehaviour
{
    [SerializeField] private int numOfComplete;
    private int curComplete = 0;
    public UnityEvent onPuzzleComplete;

    
    public void completedPuzzle()
    {
        curComplete++;
        CheckForPuzzleCompletion();
        Debug.Log(curComplete);
    }

    private void CheckForPuzzleCompletion()
    {
        if (curComplete >= numOfComplete)
        {
            onPuzzleComplete.Invoke();
        }
    }

    public void PuzzlePieceRemoved()
    {
        curComplete--;
    }
}
