using System.Collections;
using UnityEngine;

public static class MiniatureRotation
{
    public static IEnumerator CaseRotation(Miniature miniature, float targetYRotation)
    {
        Transform transform = miniature.transform;
        Rigidbody rb = miniature.GetRigidbody();
        float startRotation = transform.eulerAngles.y;
        float timeElapsed = 0f;

        if (rb != null) rb.isKinematic = true;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * miniature.GetRotationSpeed();
            float smoothedYRotation = Mathf.LerpAngle(startRotation, targetYRotation, timeElapsed);
            transform.eulerAngles = new Vector3(0, smoothedYRotation, 0);

            yield return null;
        }

        miniature.TriggerTransparency();

        yield return new WaitForSeconds(1f);

        if (rb != null) rb.isKinematic = false;
        miniature.SetRotationCoroutine(null);

        if (Mathf.Approximately(targetYRotation, 90f))
        {
            Coroutine syncCoroutine = miniature.StartCoroutine(MiniatureSync.SyncTransform(miniature, 90f));
            miniature.MakeDisable();
        }
    }
}
