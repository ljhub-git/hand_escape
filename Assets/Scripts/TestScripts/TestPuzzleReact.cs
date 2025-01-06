using TMPro;
using UnityEngine;

public class TestPuzzleReact : PuzzleReactObject
{
    private TextMeshPro tmp = null;

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        tmp.enabled = true;
    }

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }
}
