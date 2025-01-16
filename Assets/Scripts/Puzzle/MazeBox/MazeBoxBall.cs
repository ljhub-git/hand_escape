using UnityEngine;
using UnityEngine.Events;

public class MazeBoxBall : MonoBehaviour
{
    private PuzzleObject puzzleObj = null;

    public enum CubeType { Type99, Type55, Type333 }
    public CubeType cubeType;

    public float maxDelta = 0.015f;
    private Vector3 previousPosition;
    private Rigidbody rb;

    private bool isSolved = false;

    private const float BallOffset = 0.015f;
    private static readonly Vector3 Type99MinLimits = new Vector3(-0.245f, -0.025f, -0.245f);
    private static readonly Vector3 Type99MaxLimits = new Vector3(0.245f, 0.025f, 0.245f);

    private static readonly Vector3 Type55MinLimits = new Vector3(-0.145f, -0.025f, -0.145f);
    private static readonly Vector3 Type55MaxLimits = new Vector3(0.145f, 0.025f, 0.145f);

    private static readonly Vector3 Type333MinLimits = new Vector3(-0.095f, -0.095f, -0.095f);
    private static readonly Vector3 Type333MaxLimits = new Vector3(0.095f, 0.095f, 0.095f);


    public AudioClip effectClip;  // ����� ȿ����
    public AudioClip effectClip2;  // ����� ȿ����

    private AudioSource audioSource;

    public ParticleSystem puzzleCompleteEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.localPosition;

        puzzleObj = transform.parent.GetComponent<PuzzleObject>();

        // AudioSource ������Ʈ�� ������
        audioSource = FindAnyObjectByType<AudioSource>();
    }

    private void Update()
    {
        Vector3 currentLocalPosition = transform.localPosition;
        Vector3 delta = currentLocalPosition - previousPosition;

        // Check if the movement exceeds the threshold and adjust
        //if (delta.magnitude > maxDelta)
        if (delta.sqrMagnitude > maxDelta * maxDelta)
        {
            // ū ��ȭ ���� - �����ϰų� ����
            rb.linearVelocity = Vector3.zero;
            //rb.isKinematic = true;
            currentLocalPosition = previousPosition + delta.normalized * maxDelta;
            //currentLocalPosition = previousPosition;
        }
        else
        {
            //rb.isKinematic = false;
        }
        transform.localPosition = ApplyPositionLimits(currentLocalPosition);
        previousPosition = transform.localPosition;

        // ��ü�� ���������� �� 
        if (isSolved)
        {
            ////�θ������Ʈ����
            //transform.SetParent(null);
            //onBallPuzzleComplete.Invoke();
            //rb.isKinematic = false;

            ////���罺ũ��Ʈ ��Ȱ��ȭ
            //this.enabled = false
            PlayPuzzleCompleteEffect(); // ��ƼŬ ȿ�� ����
            puzzleObj.SolvePuzzle();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            PlayPuzzleCompleteEffect(); // ��ƼŬ ȿ�� ����
            puzzleObj.SolvePuzzle();
        }
    }

    private Vector3 ApplyPositionLimits(Vector3 localPosition)
    {
        Vector3 minLimits, maxLimits;

        // CubeType�� ���� ���Ѱ� ����
        switch (cubeType)
        {
            case CubeType.Type99:
                minLimits = Type99MinLimits;
                maxLimits = Type99MaxLimits;
                break;

            case CubeType.Type55:
                minLimits = Type55MinLimits;
                maxLimits = Type55MaxLimits;
                break;

            case CubeType.Type333:
                minLimits = Type333MinLimits;
                maxLimits = Type333MaxLimits;
                break;

            default:
                minLimits = Type99MinLimits;
                maxLimits = Type99MaxLimits;
                break;
        }

        // ������ ����
        minLimits += Vector3.one * BallOffset;
        maxLimits -= Vector3.one * BallOffset;

        // ��ġ ���� ����
        return new Vector3(
            Mathf.Clamp(localPosition.x, minLimits.x, maxLimits.x),
            Mathf.Clamp(localPosition.y, minLimits.y, maxLimits.y),
            Mathf.Clamp(localPosition.z, minLimits.z, maxLimits.z)
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("222");
        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("333");
            isSolved = true;
            audioSource.PlayOneShot(effectClip2);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    PlayEffect();
    //}

    public void PlayEffect()
    {
        if (effectClip != null)
        {
            audioSource.PlayOneShot(effectClip);
        }
        else
        {
            Debug.LogWarning("ȿ���� Ŭ���� �������� �ʾҽ��ϴ�!");
        }
    }

    public void PlayPuzzleCompleteEffect()
    {
        if (puzzleCompleteEffect != null)
        {
            puzzleCompleteEffect.transform.position = transform.position; // ��ƼŬ ��ġ ����
            puzzleCompleteEffect.Play();
        }
        else
        {
            Debug.LogWarning("Puzzle Complete Effect�� �������� �ʾҽ��ϴ�!");
        }
    }
}
