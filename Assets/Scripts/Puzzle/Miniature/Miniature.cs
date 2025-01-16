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

    public AudioClip effectClip;  // 재생할 효과음
    public AudioClip effectClip2;  // 재생할 효과음

    private AudioSource audioSource;

    private void Awake()
    {
        puzzleObj = GetComponent<PuzzleObject>();
    }

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


        // AudioSource 컴포넌트를 가져옴
        audioSource = FindAnyObjectByType<AudioSource>();
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

        audioSource.PlayOneShot(effectClip2);
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
        PlayEffect();

        yield return new WaitForSeconds(1f);

        if (rb != null) rb.isKinematic = false;
        miniature.SetRotationCoroutine(null);


        // 정답!
        if (Mathf.Approximately(targetYRotation, 90f))
        {
            // Coroutine syncCoroutine = miniature.StartCoroutine(MiniatureSync.SyncTransform(miniature, 90f));
            MakeDisable();
            puzzleObj.SolvePuzzle();
        }
    }

    // 효과음을 재생하는 메서드
    public void PlayEffect()
    {
        if (effectClip != null)
        {
            audioSource.PlayOneShot(effectClip);
        }
        else
        {
            Debug.LogWarning("효과음 클립이 설정되지 않았습니다!");
        }
    }


    public Transform GetTarget() => target;
    public Rigidbody GetRigidbody() => rb;
    public float GetRotationSpeed() => rotationSpeed;
    public Coroutine GetRotationCoroutine() => rotationCoroutine;
    public void SetRotationCoroutine(Coroutine coroutine) => rotationCoroutine = coroutine;
}
