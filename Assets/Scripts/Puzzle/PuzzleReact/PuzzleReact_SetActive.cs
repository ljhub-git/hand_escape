public class PuzzleReact_SetActive : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        gameObject.SetActive(true);
    }
}
