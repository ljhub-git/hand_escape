using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ubongo_PuzzleManager : PuzzleObject
{
    [SerializeField] private int numOfComplete = 0;
    private int curComplete = 0;
    public bool isEnd = false;
    private List<Ubongo_PuzzleSocket> puzzleSocketList;

    private void Start()
    {
        puzzleSocketList = new List<Ubongo_PuzzleSocket>(GetComponentsInChildren<Ubongo_PuzzleSocket>());
        numOfComplete = puzzleSocketList.Count;
    }
    public void AddNumOfComplete()
    {
        numOfComplete++;
    }
    public void completedPuzzle()
    {
        curComplete++;
        Debug.Log(curComplete);

    }
    public void SnapCheckComplete()
    {
        Invoke("CheckForPuzzleCompletion", 0.5f);
    }

    private void CheckForPuzzleCompletion()
    {
        if (curComplete >= numOfComplete && isEnd)
        {
            // onPuzzleComplete.Invoke();
            SolvePuzzle();
            Debug.Log("Complete");
        }
    }

    public void PuzzlePieceRemoved()
    {
        curComplete--;
        Debug.Log(curComplete);
    }
}
