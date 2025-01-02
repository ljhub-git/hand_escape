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
    /// Laser 발사 코루틴 호출.
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
    /// 레이저 발사 코루틴.
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
            // 최대 반사 가능한 횟수만큼 반복하면서 레이캐스트를 시행한다.
            for (int i = 1; i <= maxBounces; ++i)
            {
                Ray ray = new Ray(pos, dir);
                RaycastHit hit;

                // 레이가 어떤 물체에 맞은 경우
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    pos = hit.point;
                    // 도착 지점이자 다음 라인의 시작 지점은 충돌한 지점.
                    lr.SetPosition(i, pos);

                    // 해당 물체가 레이저의 목표 골이라면 퍼즐이 풀린다.
                    // 레이저의 목표에 도달했으니까 레이저 반사를 더 안 해도 됨.
                    if (hit.collider.GetComponent<LaserGoal>() != null)
                    {
                        for (int j = i + 1; j <= maxBounces; ++j)
                        {
                            lr.SetPosition(j, pos);
                        }

                        SolvePuzzle();
                        break;
                    }

                    // 만약 충돌했던 콜라이더가 Mirror 태그가 없다면 반사가 안 된다는 뜻.
                    // 반사가 끝났으니 반복문 탈출
                    if (!hit.collider.CompareTag("Mirror"))
                    {
                        for (int j = i + 1; j <= maxBounces; ++j)
                        {
                            lr.SetPosition(j, pos);
                        }

                        break;
                    }

                    // 이 두 경우가 아니면 거울에 맞은 거니까 충돌한 지점의 노말을 이용해서 반사 방향을 알아낸다.
                    dir = Vector3.Reflect(dir, hit.normal);
                }
                // 레이가 아무 물체에 안 맞는다면 레이저를 조금만 더 진행시키고 멈춰야 한다.
                else
                {
                    for (int j = i; j <= maxBounces; ++j)
                    {
                        // 아무 물체에 안 맞았을 경우 현재 발사된 방향으로 최대거리만큼 간 뒤 끊는다.
                        // Todo : 불필요한 연산으로 인해서 성능 저하 발생중. 추후에 이를 고쳐야 함.
                        lr.SetPosition(j, pos + dir * maxDistance);
                    }
                    break;
                }
            }
            yield return null;
        }
    }

}
