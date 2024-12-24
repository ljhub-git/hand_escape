using UnityEngine;
using UnityEngine.UIElements;

public class LaserShooter : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint = null;

    int maxBounces = 20;
    private LineRenderer lr = null;

    private void Start()
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
                // �ش� ��ü�� �������� ��ǥ ���̶��
                if(hit.collider.GetComponent<LaserGoal>() != null)
                {
                    break;
                }

                _pos = hit.point;
                _dir = Vector3.Reflect(_dir, hit.normal);
                lr.SetPosition(i + 1, hit.point);

                // ���� �浹�ߴ� �ݶ��̴��� Mirror �±װ� ���ٸ� �ݻ簡 �� �ȴٴ� ��.
                // �ݻ簡 �������� �ݺ��� Ż��
                if(!hit.collider.CompareTag("Mirror"))
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
