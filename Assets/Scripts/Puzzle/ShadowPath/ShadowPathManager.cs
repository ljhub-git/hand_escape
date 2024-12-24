using UnityEngine;

public class ShadowPathManager : MonoBehaviour
{
    [SerializeField]
    private Transform start3DTr = null;
    [SerializeField]
    private Transform end3DTr = null;
    [SerializeField]
    private Transform character3DTr = null;

    private float xLength3D = 0f;

    private float ratio = 0f;

    private void Start()
    {
        xLength3D = start3DTr.position.x - end3DTr.position.x;
    }

    private void Update()
    {
        if(xLength3D < 0)
            ratio = character3DTr.localPosition.x / xLength3D;
        else
            ratio = 1f - (character3DTr.localPosition.x / xLength3D);
    }
}
