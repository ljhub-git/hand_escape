using UnityEngine;

public class PuzzleReact_SetActiveAndEnableGravity : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        MeshRenderer mr = GetComponent<MeshRenderer>();
        Collider col = GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (mr != null)
            mr.enabled = true;
        if (col != null)
            col.enabled = true;
        if (rb != null)
            rb.useGravity = true;
    }
}
