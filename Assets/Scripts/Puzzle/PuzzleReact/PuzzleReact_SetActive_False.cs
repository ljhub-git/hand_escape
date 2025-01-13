using UnityEngine;

public class PuzzleReact_SetActive_False : PuzzleReactObject
{
    private MeshRenderer meshRenderer = null;
    private Collider col = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        meshRenderer.enabled = false;
        col.enabled = false;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleSolved();
        meshRenderer.enabled = true;
        col.enabled = true;
    }
}
