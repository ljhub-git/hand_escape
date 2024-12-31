using UnityEngine;

public class WaitingRoomManager : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        Debug.Log("Waiting Room Puzzle Solved!");
    }


}
