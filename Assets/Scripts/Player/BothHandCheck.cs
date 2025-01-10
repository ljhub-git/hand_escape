using UnityEngine;

public class BothHandCheck : MonoBehaviour
{
    [SerializeField]
    private PuzzleReact_OnOff leftHandCorrect;
    [SerializeField]
    private PuzzleReact_OnOff rightHandCorrect;

    private PuzzleObject puzzleObject = null;

    private void Awake()
    {
        puzzleObject = GetComponent<PuzzleObject>();
    }

    private void Update()
    {
        if(leftHandCorrect.IsOn && rightHandCorrect.IsOn)
        {
            puzzleObject.SolvePuzzle();
        }
    }
}
