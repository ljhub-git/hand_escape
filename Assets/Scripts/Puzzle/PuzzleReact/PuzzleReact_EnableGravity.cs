using UnityEngine;

public class PuzzleReact_EnableGravity : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = true;
    }
}
