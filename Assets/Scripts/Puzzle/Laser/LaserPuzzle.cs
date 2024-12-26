using UnityEngine;

public class LaserPuzzle : MonoBehaviour, IPuzzleObject
{
    [SerializeField]
    private Transform startPoint = null;

    private LineRenderer lr = null;

    private int maxBounces = 20;
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

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        CastLaser();
    }

    private void CastLaser()
    {
        pos = startPoint.position;
        dir = startPoint.forward;

        lr.SetPosition(0, pos);

        // 최대 반사 가능한 횟수만큼 반복하면서 레이캐스트를 시행한다.
        for (int i = 0; i < maxBounces; ++i)
        {
            Ray ray = new Ray(pos, dir);
            RaycastHit hit;

            Debug.DrawRay(pos, dir * maxDistance, Color.yellow);

            // 레이가 어떤 물체에 맞은 경우
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                pos = hit.point;
                // 충돌한 지점의 노말을 이용해서 반사 효과를 일으킨다.
                dir = Vector3.Reflect(dir, hit.normal);
                // 다음 시작 지점은 충돌한 지점.
                lr.SetPosition(i + 1, pos);

                // 해당 물체가 레이저의 목표 골이라면 퍼즐이 풀린다.
                // 레이저의 목표에 도달했으니까 레이저 반사를 더 안 해도 됨.
                if (hit.collider.GetComponent<LaserGoal>() != null)
                {
                    SolvePuzzle();
                    break;
                }

                // 만약 충돌했던 콜라이더가 Mirror 태그가 없다면 반사가 안 된다는 뜻.
                // 반사가 끝났으니 반복문 탈출
                if (!hit.collider.CompareTag("Mirror"))
                {
                    break;
                }
            }
            // 레이가 아무 물체에 안 맞는다면 레이저를 조금만 더 진행시키고 멈춰야 한다.
            else
            {
                // 라인 렌더러의 위치 카운터를 동적으로 초기화해준다.
                // 이거 안 해주면 라인 렌더러의 끝점이 이상한 곳으로 감.
                lr.positionCount = i + 2;

                // 아무 물체에 안 맞았을 경우 라인 방향에서 최대거리만큼 간 뒤 끊는다.
                lr.SetPosition(i + 1, pos + dir * maxDistance);

                break;
            }
        }
    }
}
