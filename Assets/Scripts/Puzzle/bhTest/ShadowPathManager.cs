using UnityEngine;

public class ShadowPathManager : MonoBehaviour
{
    [SerializeField]
    private Transform startTr = null;
    [SerializeField]
    private Transform endTr = null;
    [SerializeField]
    private Transform character2DTr = null;

    [SerializeField]
    private Transform start3DTr = null;
    [SerializeField]
    private Transform end3DTr = null;
    [SerializeField]
    private Transform character3DTr = null;


    private float xLength2D = 0f;
    //private float yLength2D = 0f;

    private float xLength3D = 0f;
    //private float yLength3D = 0f;

    private float ratio = 0f;

    private void Start()
    {
        xLength2D = Mathf.Abs(startTr.position.x - endTr.position.x);
        xLength3D = Mathf.Abs(start3DTr.position.x - end3DTr.position.x);
    }

    private void Update()
    {
        ratio = 1f - (character3DTr.localPosition.x / xLength3D);

        Vector3 pos = character2DTr.position;
        pos.x = Mathf.Lerp(startTr.position.x, endTr.position.x, ratio);

        character2DTr.position = pos;
    }
}
