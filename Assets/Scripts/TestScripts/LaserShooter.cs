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
        CastLaser(transform.position, transform.forward);
    }

    private void CastLaser(Vector3 _pos, Vector3 _dir)
    {
        lr.SetPosition(0, startPoint.position);

        Ray ray = new Ray(_pos, _dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 300))
        {
            _pos = hit.point;
            _dir = Vector3.Reflect(_dir, hit.normal);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(1, startPoint.position + _dir * 300f);
        }

        //for (int i = 0; i < maxBounces; ++i)
        //{
        //    Ray ray = new Ray(_pos, _dir);
        //    RaycastHit hit;

        //    if(Physics.Raycast(ray, out hit, 300, 1))
        //    {
        //        _pos = hit.point;
        //        _dir = Vector3.Reflect(_dir, hit.normal);
        //        lr.SetPosition(i + 1, hit.point);

        //        if(hit.transform.name != "Mirror" && reflectOnlyMirror)
        //        {
        //            for(int j = i + 1; j <= maxBounces; ++j)
        //            {
        //                lr.SetPosition(j, hit.point);
        //            }
        //            break;
        //        }
        //    }
        //}
    }
}
