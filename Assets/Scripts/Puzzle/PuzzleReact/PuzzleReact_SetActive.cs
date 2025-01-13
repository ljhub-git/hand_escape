using UnityEngine;

public class PuzzleReact_SetActive : PuzzleReactObject
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

        // gameObject.SetActive(true);
        if(meshRenderer != null)
            meshRenderer.enabled = true;
        if(col != null)
            col.enabled = true;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();

        // gameObject.SetActive(false);
        if(meshRenderer != null)
            meshRenderer.enabled = false;
        if(col != null)
            col.enabled = false;
    }
}
