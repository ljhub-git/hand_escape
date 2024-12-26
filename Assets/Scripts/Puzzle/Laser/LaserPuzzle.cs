using System.Collections;
using UnityEngine;

public class LaserPuzzle : MonoBehaviour, IPuzzleObject
{
    [SerializeField]
    private Transform startPoint = null;

    private LineRenderer lr = null;

    private int maxBounces = 15;
    private float maxDistance = 15f;

    private Vector3 pos = Vector3.zero;
    private Vector3 dir = Vector3.zero;

    public void SolvePuzzle()
    {
        Debug.Log("SolvePuzzle");
    }
    public void ResetPuzzle()
    {
    }

    /// <summary>
    /// Laser �߻� �ڷ�ƾ ȣ��.
    /// </summary>
    public void StartLaserShoot()
    {
        StartCoroutine(LaserCoroutine());
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        StartLaserShoot();
    }

    /// <summary>
    /// ������ �߻� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaserCoroutine()
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;

        while(true)
        {
            pos = startPoint.position;
            dir = startPoint.forward;

            lr.SetPosition(0, pos);

            // �ִ� �ݻ� ������ Ƚ����ŭ �ݺ��ϸ鼭 ����ĳ��Ʈ�� �����Ѵ�.
            for (int i = 1; i <= maxBounces; ++i)
            {
                Ray ray = new Ray(pos, dir);
                RaycastHit hit;

                // ���̰� � ��ü�� ���� ���
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    pos = hit.point;
                    // ���� �������� ���� ������ ���� ������ �浹�� ����.
                    lr.SetPosition(i, pos);
                    // �浹�� ������ �븻�� �̿��ؼ� �ݻ� ������ �˾Ƴ���.
                    dir = Vector3.Reflect(dir, hit.normal);

                    // �ش� ��ü�� �������� ��ǥ ���̶�� ������ Ǯ����.
                    // �������� ��ǥ�� ���������ϱ� ������ �ݻ縦 �� �� �ص� ��.
                    if (hit.collider.GetComponent<LaserGoal>() != null)
                    {
                        for (int j = i + 1; j <= maxBounces; ++j)
                        {
                            lr.SetPosition(j, pos);
                        }

                        SolvePuzzle();
                        break;
                    }

                    // ���� �浹�ߴ� �ݶ��̴��� Mirror �±װ� ���ٸ� �ݻ簡 �� �ȴٴ� ��.
                    // �ݻ簡 �������� �ݺ��� Ż��
                    if (!hit.collider.CompareTag("Mirror"))
                    {
                        break;
                    }
                }
                // ���̰� �ƹ� ��ü�� �� �´´ٸ� �������� ���ݸ� �� �����Ű�� ����� �Ѵ�.
                else
                {
                    for (int j = i; j <= maxBounces; ++j)
                    {
                        // �ƹ� ��ü�� �� �¾��� ��� ���� �߻�� �������� �ִ�Ÿ���ŭ �� �� ���´�.
                        // Todo : ���ʿ��� �������� ���ؼ� ���� ���� �߻���. ���Ŀ� �̸� ���ľ� ��.
                        lr.SetPosition(j, pos + dir * maxDistance);
                    }
                    //�ƹ� ��ü�� �� �¾��� ��� ���� �߻�� �������� �ִ�Ÿ���ŭ �� �� ���´�.
                    // lr.SetPosition(i, pos + dir * maxDistance);
                    break;
                }
            }

            yield return null;
        }
    }

    //private void FixedUpdate()
    //{
    //    CastLaser();
    //}

    //private void CastLaser()
    //{
    //    pos = startPoint.position;
    //    dir = startPoint.forward;

    //    lr.SetPosition(0, pos);

    //    // �ִ� �ݻ� ������ Ƚ����ŭ �ݺ��ϸ鼭 ����ĳ��Ʈ�� �����Ѵ�.
    //    for (int i = 0; i < maxBounces; ++i)
    //    {
    //        Ray ray = new Ray(pos, dir);
    //        RaycastHit hit;

    //        // ���̰� � ��ü�� ���� ���
    //        if (Physics.Raycast(ray, out hit, maxDistance))
    //        {
    //            pos = hit.point;
    //            // �浹�� ������ �븻�� �̿��ؼ� �ݻ� ȿ���� ����Ų��.
    //            dir = Vector3.Reflect(dir, hit.normal);
    //            // ���� ���� ������ �浹�� ����.
    //            lr.SetPosition(i + 1, pos);

    //            // �ش� ��ü�� �������� ��ǥ ���̶�� ������ Ǯ����.
    //            // �������� ��ǥ�� ���������ϱ� ������ �ݻ縦 �� �� �ص� ��.
    //            if (hit.collider.GetComponent<LaserGoal>() != null)
    //            {
    //                SolvePuzzle();
    //                break;
    //            }

    //            // ���� �浹�ߴ� �ݶ��̴��� Mirror �±װ� ���ٸ� �ݻ簡 �� �ȴٴ� ��.
    //            // �ݻ簡 �������� �ݺ��� Ż��
    //            if (!hit.collider.CompareTag("Mirror"))
    //            {
    //                break;
    //            }
    //        }
    //        // ���̰� �ƹ� ��ü�� �� �´´ٸ� �������� ���ݸ� �� �����Ű�� ����� �Ѵ�.
    //        else
    //        {
    //            // ���� �������� ��ġ ī���͸� �������� �ʱ�ȭ���ش�.
    //            // �̰� �� ���ָ� ���� �������� ������ �̻��� ������ ��.
    //            lr.positionCount = i + 2;

    //            // �ƹ� ��ü�� �� �¾��� ��� ���� ���⿡�� �ִ�Ÿ���ŭ �� �� ���´�.
    //            lr.SetPosition(i + 1, pos + dir * maxDistance);

    //            break;
    //        }
    //    }
    //}
}
