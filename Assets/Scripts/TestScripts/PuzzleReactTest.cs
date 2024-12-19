using UnityEngine;

public class PuzzleReactTest : MonoBehaviour, IReactToPuzzle
{
    [SerializeField]
    private Color reactColor = Color.white;

    public void OnPuzzleReset()
    {
        
    }

    public void OnPuzzleSolved()
    {
        GetComponentInChildren<MeshRenderer>().material.color = reactColor;
    }
}
