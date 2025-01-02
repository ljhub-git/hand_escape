using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private Jigsaw_PuzzleManager puzzleManager;
    private Vector3 correctPosition;
    private bool isPlaced = false;

    public void SetManager(Jigsaw_PuzzleManager manager)
    {
        puzzleManager = manager;
    }

    public void SetCorrectPosition(Vector3 position)
    {
        correctPosition = position;
    }

    public bool IsCorrectPosition()
    {
        return Vector3.Distance(transform.position, correctPosition) < 0.1f;
    }

    public void PlaceTile()
    {
        if (isPlaced) return;

        isPlaced = true;
        transform.position = correctPosition;

        if (puzzleManager != null)
        {
            puzzleManager.OnTilePlaced(this);
        }
    }

    public void DisableTileCollider()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
}
