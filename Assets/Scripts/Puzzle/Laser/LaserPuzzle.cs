using UnityEngine;

public class LaserPuzzle : MonoBehaviour, IPuzzleObject
{
    [SerializeField]
    private LaserGoal goal = null;

    [SerializeField]
    private Transform startPoint = null;

    private LineRenderer lr = null;

    int maxBounces = 20;

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
        CastLaser(startPoint.position, startPoint.forward);
    }

    private void CastLaser(Vector3 _pos, Vector3 _dir)
    {
        // 레이저의 시작점은 시작 포인트로 고정.
        lr.SetPosition(0, startPoint.position);

        // 최대 반사 가능한 횟수만큼 반복하면서 레이캐스트를 시행한다.
        for (int i = 0; i < maxBounces; ++i)
        {
            Ray ray = new Ray(_pos, _dir);
            RaycastHit hit;

            // 레이가 어떤 물체에 맞을 경우
            if (Physics.Raycast(ray, out hit, 15f))
            {
                _pos = hit.point;
                _dir = Vector3.Reflect(_dir, hit.normal);
                lr.SetPosition(i + 1, hit.point);

                // 해당 물체가 레이저의 목표 골이라면 퍼즐이 풀린다.
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
                lr.positionCount = i + 2;

                lr.SetPosition(i + 1, _pos + _dir * 15f);

                break;
            }
        }
    }
}
