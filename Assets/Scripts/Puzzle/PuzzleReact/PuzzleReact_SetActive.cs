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
        meshRenderer.enabled = true;
        col.enabled = true;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleSolved();

        // gameObject.SetActive(false);
        meshRenderer.enabled = false;
        col.enabled = false;
    }
}
