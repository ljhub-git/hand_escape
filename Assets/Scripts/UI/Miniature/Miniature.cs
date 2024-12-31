using UnityEngine;

public class Miniature : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float snapThreshold = 30f; // Threshold for snapping (e.g., within 10 degrees)
    [SerializeField] private float[] snapAngles = { 0f, 90f, 180f, 270f}; // Snap angles

    private float lastSnapAngles = -1f;

    private void Update()
    {
        float currentYRotation = NormalizeAngle(transform.eulerAngles.y);
        float snappedAngle = GetSnapAngle(currentYRotation);

        if (snappedAngle >= 0)
        {
            transform.eulerAngles = new Vector3(0, snappedAngle, 0);
        }
        
        SyncTransform();

    }

    private float GetSnapAngle(float currentYRotation)
    {
        foreach (float snapAngle in snapAngles)
        {
            //Debug.Log(Mathf.Abs(Mathf.DeltaAngle(currentYRotation, snapAngle)));
            if (Mathf.Abs(Mathf.DeltaAngle(currentYRotation, snapAngle)) <= snapThreshold && !Mathf.Approximately(lastSnapAngles, snapAngle))
            {
                lastSnapAngles = snapAngle;
                return snapAngle;
            }
        }
        return -1f; // No snapping condition met
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        return angle < 0 ? angle + 360f : angle;
    }

    private void SyncTransform()
    {
        target.rotation = transform.rotation;
    }

}
