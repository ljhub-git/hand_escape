using UnityEngine;
using System.Collections;

public class PuzzleReact_FallDown : PuzzleReactObject
{
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        StartCoroutine(MoveUp());
        //transform.position += Vector3.up;
    }
    private IEnumerator MoveUp()
    {
        float t = 0;
        Vector3 startPos = transform.position;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, startPos + Vector3.up, t);
            yield return null;
        }
        transform.position = startPos + Vector3.up;       
    }
}
