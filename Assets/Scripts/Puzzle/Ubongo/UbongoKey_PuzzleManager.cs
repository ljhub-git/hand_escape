using UnityEngine;
public class UbongoKey_PuzzleManager : PuzzleObject
{
    [SerializeField] private int numOfComplete = 0;
    private int curComplete = 0;

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
            SolvePuzzle();
        }
    }

    public void PuzzlePieceRemoved()
    {
        curComplete--;
    }
}
