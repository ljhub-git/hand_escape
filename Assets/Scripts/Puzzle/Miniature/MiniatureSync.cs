using System.Collections;
using UnityEngine;

public static class MiniatureSync
{
    public static IEnumerator SyncTransform(Miniature miniature, float goalAngle)
    {
        Transform target = miniature.GetTarget();
        float timeElapsed = 0f;
        Quaternion initialRotation = target.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, goalAngle, 0);

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * 0.1f;
            target.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed);
            yield return null;
        }
    }
}
