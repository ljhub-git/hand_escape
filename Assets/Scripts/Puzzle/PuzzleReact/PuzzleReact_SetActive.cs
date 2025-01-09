using UnityEngine;

public class PuzzleReact_SetActive : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        gameObject.SetActive(true);
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleSolved();

        gameObject.SetActive(false);
    }
}
