using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        SceneManager.LoadScene("S_Stage1");
    }


}
