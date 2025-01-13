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
        if (meshRenderer != null)
            meshRenderer.enabled = false;
        if(col != null)
            col.enabled = false;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();
        if (meshRenderer != null)
            meshRenderer.enabled = true;
        if (col != null)
            col.enabled = true;
    }
}
