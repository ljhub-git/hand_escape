using UnityEngine;
using UnityEngine.UIElements;

public class LaserShooter : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint = null;
    [SerializeField]
    private bool reflectOnlyMirror = true;

    int maxBounces = 20;
    private LineRenderer lr = null;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, startPoint.position);
    }

    private void Update()
    {
        CastLaser(startPoint.position, startPoint.forward);
    }

    private void CastLaser(Vector3 _pos, Vector3 _dir)
    {
        lr.SetPosition(0, startPoint.position);

        for (int i = 0; i < maxBounces; ++i)
        {
            Ray ray = new Ray(_pos, _dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 15f))
            {
                _pos = hit.point;
                _dir = Vector3.Reflect(_dir, hit.normal);
                lr.SetPosition(i + 1, hit.point);

                if(!hit.collider.CompareTag("Mirror"))
                {
                    break;
                }
            }
            else
            {
                lr.SetPosition(i + 1, _pos + _dir * 15f);
                break;
            }
        }
    }
}
