using System.Collections;
using UnityEngine;

public class PuzzleReact_SmoothRotate : PuzzleReactObject
{
    public GameObject targetObj = null;

    public float goalAngle = 90f;

    public float rotateTime = 1f;

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        StartCoroutine(SmoothRotate());
    }

    public IEnumerator SmoothRotate()
    {
        Transform target = targetObj.transform;
        float timeElapsed = 0f;
        Quaternion initialRotation = target.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, goalAngle, 0);

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime / rotateTime;
            target.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed);
            yield return null;
        }
    }
}
