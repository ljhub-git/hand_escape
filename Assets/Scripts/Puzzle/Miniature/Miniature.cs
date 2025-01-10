using System.Collections;
using UnityEngine;

public class Miniature : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float snapThreshold = 30f;
    [SerializeField] private float[] snapAngles = { 0f, 90f, 180f, 270f };

    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float targetAlpha = 0.5f;
    [SerializeField] private float fadeDuration = 0.2f;

    private SnapAngle snapAngle = new SnapAngle();
    private ChangeTransparency changeTransparency;

    private Material material;
    private Color originalColor;
    private Coroutine currentCoroutine;
    private Coroutine rotationCoroutine;

    private PuzzleObject puzzleObj = null;

    private Rigidbody rb;

    public bool isInteractable = true;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;
            changeTransparency = new ChangeTransparency(material);
        }

        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isInteractable) return;

        float currentYRotation = snapAngle.NormalizeAngle(transform.eulerAngles.y);
        float snappedAngle = snapAngle.GetSnapAngle(currentYRotation, snapAngles, snapThreshold);

        // 스냅 각도가 유효하면 SmoothRotation 실행
        if (snappedAngle >= 0 && rotationCoroutine == null)
        {
            rotationCoroutine =
                StartCoroutine(CaseRotation(this, snappedAngle));
        }
    }

    public void TriggerTransparency()
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

    public void MakeDisable()
    {
        if (material != null)
        {
            Color color = material.color;
            color.a = 0.3f;
            material.color = Color.black;
        }

        //StartCoroutine(changeTransparency.FadeTransparency(
        //    originalColor.a, targetAlpha, fadeDuration));

        rb.isKinematic = true;
        isInteractable = false;
    }

    public IEnumerator CaseRotation(Miniature miniature, float targetYRotation)
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


        // 정답!
        if (Mathf.Approximately(targetYRotation, 90f))
        {
            // Coroutine syncCoroutine = miniature.StartCoroutine(MiniatureSync.SyncTransform(miniature, 90f));
            // 이거 아마도 퍼즐에 반응하는 거로 가야 할 듯?
            MakeDisable();
            puzzleObj.SolvePuzzle();
        }
    }

    public Transform GetTarget() => target;
    public Rigidbody GetRigidbody() => rb;
    public float GetRotationSpeed() => rotationSpeed;
    public Coroutine GetRotationCoroutine() => rotationCoroutine;
    public void SetRotationCoroutine(Coroutine coroutine) => rotationCoroutine = coroutine;
}
