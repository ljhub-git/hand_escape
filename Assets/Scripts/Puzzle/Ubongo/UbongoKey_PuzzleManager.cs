using UnityEngine;
public class UbongoKey_PuzzleManager : MonoBehaviour
{
    [SerializeField] private int numOfComplete = 0;
    private int curComplete = 0;
    private PuzzleObject puzzleObj = null;

    private void Awake()
    {
        puzzleObj = GetComponent<PuzzleObject>();
    }

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
            puzzleObj.SolvePuzzle();
        }
    }

    public void PuzzlePieceRemoved()
    {
        curComplete--;
    }
}
