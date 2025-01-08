using UnityEngine;

public class ShadowHandOnOffReact : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        gameObject.SetActive(true);
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();
        gameObject.SetActive(false);
    }
}
