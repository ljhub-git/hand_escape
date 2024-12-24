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
        // �������� �������� ���� ����Ʈ�� ����.
        lr.SetPosition(0, startPoint.position);

        // �ִ� �ݻ� ������ Ƚ����ŭ �ݺ��ϸ鼭 ����ĳ��Ʈ�� �����Ѵ�.
        for (int i = 0; i < maxBounces; ++i)
        {
            Ray ray = new Ray(_pos, _dir);
            RaycastHit hit;

            // ���̰� � ��ü�� ���� ���
            if (Physics.Raycast(ray, out hit, 15f))
            {
                _pos = hit.point;
                _dir = Vector3.Reflect(_dir, hit.normal);
                lr.SetPosition(i + 1, hit.point);

                // �ش� ��ü�� �������� ��ǥ ���̶�� ������ Ǯ����.
                if (hit.collider.GetComponent<LaserGoal>() != null)
                {
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
                // ���� �������� ��ġ ī���͸� �������� �ʱ�ȭ���ش�.
                lr.positionCount = i + 2;

                lr.SetPosition(i + 1, _pos + _dir * 15f);

                break;
            }
        }
    }
}
