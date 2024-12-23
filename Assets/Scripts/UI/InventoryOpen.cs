using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField]
    private float m_MaxScale = 1.0f;

    [SerializeField]
    private float m_MinScale = 1.0f;

    [SerializeField]
    private float m_MaxDistance = 3.5f;

    [SerializeField]
    private float m_MinDistance = 0.25f;

    private Vector3 m_StartingScale = Vector3.one;

    public float distance = 0.3f;
    public float verticalOffset = -0.2f;

    public Transform positionSource;
    public void OpenKeyboard()
    {
        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        RepositionKeyboard(targetPosition);
    }

    public void RepositionKeyboard(Vector3 kbPos, float verticalOffset = 0.0f)
    {
        transform.position = kbPos;
        ScaleToSize();
        LookAtTargetOrigin();
    }

    private void ScaleToSize()
    {
        float distance = (transform.position - Camera.main.transform.position).magnitude;
        float distancePercent = (distance - m_MinDistance) / (m_MaxDistance - m_MinDistance);
        float scale = m_MinScale + (m_MaxScale - m_MinScale) * distancePercent;

        scale = Mathf.Clamp(scale, m_MinScale, m_MaxScale);
        transform.localScale = m_StartingScale * scale;

        Debug.LogFormat("Setting scale: {0} for distance: {1}", scale, distance);
    }

    private void LookAtTargetOrigin()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.up, 180.0f);
    }
}
