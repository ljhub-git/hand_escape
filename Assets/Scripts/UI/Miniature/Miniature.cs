using System.Collections;
using UnityEngine;

public class Miniature : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float snapThreshold = 30f;
    [SerializeField] private float[] snapAngles = { 0f, 90f, 180f, 270f };

    [SerializeField] private float rotationSpeed = 5f;

    public float targetAlpha = 0.5f;
    public float fadeDuration = 0.2f;

    private SnapAngle snapAngle = new SnapAngle();
    private ChangeTransparency changeTransparency;

    private Material material;
    private Color originalColor;
    private Coroutine currentCoroutine;
    private Coroutine rotationCoroutine;
    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;
            changeTransparency = new ChangeTransparency(material);
        }
    }

    private void Update()
    {
        float currentYRotation = snapAngle.NormalizeAngle(transform.eulerAngles.y);
        float snappedAngle = snapAngle.GetSnapAngle(currentYRotation, snapAngles, snapThreshold);

        if (snappedAngle >= 0)
        {
            // 목표 각도를 설정
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
            rotationCoroutine = StartCoroutine(SmoothRotation(snappedAngle));
        }

        SyncTransform();
    }

    private IEnumerator SmoothRotation(float targetYRotation)
    {
        float startRotation = transform.eulerAngles.y;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * rotationSpeed;
            float smoothedYRotation = Mathf.LerpAngle(startRotation, targetYRotation, timeElapsed);
            transform.eulerAngles = new Vector3(0, smoothedYRotation, 0);

            yield return null;
        }

        // 스냅된 각도에 도달한 후 투명도 변경을 트리거
        TriggerTransparency();
    }

    private void TriggerTransparency()
    {
        if (material == null) return;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = 
            StartCoroutine(changeTransparency.FadeTransparency(
            originalColor.a, targetAlpha, fadeDuration));
    }

    private void SyncTransform()
    {
        target.rotation = transform.rotation;
    }
}
