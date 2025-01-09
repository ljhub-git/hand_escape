using UnityEngine;

public class PuzzleReact_SetActive_False : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        gameObject.SetActive(false);
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleSolved();

        gameObject.SetActive(true);
    }
}
