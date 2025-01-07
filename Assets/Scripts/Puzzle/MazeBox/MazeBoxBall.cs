using UnityEngine;
using UnityEngine.Events;

public class MazeBoxBall : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab = null;

    private MazeBoxManager mazeBoxMng = null;

    public float maxDelta = 0.01f;
    private Vector3 previousPosition;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.localPosition;

        mazeBoxMng = transform.parent.GetComponent<MazeBoxManager>();
    }

    private void Update()
    {
        Vector3 currentLocalPosition = transform.localPosition;
        Vector3 delta = currentLocalPosition - previousPosition;

        // Check if the movement exceeds the threshold and adjust
        if (delta.magnitude > maxDelta)
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
        if (transform.localPosition.z < -0.25f)
        {
            ////�θ������Ʈ����
            //transform.SetParent(null);
            //onBallPuzzleComplete.Invoke();
            //rb.isKinematic = false;

            ////���罺ũ��Ʈ ��Ȱ��ȭ
            //this.enabled = false

            mazeBoxMng.SolvePuzzle();
        }
    }

    private Vector3 ApplyPositionLimits(Vector3 localPosition)
    {
        float xMin = -0.23f, xMax = 0.23f;
        float yMin = -0.01f, yMax = 0.01f;
        
        float zMin = Mathf.Abs(localPosition.x) < 0.0075f ? -0.3f : -0.23f;
        float zMax = 0.23f;

        localPosition.x = Mathf.Clamp(localPosition.x, xMin, xMax);
        localPosition.y = Mathf.Clamp(localPosition.y, yMin, yMax);
        localPosition.z = Mathf.Clamp(localPosition.z, zMin, zMax);

        return localPosition;
    }
}
