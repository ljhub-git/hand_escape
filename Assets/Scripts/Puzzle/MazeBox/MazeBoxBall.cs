using UnityEngine;
using UnityEngine.Events;

public class MazeBoxBall : MonoBehaviour
{
    private PuzzleObject puzzleObj = null;

    public enum CubeType { Type99, Type55, Type333 }
    public CubeType cubeType;

    public float maxDelta = 0.01f;
    private Vector3 previousPosition;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.localPosition;

        puzzleObj = transform.parent.GetComponent<PuzzleObject>();
    }

    private void Update()
    {
        Vector3 currentLocalPosition = transform.localPosition;
        Vector3 delta = currentLocalPosition - previousPosition;

        // Check if the movement exceeds the threshold and adjust
        if (delta.magnitude > maxDelta)
        {
            // 큰 변화 감지 - 무시하거나 제한
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

        // 물체가 빠져나왔을 때 
        if (transform.localPosition.z < -0.25f)
        {
            ////부모오브젝트제거
            //transform.SetParent(null);
            //onBallPuzzleComplete.Invoke();
            //rb.isKinematic = false;

            ////현재스크립트 비활성화
            //this.enabled = false

            puzzleObj.SolvePuzzle();
        }
    }

    private Vector3 ApplyPositionLimits(Vector3 localPosition)
    {
        float xMin, xMax, yMin, yMax, zMin, zMax;

        // Cube type별 제한값 설정
        switch (cubeType)
        {
            case CubeType.Type99:
                xMin = -0.23f; xMax = 0.23f;
                yMin = -0.01f; yMax = 0.01f;
                zMin = Mathf.Abs(localPosition.x) < 0.0075f ? -0.3f : -0.23f;
                zMax = 0.23f;
                break;

            case CubeType.Type55:
                xMin = -0.145f; xMax = 0.145f;
                yMin = -0.025f; yMax = 0.025f;
                zMin = -0.145f; zMax = 0.145f;
                break;

            case CubeType.Type333:
                xMin = -0.15f; xMax = 0.15f;
                yMin = -0.03f; yMax = 0.03f;
                zMin = -0.2f; zMax = 0.2f;
                break;

            default:
                xMin = -0.23f; xMax = 0.23f;
                yMin = -0.01f; yMax = 0.01f;
                zMin = -0.23f; zMax = 0.23f;
                break;
        }

        localPosition.x = Mathf.Clamp(localPosition.x, xMin, xMax);
        localPosition.y = Mathf.Clamp(localPosition.y, yMin, yMax);
        localPosition.z = Mathf.Clamp(localPosition.z, zMin, zMax);

        return localPosition;
    }
}
