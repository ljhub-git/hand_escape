using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzzle : PuzzleObject
{
    [SerializeField]
    private Transform startPoint = null;

    private LineRenderer lr = null;

    private int maxBounces = 15;
    private float maxDistance = 15f;

    private Vector3 pos = Vector3.zero;
    private Vector3 dir = Vector3.zero;

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

        while (true)
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
                        for (int j = i + 1; j <= maxBounces; ++j)
                        {
                            lr.SetPosition(j, pos);
                        }

                        break;
                    }

                    // �� �� ��찡 �ƴϸ� �ſ￡ ���� �Ŵϱ� �浹�� ������ �븻�� �̿��ؼ� �ݻ� ������ �˾Ƴ���.
                    dir = Vector3.Reflect(dir, hit.normal);
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
                    break;
                }
            }
            yield return null;
        }
    }

}
