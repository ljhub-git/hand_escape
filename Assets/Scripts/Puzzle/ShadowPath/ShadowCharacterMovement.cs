using UnityEngine;

public class ShadowCharacterMovement : MonoBehaviour
{
    [SerializeField]
    private Transform startTr = null;
    [SerializeField]
    private Transform endTr = null;
    [SerializeField]
    private Transform character2DTr = null;

    private float xLength2D = 0f;

    public void MovebyRatio(float _ratio)
    {
        Vector3 pos = character2DTr.position;
        pos.x = Mathf.Lerp(startTr.position.x, endTr.position.x, _ratio);

        character2DTr.position = pos;
    }

    private void Start()
    {
        xLength2D = Mathf.Abs(startTr.position.x - endTr.position.x);
    }
}
