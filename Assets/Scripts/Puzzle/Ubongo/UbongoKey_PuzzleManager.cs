using UnityEngine;
using UnityEngine.Events;


public class UbongoKey_PuzzleManager : MonoBehaviour
{
    [SerializeField] private int numOfComplete = 0;
    private int curComplete = 0;
    public UnityEvent onPuzzleComplete;

    public void AddNumOfComplete()
    {
        numOfComplete++;
    }
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
