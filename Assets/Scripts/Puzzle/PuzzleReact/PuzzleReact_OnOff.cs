using UnityEngine;

public class PuzzleReact_OnOff : PuzzleReactObject
{
    private bool isOn = false;

    public bool IsOn
    {
        get { return isOn; }
    }
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        isOn = true;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();
        isOn = false;
    }
}
