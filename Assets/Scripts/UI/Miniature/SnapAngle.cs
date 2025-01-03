using UnityEngine;

public class SnapAngle
{
    private float lastSnapAngle = -1f;

    public float GetSnapAngle(float currentYRotation, float[] snapAngles, float snapThreshold)
    {
        foreach (float snapAngle in snapAngles)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(currentYRotation, snapAngle)) <= snapThreshold && !Mathf.Approximately(lastSnapAngle, snapAngle))
            {
                lastSnapAngle = snapAngle;
                return snapAngle;
            }
        }
        return -1f;
    }

    public float NormalizeAngle(float angle)
    {
        angle %= 360f;
        return angle < 0 ? angle + 360f : angle;
    }
}
